using System;
using System.Collections.Generic;
using System.Drawing;
using AForge.Video.VFW;

namespace Player
{
    public class VideoCompositor
    {
        private string inputPath;
        private string outputPath;
        private string encoding="xvid";
        //Class Widget doesn't exist yet!
        private List<string> activeWidgets;

        public VideoCompositor(string inputPath, string outputPath, string encoding, List<string> activeWidgets)
        {
            this.inputPath = inputPath;
            this.outputPath = outputPath;
            this.encoding = encoding;
            this.activeWidgets = activeWidgets;
        }

        public void RenderVideo()
        {
            AVIWriter writer = new AVIWriter(encoding);
            // instantiate AVI reader
            AVIReader reader = new AVIReader();
            // open video file
            reader.Open(inputPath);
            float framerate = reader.FrameRate;
            // create new AVI file and open it
            writer.Open(outputPath, reader.Width, reader.Height);
            // read the video file
            //TO DO: Implement videoStart and videoEnd (crop video)
            while (reader.Position - reader.Start < reader.Length)
            {
                // get next frame
                Bitmap image = reader.GetNextFrame();
                for (int t = 0; t < reader.Position - reader.Start; t++)
                {
                    Bitmap widgets = new Bitmap(reader.Width, reader.Height);
                    foreach (var activeWidget in activeWidgets)
                    {
                        using (Graphics grfx = Graphics.FromImage(widgets))
                        {
                            //grfx.DrawImage(activeWidget.Draw(t/framerate), 0, 0);
                        }
                    }
                    using (Graphics grfx = Graphics.FromImage(image))
                    {
                        grfx.DrawImage(widgets, 0, 0);
                    }
                }
                writer.AddFrame(image);
            }
            reader.Close();
            writer.Close();
        }
    }
}