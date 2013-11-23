using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace TrackApp
{
    public struct GPSPoint
    {
        public double longitude;
        public double latitude;
        public double elevation;
        public float time;
        public GPSPoint(double lon, double lat, double ele, float dt) 
        {
            this.longitude = lon;
            this.latitude = lat;
            this.elevation = ele;
            this.time = dt;
        }
    }
    public class GPSData
    {
        static List<GPSPoint> gpsPoints = new List<GPSPoint>();
        
        public void AddPoint(GPSPoint p)
        {
            gpsPoints.Add(p);
        }

        public int GetPointCount()
        {
            return gpsPoints.Count;
        }

        public void LoadGPXFile(string sFile)
        {
            //
        }
        public double Speed(float time)
        {
            int index=IndexThisOrPreviousReading(time);
            double distance = DistanceBetweenPoints(gpsPoints[index], gpsPoints[index + 1]);
            float timeSpan = gpsPoints[index + 1].time - gpsPoints[index].time;
            return distance / timeSpan;
        }
        private double DistanceBetweenPoints(GPSPoint point1, GPSPoint point2)
        {
            var startLatitudeRadians = point1.latitude * (Math.PI / 180.0);
            var startLongitudeRadians = point1.longitude * (Math.PI / 180.0);
            var endLatitudeRadians = point2.latitude * (Math.PI / 180.0);
            var endLongitudeRadians = point2.longitude * (Math.PI / 180.0);

            var distanceLongitude = endLongitudeRadians - startLongitudeRadians;
            var distanceLatitude = endLatitudeRadians - startLatitudeRadians;
            var distanceElevation = Math.Abs(point1.elevation - point2.elevation);

            var result1 = Math.Pow(Math.Sin(distanceLatitude / 2.0), 2.0) +
                          Math.Cos(startLatitudeRadians) * Math.Cos(endLatitudeRadians) *
                          Math.Pow(Math.Sin(distanceLongitude / 2.0), 2.0);

            // Using 3956 as the number of miles around the earth
            var result2 = 3956.0 * 2.0 *
                          Math.Atan2(Math.Sqrt(result1), Math.Sqrt(1.0 - result1));

            return Math.Sqrt(result2 * result2 + distanceElevation * distanceElevation);
        }
        //In case some readings are made over greater time intervals (if you are in a tunel or loose signal, etc)
        private int IndexThisOrPreviousReading(float time)
        {
            GPSPoint ThisOrPreviousReading = gpsPoints.TakeWhile(gpsPoint => gpsPoint.time <= time).Last();
            return gpsPoints.IndexOf(ThisOrPreviousReading);
        }
        private double Interpolate(float time, double previousReading, double nextReading, float previousTime, float nextTime)
        {
            return previousReading + (nextReading - previousReading) * (time - previousTime) / (nextTime - previousTime);
        }
    }

    //TODO - leave only the abstract class (if we have class properties) or the interface (if we have only methods)
    public interface IGPSReader
    {
        int LoadPoints(string sFile);
    }

    public abstract class GPSLoader : IGPSReader
    {
        public abstract int LoadPoints(string sFile);
    }
    //TO DO: Implement Track Start time
    class GPXFileLoader : GPSLoader
    {
        GPSData gpsData = new GPSData();
        public override int LoadPoints(string sFile)
        {            
            XDocument gpxDoc = XDocument.Load(sFile);
            XNamespace gpxNamespace = XNamespace.Get("http://www.topografix.com/GPX/1/1");
            
            var points = from point in gpxDoc.Descendants(gpxNamespace + "trkpt")  //trk/trkseg/trkpt
                            select new
                            {
                                Latitude  = point.Attribute("lat").Value,
                                Longitude = point.Attribute("lon").Value,
                                Elevation = point.Element(gpxNamespace + "ele") != null ?
                                            point.Element(gpxNamespace + "ele").Value : null,
                                Dt        = point.Element(gpxNamespace + "time") != null ?
                                            point.Element(gpxNamespace + "time").Value : null
                            };            

            StringBuilder sb = new StringBuilder();
            DateTime startTime = new DateTime();
            bool isStart = true;
            foreach (var pt in points)
            {
                if (isStart)
                {
                    startTime = System.Convert.ToDateTime(pt.Dt);
                    isStart = false;
                }
                // This is where we'd instantiate data
                // containers for the information retrieved.
                sb.Append(
                    string.Format("Latitude:{0} Longitude:{1} Elevation:{2} Date:{3}\n",
                    pt.Latitude, pt.Longitude,
                    pt.Elevation, pt.Dt));
                //int timeSpan=
                gpsData.AddPoint(new GPSPoint(System.Convert.ToDouble(pt.Latitude),
                                                System.Convert.ToDouble(pt.Longitude),
                                                System.Convert.ToDouble(pt.Elevation),
                                                //System.Convert.ToDateTime(pt.Dt)
                                                (float)(System.Convert.ToDateTime(pt.Dt) - startTime).TotalSeconds
                                                )); //new GPSPoint(20f, 30f, 40f, DateTime.Now)

                MessageBox.Show(string.Format("Latitude:{0} Longitude:{1} Elevation:{2} Date:{3}\n", pt.Latitude, pt.Longitude, pt.Elevation, pt.Dt));
            }

            MessageBox.Show(gpsData.GetPointCount().ToString());
            return gpsData.GetPointCount();
        } 
    }
}
