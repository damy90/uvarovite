using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class WidgetTrack:Widget
{
    //private Point[] track;
    Bitmap trackBitmap;
    PointF[] trackPoints;
    double ratio;
    double longtitudeCorrectionScale = GPSData.longtitudeCorrectionScale;
    public override void Draw(Graphics grfx, float time)
    {
        if (trackBitmap == null)
        {
            int wholeTrackLineWidth = ProjectSettings.GetSettings().WholeTrackLineWidth;
            int traveledTrackLineWidth = ProjectSettings.GetSettings().TraveledTrackLineWidth;
            Pen wholeTrackPen = new Pen(ProjectSettings.GetSettings().WholeTrackColor, wholeTrackLineWidth );
            Pen traveledTrackPen = new Pen(ProjectSettings.GetSettings().TraveledTrackColor, traveledTrackLineWidth);

            GPSPoint[] trackData = GPSData.GetData().GetTrack();
            trackPoints=new PointF[trackData.Length];
            GPSBox box = GPSData.GetData().GetBox();

            Size widgetSize = new Size();
            widgetSize.Height = ProjectSettings.GetSettings().TrackHeight;
            ratio = (widgetSize.Height - wholeTrackLineWidth) / (box.Size.Lattitude);//avaiable size is slighly smaller due to line width
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
        //for (int i = 0; i < trackData.Length; i++)
        //{
        //    trackPoints[i].X = position.X + (float)((trackData[i].longitude - box.Position.Longtitude) * ratio * longtitudeCorrectionScale);
        //    trackPoints[i].Y = position.Y + widgetSize.Height - (float)((trackData[i].latitude - box.Position.Lattitude) * ratio);
        //}
        //grfx.DrawLines(pen, trackPoints);
        grfx.DrawImage(trackBitmap, ProjectSettings.GetSettings().TrackPostion);
    }
}
