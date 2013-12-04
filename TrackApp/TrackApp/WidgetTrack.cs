using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

public class WidgetTrack : WidgetDrawOnMap
{
    Bitmap trackBitmap;
    PointF[] trackPoints;
    int prevIndex = 0;
    public override void Draw(Graphics grfx, float time)
    {
        //whole track
        //TODO check If there is only 1 point
        var settings=ProjectSettings.GetSettings();
        int wholeTrackLineWidth = settings.WholeTrackLineWidth;
        Pen wholeTrackPen = new Pen(settings.WholeTrackColor, wholeTrackLineWidth);

        if (trackBitmap == null)
        {
            GPSPoint[] trackData = Gps.GetTrack();
            trackPoints = new PointF[trackData.Length];

            SizeF widgetSize = GetBoundSize();
            trackBitmap = new Bitmap((int)Math.Ceiling(widgetSize.Width), (int)Math.Ceiling(widgetSize.Height));

            SizeF trackSize = GetSize();
            using (Graphics drawTrack = Graphics.FromImage(trackBitmap))
            {
                for (int i = 0; i < trackData.Length; i++)
                    trackPoints[i] = Gps.ToPixelCoordinate(trackData[i], trackSize, wholeTrackLineWidth);
                drawTrack.DrawLines(wholeTrackPen, trackPoints);
            }
        }
        //draw track (traveled)
        //TODO: use interpolation
        if (settings.ShowTraveledTrack)
        {
            int index = Gps.GetIndex(time);
            if (prevIndex != null && index != prevIndex)
            {
                PointF[] subTrackPoints = new PointF[index - prevIndex + 1];
                Array.Copy(trackPoints, prevIndex, subTrackPoints, 0, index - prevIndex + 1);//index - prevIndex + 1 = 2

                int traveledTrackLineWidth = settings.TraveledTrackLineWidth;
                Pen traveledTrackPen = new Pen(settings.TraveledTrackColor, traveledTrackLineWidth);

                using (Graphics drawTrack = Graphics.FromImage(trackBitmap))
                {
                    if (subTrackPoints.Length > 1)
                        drawTrack.DrawLines(traveledTrackPen, subTrackPoints);
                }
                prevIndex = index;
            }
        }
        grfx.DrawImage(trackBitmap, PecentToPixels(ProjectSettings.GetSettings().TrackPostion));
    }
}

