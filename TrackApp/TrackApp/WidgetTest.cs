using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class WidgetTest:Widget
{
    Pen pen;
    Size widgetSize = new Size(200, 100);
    int positionX = 100;
    int positionY = 200;
    public WidgetTest()
    {
        pen = new Pen(Color.Red, 2);
    }
    public override void Draw(Graphics grfx, float time)
    {
        int lx = Math.Min((int)(time * 25), widgetSize.Width);
        if (lx < 2)
            return;
        Point[] sinePoints = new Point[lx];
        for (int p = 0; p < lx; p++)
            sinePoints[p] =
                new Point(p + widgetSize.Width,
                    widgetSize.Height + (int)(widgetSize.Height / 2 + Math.Sin(((float)p / 25)) * widgetSize.Height / 2));

        grfx.DrawLines(pen, sinePoints);
        /*{
            if (t >= sizeX)
                break;
            int y = (int)(sizeY / 2 + Math.Sin(((float)t) / 50) * sizeY / 3);
            grfx.SetPixel(t, y - 1, Color.Red);
            grfx.SetPixel(t, y, Color.Red);
            grfx.SetPixel(t, y + 1, Color.Red);
        }*/
    }
}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                