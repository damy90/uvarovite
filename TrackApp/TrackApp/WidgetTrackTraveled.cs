using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackApp
{
    class WidgetTrackTraveled:Widget
    {
        Bitmap trackBitmap;
        PointF[] trackPoints;
        int prevIndex = 0;

        public override void Draw(Graphics grfx, float time)
        {
            ProjectSettings prj = ProjectSettings.GetSettings();
            GPSData gps = GPSData.GetData();
            //draw track (traveled)
            //TODO: use interpolation
            int index = gps.GetIndex(time);
            if (prevIndex != null && index != prevIndex)
            {
                PointF[] subTrackPoints = new PointF[index - prevIndex + 1];
                Array.Copy(trackPoints, prevIndex, subTrackPoints, 0, index - prevIndex + 1);//index - prevIndex + 1 = 2

                int traveledTrackLineWidth = prj.TraveledTrackLineWidth;
                Pen traveledTrackPen = new Pen(prj.TraveledTrackColor, traveledTrackLineWidth);

                using (Graphics drawTrack = Graphics.FromImage(trackBitmap))
                {
                    if (subTrackPoints.Length > 1)
                        drawTrack.DrawLines(traveledTrackPen, subTrackPoints);
                }
                prevIndex = index;
            }
            grfx.DrawImage(trackBitmap, WidgetTrack.Position);
        }
    }
}
