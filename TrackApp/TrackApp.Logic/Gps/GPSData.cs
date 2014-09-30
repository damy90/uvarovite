using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;

namespace TrackApp.Logic.Gps
{
    public sealed class GPSData
    {
        private GPSPoint[] gpsPoints;

        public static double LongtitudeCorrectionScale = 1;

        private static GPSData _instance;
        private Vector vector = new Vector(0, 0);//use the previous values when the direction length is 0
        GPSBox boundingBox;

        private GPSData()
        {
        }

        public static GPSData GetData()
        {
            if (_instance == null)
                _instance = new GPSData();
            return _instance;
        }

        public int GetPointCount()
        {
            return this.gpsPoints.Length;
        }

        public void LoadGPXFile(string sFile)
        {
            //
        }

        public void Update(List<GPSPoint> pts)
        {
            double maxLong = double.MinValue;
            double maxLat = double.MinValue;
            double maxEle = double.MinValue;

            double minLong = double.MaxValue;
            double minLat = double.MaxValue;
            double minEle = double.MaxValue;
            var settigs = ProjectSettings.GetSettings();
            //trim track
            if (settigs.TrackEnd == 0)
                settigs.TrackEnd = pts[pts.Count - 1].Time;
            int trackStartPos = pts.FindIndex(0, p => p.Time >= settigs.TrackStart);
            //todo - handle trackStartPos==-1 || trackEndPos==0 we have no coords that overlap the movie
            if (trackStartPos == -1)
                trackStartPos = 0;
            int trackEndPos = pts.FindIndex(trackStartPos, p => p.Time > settigs.TrackEnd);
            if (trackEndPos == -1)
                trackEndPos = pts.Count - 1;

            pts.RemoveRange(0, trackStartPos);
            trackEndPos = trackEndPos - trackStartPos;
            pts.RemoveRange(trackEndPos, pts.Count - trackEndPos);

            this.gpsPoints = new GPSPoint[pts.Count];//we need an array rather than list for faster access
        
            if (this.gpsPoints.Length == 0)
                throw new EmptyTrackException("The generated track is empty! Try default sync settings or another track file.");
            int n = 0;
            foreach (GPSPoint point in pts)
            {
                //coppy to array
                this.gpsPoints[n] = point;
                n++;
                //find BoundingBox valuse
                maxLong = Math.Max(point.Longitude, maxLong);
                maxLat = Math.Max(point.Latitude, maxLat);
                maxEle = Math.Max(point.Elevation, maxEle);

                minLong = Math.Min(point.Longitude, minLong);
                minLat = Math.Min(point.Latitude, minLat);
                minEle = Math.Min(point.Elevation, minEle);
            }
            GPSCoord position = new GPSCoord(minLong, minLat, minEle);
            GPSCoord size = new GPSCoord(maxLong - minLong, maxLat - minLat, maxEle - minEle);
            this.boundingBox = new GPSBox(position, size);
            //for drawing maps
            LongtitudeCorrectionScale = Math.Cos((Math.PI / 180) * (maxLat + minLat) / 2);
            //distances from start point
            double distanceSum = 0;
            for (int i = 1; i < this.gpsPoints.Length; i++)
            {
                distanceSum += this.gpsPoints[i].DistanceFromPoint(this.gpsPoints[i - 1]);
                this.gpsPoints[i].Distance = distanceSum;
            }
        }

        //generate the UpdateGPSData event
        //UpdateGPSData(this, EventArgs.Empty);
        //TODO average speed (gradually change speed)
        public double GetSpeed(float time)
        {
            int index = this.GetIndex(time);
            if ((index == this.gpsPoints.Length - 1) || (this.gpsPoints.Length == 0))
                return 0;
            double distance = this.gpsPoints[index].DistanceFromPoint(this.gpsPoints[index + 1]);
            float timeSpan = this.gpsPoints[index + 1].Time - this.gpsPoints[index].Time;
            return distance / timeSpan;
        }

        public GPSBox GetBox()
        {
            return this.boundingBox;
        }

        public GPSCoord GetPosition(float time)
        {
            int index = this.GetIndex(time);
            if (index == this.gpsPoints.Length - 1)
                return new GPSCoord(this.gpsPoints[this.gpsPoints.Length - 1].Longitude, this.gpsPoints[this.gpsPoints.Length - 1].Latitude, this.gpsPoints[this.gpsPoints.Length - 1].Elevation);
            double Long = this.Interpolate(time, this.gpsPoints[index].Longitude, this.gpsPoints[index + 1].Longitude, this.gpsPoints[index].Time, this.gpsPoints[index + 1].Time);
            double lat = this.Interpolate(time, this.gpsPoints[index].Latitude, this.gpsPoints[index + 1].Latitude, this.gpsPoints[index].Time, this.gpsPoints[index + 1].Time);
            double ele = this.Interpolate(time, this.gpsPoints[index].Elevation, this.gpsPoints[index + 1].Elevation, this.gpsPoints[index].Time, this.gpsPoints[index + 1].Time);
            return new GPSCoord(Long, lat, ele);
        }

        public Vector[] GetOrientation(float time, double length=1)
        {
            int index = this.GetIndex(time);
            double longDirection = (this.gpsPoints[index + 1].Longitude - this.gpsPoints[index].Longitude) * LongtitudeCorrectionScale;
            double latDirection = this.gpsPoints[index + 1].Latitude - this.gpsPoints[index].Latitude;

            double lengthVector = Math.Sqrt(longDirection * longDirection + latDirection * latDirection);
            if (lengthVector != 0)
                this.vector = new Vector(longDirection, latDirection) * length / lengthVector;
            Vector perpVector = new Vector(this.vector.Y, -this.vector.X);

            Vector[] orientation =
            {
                this.vector,
                perpVector
            };
            return orientation;
        }

        public double GetDistance(float time)
        {
            int index = this.GetIndex(time);
            double distance = this.gpsPoints[index].Distance;
            GPSCoord lastPosition = this.GetPosition(time);
            GPSPoint interpolatedPos = new GPSPoint(lastPosition.Longitude, lastPosition.Latitude, lastPosition.Elevation, 0, 0);
            return distance += interpolatedPos.DistanceFromPoint(this.gpsPoints[index]);
        }

        public GPSPoint[] GetTrack()
        {
            return this.gpsPoints;
        }

        //In case some readings are made over greater time intervals (if you are in a tunel or loose signal, etc)
        public int GetIndex(float time)
        {
            int index = Array.BinarySearch(this.gpsPoints, new GPSPoint(time));
            if (index < 0) // the value wasn't found
            {
                index = ~index;
                index -= 1;
            }
            return index;
        }

        //converts a GPS coordinate to pixel coordinate
        //size and return value are in pixels
        public PointF ToPixelCoordinate(GPSCoord pt, SizeF size, int border = 0)
        {
            double ratio = size.Height / (this.boundingBox.Size.Latitude);
            return new PointF((float)((pt.Longitude - this.boundingBox.Position.Longitude) * ratio * LongtitudeCorrectionScale) + ((float)border) / 2,
                size.Height - (float)((pt.Latitude - this.boundingBox.Position.Latitude) * ratio) + ((float)border) / 2);
        }

        private double Interpolate(float time, double previousReading, double nextReading, float previousTime, float nextTime)
        {
            if (nextTime == previousTime)
                throw new DivideByZeroException("Division by zero: Two readings were taken at the same time");
            return previousReading + (nextReading - previousReading) * (time - previousTime) / (nextTime - previousTime);
        }
    }
}