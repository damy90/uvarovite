using System;
using System.Collections.Generic;
using System.Drawing;
using AForge.Video.VFW;

public static class VideoCompositor
{
    private static string inputPath;
    private static string outputPath;
    private static string encoding = "xvid";
    //Class Widget doesn't exist yet!
    private static List<dynamic> activeWidgets;
    //променливи необходими за инструментите (widgets)
    public static int VideoWidth { get; private set; }
    public static int VideoHeigth { get; private set; }

    public static void RenderVideo()
    {
        AVIWriter writer = new AVIWriter(encoding);
        // instantiate AVI reader
        AVIReader reader = new AVIReader();
        // open video file
        reader.Open(inputPath);
        //float framerate = reader.FrameRate;
        short framerate = (short)reader.FrameRate; // за целите на нашата презентация ще го сложа short
        VideoWidth = reader.Width;
        VideoHeigth = reader.Height;
        // create new AVI file and open it
        writer.Open(outputPath, reader.Width, reader.Height);
        // read the video file
        //TO DO: Implement videoStart and videoEnd (crop video)
        while (reader.Position - reader.Start < reader.Length)
        {
            // get next frame
            Bitmap image = reader.GetNextFrame();
            Graphics grfx = Graphics.FromImage(image);
            foreach (var widget in activeWidgets)
                widget.draw(grfx, ((float)(reader.Position - reader.Start)) / framerate);
            writer.AddFrame(image);
        }
        reader.Close();
        writer.Close();
    }
}