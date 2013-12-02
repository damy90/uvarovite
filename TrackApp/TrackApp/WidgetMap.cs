using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;


class WidgetMap : WidgetDrawOnMap
{
    //TODO leave some space between the map frame and the track points, the map has to resize according to the track size
    Bitmap map;
    public override void Draw(Graphics grfx, float time)
    {
        if (map == null)
        {
            GPSBox box = Gps.GetBox();
            WebClient webClient = new WebClient();
            string path = @"http://maps.googleapis.com/maps/api/staticmap?size="//TODO max heigth=640, max width=640
                            + GetBoundSize().Width + 'x' + GetBoundSize().Height
                            + "&path=color:0x00000000|weight:5|"
                            + (box.Position.Latitude - 0.00).ToString() + ","
                            + (box.Position.Longitude - 0.00).ToString() + "|"
                            + (box.Position.Latitude + 0.00 + box.Size.Latitude).ToString() + ','
                            + (box.Position.Longitude + 0.00 + box.Size.Longitude).ToString()
                            + "+%20&sensor=false";
            //MessageBox.Show(path);
            webClient.DownloadFile(path, "test.png");
            map = new Bitmap("test.png");
        }
        grfx.DrawImage(map, GetBoundPosition());
    }
}

