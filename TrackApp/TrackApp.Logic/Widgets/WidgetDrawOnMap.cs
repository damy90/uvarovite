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
        protected static Size WidgetSize;
        protected static GPSData Gps = GPSData.GetData();

        /// <summary>
        /// Ofsets the track by ProjectSettings.GetSettings().TrackPostion to prevent the track from drawing outside of the frame
        /// </summary>
        /// <returns>Offset position</returns>
        protected static PointF GetPosition()
        {
            int wholeTrackLineWidth = ProjectSettings.GetSettings().WholeTrackLineWidth;
            PointF pos = PecentToPixels(ProjectSettings.GetSettings().TrackPostion);
            return new PointF(pos.X - ((float)wholeTrackLineWidth) / 2, pos.Y - ((float)wholeTrackLineWidth) / 2);
        }

        /// <summary>
        /// Method for automatic map widget sizing by given heigth.
        /// Adjusts widget aspect ratio (width) to the widget heigth and resulting width (east most and west most point in the route).
        /// </summary>
        /// <returns>The size of the map</returns>
        protected static Size GetBoundSize()
        {
            if (WidgetSize == new Size(0, 0))
            {
                GPSBox box = Gps.GetBox();
                WidgetSize.Height = ProjectSettings.GetSettings().TrackHeight * VideoCompositor.VideoDimensions.Height / 100;
                int wholeTrackLineWidth = ProjectSettings.GetSettings().WholeTrackLineWidth;
                double longtitudeCorrectionScale = GPSData.LongtitudeCorrectionScale;
                double ratio = (WidgetSize.Height - wholeTrackLineWidth) / box.Size.Latitude; // avaiable size is slighly smaller due to line width
                WidgetSize.Width = (int)Math.Ceiling(ratio * (box.Size.Longitude * longtitudeCorrectionScale) + wholeTrackLineWidth);
            }

            return WidgetSize;
        }

        /// <summary>
        /// Drawing box size for widgets that draw on the map
        /// </summary>
        /// <returns></returns>
        protected static SizeF GetSize()
        {
            int maxLineWidth = Math.Max(ProjectSettings.GetSettings().WholeTrackLineWidth, ProjectSettings.GetSettings().TraveledTrackLineWidth);
            return new SizeF(GetBoundSize().Width - maxLineWidth, GetBoundSize().Height - maxLineWidth);
        }
    }
}