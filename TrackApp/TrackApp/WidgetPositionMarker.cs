using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


public class WidgetPositionMarker: Widget
{
    Size widgetSize = new Size(200, 200);
    float minTrackTime = 0;
    float maxTrackTime = 0;
    float clipSeconds = 10;
    float index = 0;

    public override void Draw(Graphics grfx, float time)
    {
        // TODO: Implement this method
        GPSData gpsData = GPSData.GetData();
        GPSBox box = GPSData.GetData().GetBox();
        double ratio = widgetSize.Height / (box.Size.Lattitude);
        Point position = new Point(System.Convert.ToInt32(grfx.VisibleClipBounds.Width) - widgetSize.Width, System.Convert.ToInt32(grfx.VisibleClipBounds.Height) - widgetSize.Height);

        int n = 0;
        if (maxTrackTime == 0)
        {
            foreach (GPSPoint point in gpsData.GetTrack())
            {
                if (n == 0)
                {
                    minTrackTime = point.time;
                    maxTrackTime = point.time;
                }
                minTrackTime = Math.Min(point.time, minTrackTime);
                maxTrackTime = Math.Max(point.time, maxTrackTime);
                n++;
            }
            index = (maxTrackTime - minTrackTime) / clipSeconds;
        }

        GPSCoord coord = gpsData.GetPosition(time * index + minTrackTime);
        Point marker = new Point(0, 0);
        marker.X = position.X + (int)((coord.Longtitude - box.Position.Longtitude) * ratio * GPSData.longtitudeCorrectionScale);
        marker.Y = position.Y + widgetSize.Height - (int)((coord.Lattitude - box.Position.Lattitude) * ratio);

        Pen pen = new Pen(ProjectSettings.GetSettings().PositionMarkerColor, 2);
        Brush brush = new SolidBrush(ProjectSettings.GetSettings().PositionMarkerColor);
        grfx.FillRectangle(brush, marker.X, marker.Y, ProjectSettings.GetSettings().PositionMarkerSize, ProjectSettings.GetSettings().PositionMarkerSize);
        grfx.DrawRectangle(pen, marker.X, marker.Y, ProjectSettings.GetSettings().PositionMarkerSize, ProjectSettings.GetSettings().PositionMarkerSize);
    }
}
