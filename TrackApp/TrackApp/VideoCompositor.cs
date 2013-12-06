using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using AForge.Video.FFMPEG;

public static class VideoCompositor
{
    private static List<Widget> activeWidgets;
    public static Size VideoDimensions { get; private set; }
    //TODO add sound
    public static void RenderVideo()
    {
        ProjectSettings settings = ProjectSettings.GetSettings();//Optimisation When multiple settings have to be read
        //string encoding = settings.Format.ToString();//трябва да се тества
        //TODO moove to Widget, if null check
        new GPXFileLoader().LoadPoints(settings.GPXPath);

        UpdateActiveWidgets(ref activeWidgets);
        VideoFileWriter writer = new VideoFileWriter();
        // instantiate AVI reader
        VideoFileReader reader = new VideoFileReader();

        // open video file
        reader.Open(settings.VideoInputPath);
        VideoDimensions = new Size(reader.Width, reader.Height);
        float framerate = reader.FrameRate;
        // create new AVI file and open it
        var encoding = VideoCodec.MPEG4;
        switch (settings.Format.ToString())
        {
            
            case "WMV2":
                encoding = VideoCodec.WMV2;
                break;
            case "MPEG2":
                encoding = VideoCodec.MPEG2;
                break;
            case "Raw":
                encoding = VideoCodec.Raw;
                break;
            default:
                encoding = VideoCodec.MPEG4;
                break;
        }

        writer.Open(settings.VideoOutputPath, reader.Width, reader.Height, reader.FrameRate, encoding, settings.VideoQuality * 1000000);

        long videoEnd = (int)(settings.VideoEnd * reader.FrameRate);
        long videoStart = (int)(settings.VideoStart * reader.FrameRate);
        if (videoEnd == 0 || videoEnd > reader.FrameCount)
            videoEnd = reader.FrameCount;

        for (long n = 0; n < videoEnd; n++)
        {
            int speed = settings.VideoSpeed;
            // get next frame
            Bitmap videoFrame = reader.ReadVideoFrame();
            if (n % speed == 0 && n > videoStart)
            {
                using (Graphics grfx = Graphics.FromImage(videoFrame))
                {
                    grfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    foreach (var widget in activeWidgets)
                        widget.Draw(grfx, n / framerate);
                }
                writer.WriteVideoFrame(videoFrame);
                videoFrame.Dispose();
            }
            videoFrame.Dispose();
            string progress = string.Format("{0} {1}", (int)(100 * n / videoEnd), '%');
        }
        reader.Close();
        writer.Close();
    }

    public static Bitmap Preview(float time)
    {
        ProjectSettings settings = ProjectSettings.GetSettings();//Optimisation When multiple settings have to be read
        VideoFileReader reader = new VideoFileReader();
        reader.Open(settings.VideoInputPath);
        //TODO moove to Widget
        new GPXFileLoader().LoadPoints(settings.GPXPath);
        UpdateActiveWidgets(ref activeWidgets);
        float framerate = reader.FrameRate;
        VideoDimensions = new Size(reader.Width, reader.Height);
        for (long n = 0; n < reader.FrameCount; n++)
        {
            // get next frame
            Bitmap videoFrame = reader.ReadVideoFrame();
            if (n == time * framerate)
            {
                using (Graphics grfx = Graphics.FromImage(videoFrame))
                {
                    grfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    foreach (var widget in activeWidgets)
                        widget.Draw(grfx, time);
                }
                reader.Close();
                videoFrame.Save("testPreviewFrame.png",ImageFormat.Png);//test - atm using the path, alternatively -> return path/BitmapImage
                return videoFrame;
            }
            videoFrame.Dispose();
            string progress = string.Format("{0} {1}", (int)(100 * n / time * framerate), '%');
        }
        throw new ApplicationException("The time speciffied is outside the video timespan.");
    }

    private static void UpdateActiveWidgets(ref List<Widget> activeWidgets)
    {
        ProjectSettings settings = ProjectSettings.GetSettings();
        activeWidgets = new List<Widget>();
        if (settings.ShowMap)
            activeWidgets.Add(new WidgetMap());
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

