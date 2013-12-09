using System.Drawing;
using System.Net;



class WidgetMap : WidgetDrawOnMap
{
    //TODO leave some space between the map frame and the track points, the map has to resize according to the track size
    private Bitmap map;
    private static int fileCount = 0;
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
            //TODO catch web client exeption System.Net.WebException
            try
            {
                webClient.DownloadFile(path, "test" + fileCount + ".png");
                fileCount++;
            }
            catch (WebException)
            {
                throw new WebException("Could not load google map");
            }
            map = new Bitmap("test.png");
        }
        grfx.DrawImage(map, PecentToPixels(ProjectSettings.GetSettings().TrackPostion));
    }
}

