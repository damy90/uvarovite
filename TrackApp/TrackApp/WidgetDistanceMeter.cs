using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class WidgetDistanceMeter:Widget
{
    public override void Draw(Graphics grfx, float time)
    {
        var settings = ProjectSettings.GetSettings();
        Point position = PecentToPixels(settings.DistanceWidgetPosition);
        //200px, bottom

        double distance = GPSData.GetData().GetDistance(time) / 1000;
        string s = string.Format("{0:0.0} {1}", distance, "km");

        Font font = settings.DistanceWidgetFont;
        Brush brush = new SolidBrush(settings.DistanceWidgetColor);
        grfx.DrawString(s, font, brush, position);
    }
}
