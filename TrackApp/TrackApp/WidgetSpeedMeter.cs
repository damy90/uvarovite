using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class WidgetSpeedMeter:Widget
{
    public override void Draw(Graphics grfx, float time)
    {
        var settings = ProjectSettings.GetSettings();

        Point position = PecentToPixels(settings.SpeedWidgetPosition);

        double speed = GPSData.GetData().GetSpeed(time);
        string s = string.Format("{0:0.0} {1}", speed, "km/h");

        Font font = settings.SpeedWidgetFont;
        Brush brush = new SolidBrush(settings.SpeedWidgetColor);
        grfx.DrawString(s, font, brush, position);
    }
}
