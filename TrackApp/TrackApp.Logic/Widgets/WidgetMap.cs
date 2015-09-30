using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using TrackApp.Logic.Gps;

namespace TrackApp.Logic.Widgets
{
    public class WidgetMap : WidgetDrawOnMap
    {
        private enum ImageOpreations
        {
            Downsample,
            Upsample
        }
        // TODO leave some space between the map frame and the track points, the map has to resize according to the track size
        //TODO: use google maps api for drawing, calculating, etc
        private Bitmap map;
        /// <summary>
        /// Get map from google maps and render it on a frame
        /// </summary>
        public override void Draw(Graphics grfx, float time)
        {
            if (this.map == null)
            {
                GetBoundSize();//TODO: remove this dependancy
                GPSBox box = Gps.GetBox();
                //new GPSCoord()
                var southWest = new GPSCoord(box.Position.Longitude, box.Position.Latitude, 0);
                var northEast = new GPSCoord(box.Position.Longitude + box.Size.Longitude, box.Position.Latitude + box.Size.Latitude, 0);
                //var size = new GPSCoord(box.Size.Latitude, box.Size.Longitude, 0);

                int curentZoomLevel = this.GetBoundsZoomLevel(northEast, southWest, WidgetSize);
                int curentRouteHeigthPixels = UnscaledMapImageHeigth(box.Size.Latitude, curentZoomLevel);
                double currentRatio = (double)WidgetSize.Height / curentRouteHeigthPixels;

                int nextZoomLevel = curentZoomLevel + 1;
                int nextRouteHeigthPixels = UnscaledMapImageHeigth(box.Size.Latitude, nextZoomLevel);
                double nextRatio = (double)WidgetSize.Height / nextRouteHeigthPixels;
                var nextRouteWidth = (int)Math.Floor(WidgetSize.Width / nextRatio);

                double minDeltaRatio;
                ImageOpreations imageoperation;
                int chosenHeigth;
                int chosenWidth;
                if (nextRouteHeigthPixels <= 640 && nextRouteWidth <= 640)
                {
                    double currentDelta = Math.Abs(1 - currentRatio);
                    double nextDelta = Math.Abs(1 - nextRatio);

                    if (currentDelta < nextDelta)
                    {
                        imageoperation = ImageOpreations.Upsample;
                        minDeltaRatio = currentRatio;
                        chosenHeigth = curentRouteHeigthPixels;
                        chosenWidth = (int)(WidgetSize.Width / currentRatio);
                    }
                    else
                    {
                        imageoperation = ImageOpreations.Downsample;
                        minDeltaRatio = nextRatio;
                        chosenHeigth = nextRouteHeigthPixels;
                        chosenWidth = nextRouteWidth;
                    }
                }
                else
                {
                    imageoperation = ImageOpreations.Upsample;
                    minDeltaRatio = currentRatio;
                    chosenHeigth = curentRouteHeigthPixels;
                    chosenWidth = (int)(WidgetSize.Width / currentRatio);
                }

                Point chosenSize = new Point(chosenWidth, chosenHeigth);
                var widgetSize = new Point(WidgetSize.Width, WidgetSize.Height);
                this.map = GetMap(southWest, northEast, chosenSize);
                //this.map = GetMap(southWest, northEast, widgetSize);

                this.map = ImageEffects.ResizeImage(this.map, WidgetSize.Width, WidgetSize.Height);
                this.map = ImageEffects.ChangeOpacity(this.map, ProjectSettings.GetSettings().MapOpacity);
            }

            grfx.DrawImage(this.map, PecentToPixels(ProjectSettings.GetSettings().TrackPostion));
        }

        private int UnscaledMapImageHeigth(double routeHeigthLat, int zoomLevel)
        {
            //m/px
            double[] zoomScales =
            {
                    21282,
                    16355,
                    10064,
                    5540,
                    2909,
                    1485,
                    752,
                    378,
                    190,
                    95,
                    48,
                    24,
                    12,
                    6,
                    3,
                    1.48,
                    0.74,
                    0.37,
                    0.19
            };
            var origin = new GPSPoint(0, 0, 0, 0);
            var heigth = new GPSPoint(0, routeHeigthLat, 0, 0);
            double distance = origin.DistanceFromPoint(heigth);
            double scale = zoomScales[zoomLevel - 1];
            var result = (int)Math.Round(distance / scale);

            //TODO: real fix, remove this hack.
            var quickDemoHack = new Dictionary<int, int>();
            quickDemoHack.Add(12, 10);
            quickDemoHack.Add(13, 29);
            quickDemoHack.Add(14, 75);

            return result;// - quickDemoHack[zoomLevel];
        }

        private Bitmap GetMap(GPSCoord southWest, GPSCoord northEast, Point imageSize)
        {
            //GPSBox box = Gps.GetBox();
            var webClient = new WebClient();

            string path = @"http://maps.googleapis.com/maps/api/staticmap?size="// TODO max heigth=640, max width=640
                          +
                          imageSize.X + 'x' + imageSize.Y +//+10
                          "&path=color:0x00000000|weight:5|" +
                          (southWest.Latitude - 0.00).ToString() + "," +
                          (southWest.Longitude - 0.00).ToString() + "|" +
                          (northEast.Latitude).ToString() + ',' +
                          (northEast.Longitude).ToString() +
                          "+%20&sensor=false";
            ////MessageBox.Show(path);
            // TODO use a variable instead of file
            try
            {
                this.map = new Bitmap(webClient.OpenRead(path));
            }
            catch (WebException)
            {
                throw new WebException("Could not load google map");
            }

            return this.map;
        }

        //source http://stackoverflow.com/questions/6048975/google-maps-v3-how-to-calculate-the-zoom-level-for-a-given-bounds
        int GetBoundsZoomLevel(GPSCoord northEast, GPSCoord southWest, Size mapDim)
        {
            var worldDim = new Size(256, 256);//{ height: 256, width: 256 };
            int zoomMax = 21;

            //var ne = bounds.getNorthEast();
            //var sw = bounds.getSouthWest();

            //GPSCoord ne = new GPSCoord(northEast.Y, northEast.X, 0);
            //GPSCoord sw = new GPSCoord(southWest.Y, southWest.X, 0);

            GPSCoord ne = northEast;
            GPSCoord sw = southWest;

            double latFraction = (LatRad(ne.Latitude) - LatRad(sw.Latitude)) / Math.PI;

            double lngDiff = ne.Longitude - sw.Longitude;
            double lngFraction = ((lngDiff < 0) ? (lngDiff + 360) : lngDiff) / 360;

            int latZoom = Zoom(mapDim.Height, worldDim.Height, latFraction);
            int lngZoom = Zoom(mapDim.Width, worldDim.Width, lngFraction);

            return new[] { latZoom, lngZoom, zoomMax }.Min();//Math.Min();
        }

        double LatRad(double lat)
        {
            double sin = Math.Sin(lat * Math.PI / 180);
            double radX2 = Math.Log((1 + sin) / (1 - sin)) / 2;
            return Math.Max(Math.Min(radX2, Math.PI), -Math.PI) / 2;
        }

        int Zoom(double mapPx, double worldPx, double fraction)
        {
            return (int)Math.Floor(Math.Log(mapPx / worldPx / fraction) / Math.Log(2));
        }
    }
}