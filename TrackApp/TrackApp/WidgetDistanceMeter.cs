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
        Point position = new Point(200, System.Convert.ToInt32(grfx.VisibleClipBounds.Height) - 50);

        double distance = GPSData.GetData().GetDistance(time) / 1000;
        string s = string.Format("{0:0.0} {1}", distance, "km");

        Font font = ProjectSettings.GetSettings().DistanceWidgetFont;
        Brush brush = new SolidBrush(ProjectSettings.GetSettings().DistanceWidgetColor);
        grfx.DrawString(s, font, brush, position);
    }
}
