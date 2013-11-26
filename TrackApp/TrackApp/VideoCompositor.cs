using System;
using System.Collections.Generic;
using System.Drawing;
//using AForge.Video.VFW;
using AForge.Video.FFMPEG;

public static class VideoCompositor
{
    private static ProjectSettings settings = ProjectSettings.GetSettings();//Optimisation When multiple settings have to be read
    private static string inputPath = settings.VideoInputPath;
    private static string outputPath = settings.VideoOutputPath;
    private static string encoding = settings.Format.ToString();//трябва да се тества
    private static List<Widget> activeWidgets;

    public static void RenderVideo()
    {
        new GPXFileLoader().LoadPoints("workout.gpx");
        
        UpdateActiveWidgets(ref activeWidgets);
        //AVIWriter writer = new AVIWriter(encoding);
        VideoFileWriter writer = new VideoFileWriter();
        // instantiate AVI reader
        //AVIReader reader = new AVIReader();
        VideoFileReader reader = new VideoFileReader();

        // open video file
        reader.Open(inputPath);
        //float framerate = reader.FrameRate;
        float framerate = reader.FrameRate;
        // create new AVI file and open it
        writer.Open(outputPath, reader.Width, reader.Height, reader.FrameRate, VideoCodec.MPEG4, 2000000 );//.MPEG4 );
        // read the video file and render output
        //videoStart and videoEnd (crop video)
        
        /*reader.Position = reader.Start + (int)(settings.VideoStart * reader.FrameRate);
        int VideoEnd = reader.Start + (int)(settings.VideoEnd * reader.FrameRate);
        if (VideoEnd == reader.Start)//settings.VideoEnd is not set (0 by default)
            VideoEnd = reader.Length;*/

        long videoEnd = (int)(settings.VideoEnd * reader.FrameRate);
        if (videoEnd == 0 || videoEnd > reader.FrameCount )
            videoEnd = reader.FrameCount;    

        //while (reader.Position - reader.Start < VideoEnd)
        for (long n = 0; n < videoEnd; n++ )
        {
            // get next frame
            //TODO Using graphics
            Bitmap videoFrame = reader.ReadVideoFrame();//reader.GetNextFrame();
            using (Graphics grfx = Graphics.FromImage(videoFrame))
            {
                grfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                foreach (var widget in activeWidgets)
                    widget.Draw(grfx, n / framerate);
            }
            //writer.AddFrame(image);
            writer.WriteVideoFrame(videoFrame);
            videoFrame.Dispose();
        }
        reader.Close();
        writer.Close();
    }

    private static void UpdateActiveWidgets(ref List<Widget> activeWidgets)
    {
        activeWidgets = new List<Widget>();
        //hardcoded tests
        //activeWidgets.Add(new WidgetTest());
        if (settings.ShowTrack)
            activeWidgets.Add(new WidgetTrack());
        if (settings.ShowPositionMarker)
            activeWidgets.Add(new WidgetPositionMarker());
        if (settings.ShowDistanceWidget)
            activeWidgets.Add(new WidgetDistanceMeter());
        if (settings.ShowSpeedWidget)
            activeWidgets.Add(new WidgetSpeedMeter());
    }
}