using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;

namespace TrackApp.Logic.Gps
{
    /// <summary>
    /// GPS route and calculations
    /// </summary>
    public sealed class GPSData
    {
        //TODO: use google maps api for drawing, calculating, etc
        public static double longitudeCorrectionScale = 1;

        private static GPSData _instance;

        private GPSPoint[] gpsPoints;
        private Vector vector = new Vector(0, 0); // use the previous values when the direction length is 0
        private GPSBox boundingBox;

        private GPSData()
        {
        }

        public static GPSData GetData()
        {
            if (_instance == null)
            {
                _instance = new GPSData();
            }

            return _instance;
        }

        /// <summary>
        /// Gets the count of all the points in the route
        /// </summary>
        public int GetPointCount()
        {
            return this.gpsPoints.Length;
        }

        //TODO: this needs heavy refactoring!
        /// <summary>
        /// Populates the gpsPoints array and trims the rout, calculates the bounding box (contains the entire track), magick
        /// </summary>
        /// <param name="pts"></param>
        public void Update(List<GPSPoint> pts)
        {
            double maxLong = double.MinValue;
            double maxLat = double.MinValue;
            double maxEle = double.MinValue;

            double minLong = double.MaxValue;
            double minLat = double.MaxValue;
            double minEle = double.MaxValue;
            var settigs = ProjectSettings.GetSettings();

            // trim track
            if (settigs.TrackEnd == 0)
            {
                settigs.TrackEnd = pts[pts.Count - 1].Time;
            }

            int trackStartPos = pts.FindIndex(0, p => p.Time >= settigs.TrackStart);

            // trackStartPos==-1 && trackEndPos==0 we have no coords that overlap the movie
            if (trackStartPos == -1)
            {
                trackStartPos = 0;
            }

            int trackEndPos = pts.FindIndex(trackStartPos, p => p.Time > settigs.TrackEnd);
            if (trackEndPos == -1)
            {
                trackEndPos = pts.Count - 1;
            }

            pts.RemoveRange(0, trackStartPos);
            trackEndPos = trackEndPos - trackStartPos;
            pts.RemoveRange(trackEndPos, pts.Count - trackEndPos);

            // TODO: use lists
            this.gpsPoints = pts.ToArray();

            if (this.gpsPoints.Length == 0)
            {
                throw new EmptyTrackException("The generated track is empty! Try the default sync settings or another track file.");
            }

            float statTime = gpsPoints[0].Time;

            foreach (GPSPoint point in gpsPoints)
            {
                // find BoundingBox values
                point.Time -= statTime;
                maxLong = Math.Max(point.Longitude, maxLong);
                maxLat = Math.Max(point.Latitude, maxLat);
                maxEle = Math.Max(point.Elevation, maxEle);

                minLong = Math.Min(point.Longitude, minLong);
                minLat = Math.Min(point.Latitude, minLat);
                minEle = Math.Min(point.Elevation, minEle);
            }

            GPSCoord boundingBoxPosition = new GPSCoord(minLong, minLat, minEle);
            GPSCoord boundingBoxSize = new GPSCoord(maxLong - minLong, maxLat - minLat, maxEle - minEle);
            this.boundingBox = new GPSBox(boundingBoxPosition, boundingBoxSize);

            // for drawing maps
            longitudeCorrectionScale = Math.Cos((Math.PI / 180) * (maxLat + minLat) / 2);

            // distances from start point
            double distanceSum = 0;
            for (int i = 1; i < this.gpsPoints.Length; i++)
            {
                distanceSum += this.gpsPoints[i].DistanceFromPoint(this.gpsPoints[i - 1]);
                this.gpsPoints[i].Distance = distanceSum;
            }
        }

        // TODO average speed (gradually change speed)
        /// <summary>
        /// Gets the speed in kph
        /// </summary>
        /// <param name="time">time in the track</param>
        /// <returns>speed in kph</returns>
        public double GetSpeed(float time)
        {
            int index = this.GetTrackPointIndex(time);
            if ((index == this.gpsPoints.Length - 1) || (this.gpsPoints.Length == 0))
            {
                return 0;
            }

            double distance = this.gpsPoints[index].DistanceFromPoint(this.gpsPoints[index + 1]);
            float timeSpan = this.gpsPoints[index + 1].Time - this.gpsPoints[index].Time;

            return distance / timeSpan;
        }

        /// <summary>
        /// Gets the smallest size of a box that fits the track
        /// </summary>
        /// <returns>The differencebetween max and min latitude, longitude and elevation</returns>
        public GPSBox GetBox()
        {
            return this.boundingBox;
        }

        /// <summary>
        /// Returns the position on a track at a given time
        /// </summary>
        /// <param name="time">time in seconds</param>
        /// <returns>GPS position (latitude, longitude and altitude)</returns>
        public GPSCoord GetPosition(float time)
        {
            int index = this.GetTrackPointIndex(time);
            if (index == this.gpsPoints.Length - 1)
            {
                return new GPSCoord(this.gpsPoints[this.gpsPoints.Length - 1].Longitude, this.gpsPoints[this.gpsPoints.Length - 1].Latitude, this.gpsPoints[this.gpsPoints.Length - 1].Elevation);
            }

            double Long = this.Interpolate(time, this.gpsPoints[index].Longitude, this.gpsPoints[index + 1].Longitude, this.gpsPoints[index].Time, this.gpsPoints[index + 1].Time);
            //double Long = this.gpsPoints[index].Longitude;
            double lat = this.Interpolate(time, this.gpsPoints[index].Latitude, this.gpsPoints[index + 1].Latitude, this.gpsPoints[index].Time, this.gpsPoints[index + 1].Time);
            //double lat = this.gpsPoints[index].Latitude;
            double ele = this.Interpolate(time, this.gpsPoints[index].Elevation, this.gpsPoints[index + 1].Elevation, this.gpsPoints[index].Time, this.gpsPoints[index + 1].Time);
            return new GPSCoord(Long, lat, ele);
        }

        /// <summary>
        /// Returns the current orientation/direction on the track based on the current and previous positions as a new coordinate system
        /// </summary>
        /// <param name="time">Time in seconds</param>
        /// <param name="length">The length of the vector</param>
        /// <returns>Two perpendicular vectors</returns>
        public Vector[] GetOrientation(float time, double length = 1)
        {
            int index = this.GetTrackPointIndex(time);
            double longDirection = (this.gpsPoints[index + 1].Longitude - this.gpsPoints[index].Longitude) * longitudeCorrectionScale;
            double latDirection = this.gpsPoints[index + 1].Latitude - this.gpsPoints[index].Latitude;

            double lengthVector = Math.Sqrt(longDirection * longDirection + latDirection * latDirection);
            if (lengthVector != 0) // if false use previous value
            {
                // Normalizing vector
                this.vector = new Vector(longDirection, latDirection) * length / lengthVector;
            }

            Vector perpVector = new Vector(this.vector.Y, -this.vector.X);

            Vector[] orientation =
            {
                this.vector,
                perpVector
            };

            return orientation;
        }

        /// <summary>
        /// Returns the distance traveled until the current time in meters.
        /// </summary>
        /// <param name="time">Time in seconds</param>
        /// <returns>Distance in meters</returns>
        public double GetDistance(float time)
        {
            int index = this.GetTrackPointIndex(time);
            double distance = this.gpsPoints[index].Distance;
            GPSCoord lastPosition = this.GetPosition(time);
            GPSPoint interpolatedPos = new GPSPoint(lastPosition.Longitude, lastPosition.Latitude, lastPosition.Elevation, 0, 0);
            return distance += interpolatedPos.DistanceFromPoint(this.gpsPoints[index]);
        }

        /// <summary>
        /// Method for getting the track
        /// </summary>
        /// <returns>The track as an array of type GPSPoint</returns>
        public GPSPoint[] GetTrack()
        {
            return this.gpsPoints;
        }

        /// <summary>
        /// Returns the nearest GPS position recorded at or before a certain time
        /// </summary>
        /// <param name="time">Time in seconds</param>
        /// <returns>GPS position</returns>
        public int GetTrackPointIndex(float time)
        {
            int index = Array.BinarySearch(this.gpsPoints, new GPSPoint(time));
            if (index < 0)
            {
                // the value wasn't found
                index = ~index;
                index -= 1;
            }

            if (index < 0)
            {
                //the value for the first index was't found
                index = 0;
            }

            return index;
        }
        
        /// <summary>
        /// Converts a GPS coordinate to pixel position for drawing on a map.
        /// </summary>
        /// <param name="GpsPosition">GPS position</param>
        /// <param name="size">Map size in pixels</param>
        /// <param name="border">Size of the object to be drawn in pixels</param>
        /// <returns>A PointF structure with floating point X and Y components</returns>
        public PointF ToPixelCoordinate(GPSCoord GpsPosition, SizeF size, int border = 0)
        {
            double ratio = size.Height / this.boundingBox.Size.Latitude;
            return new PointF(
                (float)((GpsPosition.Longitude - this.boundingBox.Position.Longitude) * ratio *
                    longitudeCorrectionScale) + ((float)border) / 2,
                size.Height - (float)((GpsPosition.Latitude - this.boundingBox.Position.Latitude) * ratio) +
                    ((float)border) / 2);
        }

        //TODO work with GpsPoint
        /// <summary>
        /// Interpolates getween two readings for a more gradual transition between points.
        /// </summary>
        private double Interpolate(float time, double previousReading, double nextReading, float previousTime, float nextTime)
        {
            if (nextTime == previousTime)
            {
                throw new DivideByZeroException("Two readings were taken at the same time");
            }

            return previousReading + (nextReading - previousReading) * (time - previousTime) / (nextTime - previousTime);
        }
    }
}