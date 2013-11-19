using System;
using System.Collections.Generic;
using System.Drawing;
using AForge.Video.VFW;

public static class VideoCompositor
{
    private static string inputPath="test2.avi";
    private static string outputPath = "testWidgetTest.avi";
    private static string encoding = "xvid";
    private static List<Widget> activeWidgets=new List<Widget>();
    //променливи необходими за инструментите (widgets)
    public static int VideoWidth { get; private set; }
    public static int VideoHeigth { get; private set; }

    public static void RenderVideo()
    {
        //hardcoded tests
        activeWidgets.Add(new WidgetTest());
        AVIWriter writer = new AVIWriter(encoding);
        // instantiate AVI reader
        AVIReader reader = new AVIReader();
        // open video file
        reader.Open(inputPath);
        //float framerate = reader.FrameRate;
        float framerate = reader.FrameRate;
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
                widget.Draw(grfx, ((float)(reader.Position - reader.Start)) / framerate);
            writer.AddFrame(image);
        }
        reader.Close();
        writer.Close();
    }
}