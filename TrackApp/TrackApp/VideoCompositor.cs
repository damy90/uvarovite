using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using AForge.Video.FFMPEG;

public static class VideoCompositor
{
    private static List<Widget> activeWidgets;
    private static int previewNumber = 0;
    private static long videoEnd;
    private static long videoStart;
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

        videoEnd = (int)(settings.VideoEnd * reader.FrameRate);
        videoStart = (int)(settings.VideoStart * reader.FrameRate);
        if (videoEnd == 0 || videoEnd > reader.FrameCount)
        {
            videoEnd = reader.FrameCount;
        }

        for (long currentFrameNumber = 0; currentFrameNumber < videoEnd; currentFrameNumber++)
        {
            int speed = settings.VideoSpeed;

            Bitmap videoFrame = GetFrame(reader, speed, ref currentFrameNumber);

            using (Graphics grfx = Graphics.FromImage(videoFrame))
            {
                grfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                foreach (var widget in activeWidgets)
                    widget.Draw(grfx, currentFrameNumber / framerate);
            }

            writer.WriteVideoFrame(videoFrame);
            videoFrame.Dispose();
            //string progress = string.Format("{0} {1}", (int)(100 * n / videoEnd), '%');
        }

        reader.Close();
        writer.Close();
    }

    private static Bitmap GetFrame(VideoFileReader reader, long skipFrames, ref long currentFrame)
    {
        // skip frames
        for (int i = 0; (i < skipFrames - 1 || currentFrame < videoStart) && currentFrame < videoEnd; i++)
        {
            reader.ReadVideoFrame().Dispose();
            currentFrame++;
        }

        return reader.ReadVideoFrame();
    }

    public static string Preview(float time)
    {
        ProjectSettings settings = ProjectSettings.GetSettings();//Optimisation When multiple settings have to be read
        VideoFileReader reader = new VideoFileReader();
        using (reader)
        {
            reader.Open(settings.VideoInputPath);
            //TODO moove to Widget
            new GPXFileLoader().LoadPoints(settings.GPXPath);
            UpdateActiveWidgets(ref activeWidgets);
            float framerate = reader.FrameRate;
            long frameCount = reader.FrameCount;
            if (time * framerate > frameCount)
            {
                throw new ApplicationException("The time speciffied is outside the video timespan.");
            }
            VideoDimensions = new Size(reader.Width, reader.Height);
            videoEnd = reader.FrameCount;
            videoStart = 0;
            // unneeded in this context but needed for a method
            long n = 0;
            // get next frame

            Bitmap videoFrame = GetFrame(reader, (long)(time * framerate), ref n);

            using (Graphics grfx = Graphics.FromImage(videoFrame))
            {
                grfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                foreach (var widget in activeWidgets)
                    widget.Draw(grfx, time);
            }

            reader.Close();
            //TODO successfully dispose of previous preview
            string previewFileName = string.Format("testPreviewFrame" + previewNumber + ".png");
            previewNumber++;
            if (System.IO.File.Exists(previewFileName))
                System.IO.File.Delete(previewFileName);
            videoFrame.Save(previewFileName, ImageFormat.Png);//test - atm using the path, alternatively -> return path/BitmapImage
            videoFrame.Dispose();
            return System.IO.Directory.GetCurrentDirectory() + "\\" + previewFileName;
            //string progress = string.Format("{0} {1}", (int)(100 * n / time * framerate), '%');
        }
    }

    private static void UpdateActiveWidgets(ref List<Widget> activeWidgets)
    {
        ProjectSettings settings = ProjectSettings.GetSettings();
        activeWidgets = new List<Widget>();
        if (settings.ShowMap)
            activeWidgets.Add(new WidgetMap());
        if (settings.ShowOverlayImage)
            activeWidgets.Add(new WidgetOverlayImage());
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

