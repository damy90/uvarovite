using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Globalization;

namespace LinqXMLTester
{
    public class GPXLoader
    {
        public List<GPXPoint> points = new List<GPXPoint>();
        public double LatMinValue { get; private set; }
        public double LatMaxValue { get; private set; }
        public double LonMinValue { get; private set; }
        public double LonMaxValue { get; private set; }
        /// <summary> 
        /// Load the Xml document for parsing 
        /// </summary> 
        /// <param name="sFile">Fully qualified file name (local)</param> 
        /// <returns>XDocument</returns> 
        private XDocument GetGpxDoc(string sFile)
        {
            XDocument gpxDoc = XDocument.Load(sFile);
            return gpxDoc;
        }

        /// <summary> 
        /// Load the namespace for a standard GPX document 
        /// </summary> 
        /// <returns></returns> 
        private XNamespace GetGpxNameSpace()
        {
            XNamespace gpx = XNamespace.Get("http://www.topografix.com/GPX/1/1");
            return gpx;
        }

        /// <summary> 
        /// When passed a file, open it and parse all waypoints from it. 
        /// </summary> 
        /// <param name="sFile">Fully qualified file name (local)</param> 
        /// <returns>string containing line delimited waypoints from 
        /// the file (for test)</returns> 
        /// <remarks>Normally, this would be used to populate the 
        /// appropriate object model</remarks> 
        public string LoadGPXWaypoints(string sFile)
        {
            XDocument gpxDoc = GetGpxDoc(sFile);
            XNamespace gpx = GetGpxNameSpace();

            var waypoints = from waypoint in gpxDoc.Descendants(gpx + "wpt")
                            select new
                            {
                                Latitude = waypoint.Attribute("lat").Value,
                                Longitude = waypoint.Attribute("lon").Value,
                                Elevation = waypoint.Element(gpx + "ele") != null ?
                                    waypoint.Element(gpx + "ele").Value : null,
                                Name = waypoint.Element(gpx + "name") != null ?
                                    waypoint.Element(gpx + "name").Value : null,
                                Dt = waypoint.Element(gpx + "cmt") != null ?
                                    waypoint.Element(gpx + "cmt").Value : null
                            };

            StringBuilder sb = new StringBuilder();
            foreach (var wpt in waypoints)
            {
                // This is where we'd instantiate data 
                // containers for the information retrieved. 
                sb.Append(
                  string.Format("Name:{0} Latitude:{1} Longitude:{2} Elevation:{3} Date:{4}\n",
                  wpt.Name, wpt.Latitude, wpt.Longitude,
                  wpt.Elevation, wpt.Dt));
            }

            return sb.ToString();
        }

        /// <summary> 
        /// When passed a file, open it and parse all tracks 
        /// and track segments from it. 
        /// </summary> 
        /// <param name="sFile">Fully qualified file name (local)</param> 
        /// <returns>string containing line delimited waypoints from the 
        /// file (for test)</returns> 
        public List<GPXPoint> LoadGPXTracks(string sFile)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-GB");

            XDocument gpxDoc = GetGpxDoc(sFile);
            XNamespace gpx = GetGpxNameSpace();
            var tracks = from track in gpxDoc.Descendants(gpx + "trk")
                         select new
                         {
                             Name = track.Element(gpx + "name") != null ?
                              track.Element(gpx + "name").Value : null,
                             Segs = (
                                  from trackpoint in track.Descendants(gpx + "trkpt")
                                  select new
                                  {
                                      Latitude = trackpoint.Attribute("lat").Value,
                                      Longitude = trackpoint.Attribute("lon").Value,
                                      Elevation = trackpoint.Element(gpx + "ele") != null ?
                                        trackpoint.Element(gpx + "ele").Value : null,
                                      Time = trackpoint.Element(gpx + "time") != null ?
                                        trackpoint.Element(gpx + "time").Value : null
                                  }
                                )
                         };
            LatMinValue = double.MaxValue;
            LatMaxValue = double.MinValue;
            LonMinValue = double.MaxValue;
            LonMaxValue = double.MinValue;


            StringBuilder sb = new StringBuilder();
            foreach (var trk in tracks)
            {
                // Populate track data objects. 
                foreach (var trkSeg in trk.Segs)
                {
                    // Populate detailed track segments 
                    // in the object model here. 
                    sb.Append(
                      string.Format("Track:{0} - Latitude:{1} Longitude:{2} " +
                                   "Elevation:{3} Date:{4}\n",
                      trk.Name, trkSeg.Latitude,
                      trkSeg.Longitude, trkSeg.Elevation,
                      trkSeg.Time));

                    if (trkSeg.Latitude != null && trkSeg.Longitude != null && trkSeg.Elevation != null && trkSeg.Time != null)
                    {
                        GPXPoint point = new GPXPoint();
                        point.Date = (DateTime)GetTime(trkSeg.Time); 
                        point.Latitude = double.Parse(trkSeg.Latitude);
                        point.Longitude = double.Parse(trkSeg.Longitude);
                        point.Elevation = double.Parse(trkSeg.Elevation);
                        this.points.Add(point);

                        if (point.Latitude < LatMinValue)
                        {
                            LatMinValue = point.Latitude;
                        }

                        if (point.Latitude > LatMaxValue)
                        {
                            LatMaxValue = point.Latitude;
                        }

                        if (point.Longitude < LonMinValue)
                        {
                            LonMinValue = point.Longitude;
                        }

                        if (point.Longitude > LonMaxValue)
                        {
                            LonMaxValue = point.Longitude;
                        }
                    }
                }
                
            }
            return points;
        }

        private DateTime? GetTime(string sequence)
        {
            string date = Regex.Replace(sequence, @"[a-zA-Z/<>]", " ");

            DateTime dateTime;

            if (DateTime.TryParse(date, out dateTime))
            {
                return dateTime;
            }
            else
            {
                return null;
            }
        }
    }
}