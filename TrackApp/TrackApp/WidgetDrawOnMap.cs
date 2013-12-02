using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public abstract class WidgetDrawOnMap:Widget
{
    
    protected static Size WidgetSize;
    protected static ProjectSettings Settings = ProjectSettings.GetSettings();
    protected static GPSData Gps = GPSData.GetData();

    protected static Point GetBoundPosition()
    {
        return Settings.TrackPostion;
    }

    protected static PointF GetPosition()
    {
        int wholeTrackLineWidth = Settings.WholeTrackLineWidth;
        PointF pos = GetBoundPosition();
        return new PointF(pos.X - ((float)wholeTrackLineWidth) / 2, pos.Y - ((float)wholeTrackLineWidth) / 2);
    }
    protected static Size GetBoundSize()
    {
        if (WidgetSize == new Size(0, 0))
        {
            GPSBox box = Gps.GetBox();
            WidgetSize.Height = Settings.TrackHeight;
            int wholeTrackLineWidth = Settings.WholeTrackLineWidth;
            double longtitudeCorrectionScale = GPSData.longtitudeCorrectionScale;
            double ratio = (WidgetSize.Height - wholeTrackLineWidth) / (box.Size.Latitude);//avaiable size is slighly smaller due to line width
            WidgetSize.Width = (int)Math.Ceiling(ratio * (box.Size.Longitude * longtitudeCorrectionScale) + wholeTrackLineWidth);
        }
        return WidgetSize;
    }
    protected static SizeF GetSize()
    {
        SizeF pos = GetBoundSize();
        int maxLineWidth = Math.Max(Settings.WholeTrackLineWidth, Settings.TraveledTrackLineWidth);
        return new SizeF(pos.Width - maxLineWidth, pos.Height - maxLineWidth);
    }
}
