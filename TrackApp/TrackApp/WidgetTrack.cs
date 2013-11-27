using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class WidgetTrack:Widget
{
    Bitmap trackBitmap;
    PointF[] trackPoints;
    int prevIndex = 0;
    public override void Draw(Graphics grfx, float time)
    {
        ProjectSettings prj = ProjectSettings.GetSettings();
        GPSData gps = GPSData.GetData();
        // draw whole track
        if (trackBitmap == null)
        {
            int wholeTrackLineWidth = prj.WholeTrackLineWidth;
            Pen wholeTrackPen = new Pen(prj.WholeTrackColor, wholeTrackLineWidth );

            GPSPoint[] trackData = gps.GetTrack();
            trackPoints = new PointF[trackData.Length];
            GPSBox box = gps.GetBox();

            Size widgetSize = new Size();
            widgetSize.Height = prj.TrackHeight;
            double ratio = (widgetSize.Height - wholeTrackLineWidth) / (box.Size.Lattitude);//avaiable size is slighly smaller due to line width
            double longtitudeCorrectionScale = GPSData.longtitudeCorrectionScale;
            widgetSize.Width = (int)Math.Ceiling( ratio * (box.Size.Longtitude * longtitudeCorrectionScale) + wholeTrackLineWidth );

            trackBitmap = new Bitmap(widgetSize.Width, widgetSize.Height);

            using (Graphics drawTrack = Graphics.FromImage(trackBitmap))
            {
                for (int i = 0; i < trackData.Length; i++)
                {
                    trackPoints[i].X = (float)((trackData[i].longitude - box.Position.Longtitude) * ratio * longtitudeCorrectionScale + ((float)wholeTrackLineWidth) / 2);
                    trackPoints[i].Y = widgetSize.Height - (float)((trackData[i].latitude - box.Position.Lattitude) * ratio + ((float)wholeTrackLineWidth) / 2 );
                }
                drawTrack.DrawLines(wholeTrackPen, trackPoints);
            }
        }
        //draw track (traveled
        int index = gps.GetIndex(time);
        if (prevIndex != null && index!=prevIndex)
        {
            PointF[] subTrackPoints = new PointF[index - prevIndex + 1];
            Array.Copy(trackPoints, prevIndex, subTrackPoints, 0, index - prevIndex + 1);//index - prevIndex + 1 = 2

            int traveledTrackLineWidth = prj.TraveledTrackLineWidth;
            Pen traveledTrackPen = new Pen(prj.TraveledTrackColor, traveledTrackLineWidth);

            using (Graphics drawTrack = Graphics.FromImage(trackBitmap))
            {
                if (subTrackPoints.Length>1)
                    drawTrack.DrawLines(traveledTrackPen, subTrackPoints);
            }
            prevIndex = index;
        }
        grfx.DrawImage(trackBitmap, prj.TrackPostion);
    }
}
