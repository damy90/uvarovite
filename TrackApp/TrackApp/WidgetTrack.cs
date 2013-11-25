using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class WidgetTrack:Widget
{
    //private Point[] track;
    //Bitmap track;
    Size widgetSize = new Size();
    Point position = new Point(100, 100);
    Pen pen = new Pen(Color.Red, 4);
    Point[] trackPoints;
    GPSPoint[] trackData;
    GPSBox box;
    double ratio;
    public override void Draw(Graphics grfx, float time)
    {
        widgetSize.Height = 100;
        if (trackPoints == null)
        {
            trackData = GPSData.GetData().GetTrack();
            trackPoints=new Point[trackData.Length];
            box = GPSData.GetData().GetBox();
            ratio = widgetSize.Height / box.Size.Longtitude;
            //track = new Bitmap(widgetSize.Width, widgetSize.Height);
            
        }
        for (int i = 0; i < trackData.Length; i++)
        {
            trackPoints[i].X = position.X + (int)((trackData[i].latitude - box.Position.Lattitude) * ratio);
            trackPoints[i].Y = position.Y + widgetSize.Height - (int)((trackData[i].longitude - box.Position.Longtitude) * ratio);
        }
        grfx.DrawLines(pen, trackPoints);
        //grfx.DrawImage(track, position);
    }
}
