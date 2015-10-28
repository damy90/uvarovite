using System;
using System.Drawing;
using TrackApp.Logic.Gps;

namespace TrackApp.Logic.Widgets
{
    /// <summary>
    /// Base map widget for drawing maps and routs
    /// </summary>
    public abstract class WidgetDrawOnMap : Widget
    {
        //TODO: reduce static fields
        public WidgetDrawOnMap()
        {
            WidgetSize = new Size(0, 0);
            Gps = GPSData.GetData();
        }

        protected static Size WidgetSize;
        protected static GPSData Gps;

        /// <summary>
        /// Offsets the track by subtracting the line width of the track from ProjectSettings.GetSettings().TrackPostion to prevent the track from drawing outside of the frame
        /// </summary>
        /// <returns>Offset position</returns>
        protected static PointF GetPosition()
        {
            int wholeTrackLineWidth = ProjectSettings.GetSettings().WholeTrackLineWidth;
            PointF pos = PecentToPixels(ProjectSettings.GetSettings().TrackPostion);
            return new PointF(pos.X - ((float)wholeTrackLineWidth) / 2, pos.Y - ((float)wholeTrackLineWidth) / 2);
        }

        /// <summary>
        /// Method for automatic map widget sizing by given height.
        /// Adjusts widget aspect ratio (width) to the widget height and resulting width (east most and west most point in the route).
        /// The method may change the widget dimencions in the settings to fit the largest supported size by the google maps api IF the map widget is active.
        /// </summary>
        /// <returns>The size of the map</returns>
        protected static Size GetBoundSize()
        {
            if (WidgetSize == new Size(0, 0))
            {
                GPSBox box = Gps.GetBox();
                WidgetSize.Height = ProjectSettings.GetSettings().TrackHeight * VideoCompositor.VideoDimensions.Height / 100;
                int wholeTrackLineWidth = ProjectSettings.GetSettings().WholeTrackLineWidth;
                double longitudeCorrectionScale = GPSData.longitudeCorrectionScale;
                double ratio = (WidgetSize.Height - wholeTrackLineWidth) / box.Size.Latitude; // avaiable size is slighly smaller due to line width
                WidgetSize.Width = (int)Math.Ceiling(ratio * (box.Size.Longitude * longitudeCorrectionScale) + wholeTrackLineWidth);
            }

            int sizeMax = Math.Max(WidgetSize.Height, WidgetSize.Width);
            if(ProjectSettings.GetSettings().ShowMap && sizeMax > 640)
            {
                //TODO: The map is too big. Maximum size supported is 640x640px. Would you like to shrink it automaticaly and continue?
                ShrinkMapToMaxSize();
                WidgetSize = new Size(0, 0);
                GetBoundSize();
            }

            return WidgetSize;
        }

        private static void ShrinkMapToMaxSize()
        {
            int sizeMax = Math.Max(WidgetSize.Height, WidgetSize.Width);
            float shrinkRatio = (float)sizeMax / 640;
            ProjectSettings.GetSettings().TrackHeight = (int)Math.Floor(ProjectSettings.GetSettings().TrackHeight/shrinkRatio);
        }

        /// <summary>
        /// Drawing box size for widgets that draw on the map taking into account the line width of the traveled track
        /// </summary>
        /// <returns></returns>
        protected static SizeF GetSize()
        {
            int maxLineWidth = Math.Max(ProjectSettings.GetSettings().WholeTrackLineWidth, ProjectSettings.GetSettings().TraveledTrackLineWidth);
            return new SizeF(GetBoundSize().Width - maxLineWidth, GetBoundSize().Height - maxLineWidth);
        }
    }
}