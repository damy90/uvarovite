using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class WidgetSpeedMeter:Widget
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
        Point position = new Point(10, System.Convert.ToInt32(grfx.VisibleClipBounds.Height) - 50);

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

        double speed = GPSData.GetData().GetAverageSpeed(time * index + minTrackTime);
        string s = System.Convert.ToInt32(speed).ToString() + " km/h";

        Font font = ProjectSettings.GetSettings().SpeedWidgetFont;
        Brush brush = new SolidBrush(ProjectSettings.GetSettings().SpeedWidgetColor);
        grfx.DrawString(s, font, brush, position);
    }
}
