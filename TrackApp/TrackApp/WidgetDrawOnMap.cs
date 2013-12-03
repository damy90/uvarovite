using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public abstract class WidgetDrawOnMap:Widget
{
    protected static Size WidgetSize;
    protected static GPSData Gps = GPSData.GetData();

    protected static Point GetBoundPosition()
    {
        /*var inputPos = ProjectSettings.GetSettings().TrackPostion;
        if (inputPos.X + GetBoundSize().Width > 100)
            inputPos.X = 100 - GetBoundSize().Width;
        if (inputPos.Y + GetBoundSize().Height > 100)
            inputPos.Y = 100 - GetBoundSize().Height;*/
        return PecentToPixels(ProjectSettings.GetSettings().TrackPostion);//TODO remoove
    }

    protected static PointF GetPosition()
    {
        int wholeTrackLineWidth = ProjectSettings.GetSettings().WholeTrackLineWidth;
        PointF pos = GetBoundPosition();
        return new PointF(pos.X - ((float)wholeTrackLineWidth) / 2, pos.Y - ((float)wholeTrackLineWidth) / 2);
    }
    protected static Size GetBoundSize()
    {
        if (WidgetSize == new Size(0, 0))
        {
            GPSBox box = Gps.GetBox();
            WidgetSize.Height = ProjectSettings.GetSettings().TrackHeight;
            int wholeTrackLineWidth = ProjectSettings.GetSettings().WholeTrackLineWidth;
            double longtitudeCorrectionScale = GPSData.longtitudeCorrectionScale;
            double ratio = (WidgetSize.Height - wholeTrackLineWidth) / (box.Size.Latitude);//avaiable size is slighly smaller due to line width
            WidgetSize.Width = (int)Math.Ceiling(ratio * (box.Size.Longitude * longtitudeCorrectionScale) + wholeTrackLineWidth);
        }
        return PecentToPixels(WidgetSize);
    }
    protected static SizeF GetSize()
    {
        int maxLineWidth = Math.Max(ProjectSettings.GetSettings().WholeTrackLineWidth, ProjectSettings.GetSettings().TraveledTrackLineWidth);
        return new SizeF(GetBoundSize().Width - maxLineWidth, GetBoundSize().Height - maxLineWidth);
    }
}

