using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public abstract class WidgetDrawOnMap:Widget
{
    
    protected static Size WidgetSize;
    protected static ProjectSettings settings = ProjectSettings.GetSettings();
    protected static GPSData gps = GPSData.GetData();

    protected static Point GetBoundPosition()
    {
        return settings.TrackPostion;
    }

    public static PointF GetPosition()
    {
        int wholeTrackLineWidth = settings.WholeTrackLineWidth;
        PointF pos = GetBoundPosition();
        return new PointF(pos.X - ((float)wholeTrackLineWidth) / 2, pos.Y - ((float)wholeTrackLineWidth) / 2);
    }
    protected static Size GetBoundSize()
    {
        if (WidgetSize == new Size(0, 0))
        {
            GPSBox box = gps.GetBox();
            WidgetSize.Height = settings.TrackHeight;
            int wholeTrackLineWidth = settings.WholeTrackLineWidth;
            double longtitudeCorrectionScale = GPSData.longtitudeCorrectionScale;
            double ratio = (WidgetSize.Height - wholeTrackLineWidth) / (box.Size.Latitude);//avaiable size is slighly smaller due to line width
            WidgetSize.Width = (int)Math.Ceiling(ratio * (box.Size.Longitude * longtitudeCorrectionScale) + wholeTrackLineWidth);
        }
        return WidgetSize;
    }
    protected static SizeF GetSize()
    {
        SizeF pos = GetBoundSize();
        int maxLineWidth = Math.Max(settings.WholeTrackLineWidth, settings.TraveledTrackLineWidth);
        return new SizeF(pos.Width - maxLineWidth, pos.Height - maxLineWidth);
    }
}
