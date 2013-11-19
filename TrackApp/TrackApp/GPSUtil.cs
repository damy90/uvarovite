using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TrackApp
{
    public struct GPSPoint {
        public double longitude;
        public double latitude;
        public double elevation;
        public DateTime time;
        public GPSPoint(double lon, double lat, double ele, DateTime dt) 
        {
            this.longitude = lon;
            this.latitude = lat;
            this.elevation = ele;
            this.time = dt;
        }
    }

    public class GPSData
    {
        List<GPSPoint> gpsPoint = new List<GPSPoint>();
        
        public void AddPoint(GPSPoint p)
        {
            gpsPoint.Add(p);
        }

        public int GetPointCount()
        {
            return gpsPoint.Count;
        }

        public void LoadGPXFile(string sFile)
        {
            //
        }
    }


    public interface IGPSReader
    {
        int LoadPoints(string sFile);
    }

    public abstract class GPSLoader : IGPSReader
    {
        public abstract int LoadPoints(string sFile);
    }

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
            foreach (var pt in points)
            {
                // This is where we'd instantiate data
                // containers for the information retrieved.
                sb.Append(
                  string.Format("Latitude:{0} Longitude:{1} Elevation:{2} Date:{3}\n",
                  pt.Latitude, pt.Longitude,
                  pt.Elevation, pt.Dt));
                
                gpsData.AddPoint(new GPSPoint(System.Convert.ToDouble(pt.Latitude), 
                                              System.Convert.ToDouble(pt.Longitude), 
                                              System.Convert.ToDouble(pt.Elevation), 
                                              System.Convert.ToDateTime(pt.Dt)
                                              )); //new GPSPoint(20f, 30f, 40f, DateTime.Now)

                MessageBox.Show(string.Format("Latitude:{0} Longitude:{1} Elevation:{2} Date:{3}\n",pt.Latitude, pt.Longitude,pt.Elevation, pt.Dt));
            }

            MessageBox.Show(gpsData.GetPointCount().ToString());
            return gpsData.GetPointCount();
        } 
    }
}
