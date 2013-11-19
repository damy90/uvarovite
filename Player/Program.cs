using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;

using AForge.Video;
using AForge.Video.VFW;
using System.Net;

namespace Player
{
    static class Program
    {
        static void Main()
        {
            AVIWriter writer = new AVIWriter("xvid");
            // instantiate AVI reader
            AVIReader reader = new AVIReader();
            // open video file
            reader.Open("test2.avi");
            // create new AVI file and open it
            writer.Open("testOut2.avi", reader.Width, reader.Height);

            WebClient webClient = new WebClient();
            webClient.DownloadFile("http://maps.googleapis.com/maps/api/staticmap?center=-15.800513,-47.91378&zoom=11&size=200x200&sensor=false", @"C:\Users\GaliaTodorova\Dropbox\Telerik_wf\OOP\TeamWork\test.png");



            Bitmap flag = new Bitmap(@"C:\Users\GaliaTodorova\Dropbox\Telerik_wf\OOP\TeamWork\test.png");  //galia
            //Bitmap toFrame = new Bitmap(flag, new Size(reader.Width, reader.Height));
            //toFrame.MakeTransparent();
            // read the video file
            while (reader.Position - reader.Start < reader.Length)
            {
                // get next frame
                Bitmap image = reader.GetNextFrame();
                for (int t = 0; t < reader.Position - reader.Start; t++)
                {
                    using (Graphics grfx = Graphics.FromImage(image))
                    {
                     grfx.DrawImage(flag, 0, 0);
                    }
                    if (t >= reader.Width)
                        break;
                    int y = (int)(reader.Height / 2 + Math.Sin(((float)t) / 50) * reader.Height / 3);
                    image.SetPixel(t, y - 1, Color.Red);
                    image.SetPixel(t, y, Color.Red);
                    image.SetPixel(t, y + 1, Color.Red);
                }
                writer.AddFrame(image);
            }
            reader.Close();
            writer.Close();
        }
    }
}
