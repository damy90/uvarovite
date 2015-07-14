using System;
using System.Drawing;
using TrackApp.Logic.Gps;

namespace TrackApp.Logic.Widgets
{
    public class WidgetTrack : WidgetDrawOnMap
    {
        private Bitmap trackBitmap;
        private PointF[] trackPoints;
        private int prevIndex = 0;

        /// <summary>
        /// Draws the entire track as well as the path already traveled if enabled.
        /// </summary>
        public override void Draw(Graphics grfx, float time)
        {
            // whole track
            // TODO check If there is only 1 point
            var settings = ProjectSettings.GetSettings();
            int wholeTrackLineWidth = settings.WholeTrackLineWidth;
            Pen wholeTrackPen = new Pen(settings.WholeTrackColor, wholeTrackLineWidth);

            if (this.trackBitmap == null)
            {
                GPSPoint[] trackData = Gps.GetTrack();
                this.trackPoints = new PointF[trackData.Length];

                SizeF widgetSize = GetBoundSize();
                this.trackBitmap = new Bitmap((int)Math.Ceiling(widgetSize.Width), (int)Math.Ceiling(widgetSize.Height));

                SizeF trackSize = GetSize();
                using (Graphics drawTrack = Graphics.FromImage(this.trackBitmap))
                {
                    for (int i = 0; i < trackData.Length; i++)
                    {
                        this.trackPoints[i] = Gps.ToPixelCoordinate(trackData[i], trackSize, wholeTrackLineWidth);
                    }
                        
                    drawTrack.DrawLines(wholeTrackPen, this.trackPoints);
                }
            }

            // draw track (traveled)
            // TODO: use interpolation
            if (settings.ShowTraveledTrack)
            {
                int index = Gps.GetTrackPointIndex(time);
                if (index != this.prevIndex)
                {
                    PointF[] subTrackPoints = new PointF[index - this.prevIndex + 1];
                    Array.Copy(this.trackPoints, this.prevIndex, subTrackPoints, 0, index - this.prevIndex + 1); // index - prevIndex + 1 = 2

                    int traveledTrackLineWidth = settings.TraveledTrackLineWidth;
                    Pen traveledTrackPen = new Pen(settings.TraveledTrackColor, traveledTrackLineWidth);

                    using (Graphics drawTrack = Graphics.FromImage(this.trackBitmap))
                    {
                        if (subTrackPoints.Length > 1)
                        {
                            drawTrack.DrawLines(traveledTrackPen, subTrackPoints);
                        }  
                    }

                    this.prevIndex = index;
                }
            }

            grfx.DrawImage(this.trackBitmap, Widget.PecentToPixels(ProjectSettings.GetSettings().TrackPostion));
        }
    }
}