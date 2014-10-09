using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using AForge.Video.FFMPEG;
using TrackApp.Logic.Gps;
using TrackApp.Logic.Widgets;

namespace TrackApp.Logic
{
    public static class VideoCompositor
    {
        private static readonly VideoFileReader reader = new VideoFileReader();
        private static List<Widget> activeWidgets;
        private static int previewNumber = 0;
        private static long videoEnd;
        private static long videoStart;

        public static Size VideoDimensions { get; private set; }

        // TODO add sound
        public static void RenderVideo()
        {
            ProjectSettings settings = ProjectSettings.GetSettings();

            // TODO moove to Widget
            if (string.IsNullOrEmpty(settings.GPXPath))
            {
                throw new ArgumentNullException("No track file was selected!");
            }

            new GPXFileLoader().LoadPoints(settings.GPXPath);

            UpdateActiveWidgets(ref activeWidgets);

            VideoFileWriter writer = new VideoFileWriter();

            // open video file
            reader.Open(settings.VideoInputPath);
            VideoDimensions = new Size(reader.Width, reader.Height);
            float framerate = reader.FrameRate;

            // create new AVI file and open it
            var encoding = (VideoCodec)Enum.Parse(typeof(VideoCodec), settings.Format.ToString());

            if (string.IsNullOrEmpty(settings.VideoOutputPath))
            {
                throw new ArgumentNullException("No output video file was specified!");
            }

            writer.Open(settings.VideoOutputPath, reader.Width, reader.Height, reader.FrameRate, encoding, settings.VideoQuality * 1000000);

            videoEnd = (int)(settings.VideoEnd * reader.FrameRate);
            videoStart = (int)(settings.VideoStart * reader.FrameRate);
            if (videoEnd == 0 || videoEnd > reader.FrameCount)
            {
                videoEnd = reader.FrameCount;
            }

            int speed = settings.VideoSpeed;
            for (long currentFrameNumber = 0; currentFrameNumber < videoEnd - speed; currentFrameNumber++)
            {
                Bitmap videoFrame = GetFrame(reader, speed, ref currentFrameNumber);

                RenderFrame(currentFrameNumber / framerate, videoFrame);

                writer.WriteVideoFrame(videoFrame);
                videoFrame.Dispose();
                ////string progress = string.Format("{0} {1}", (int)(100 * n / videoEnd), '%');
            }

            reader.Close();
            writer.Close();
        }

        public static string Preview(float time)
        {
            ProjectSettings settings = ProjectSettings.GetSettings();

            // TODO moove to Widget
            if (string.IsNullOrEmpty(settings.GPXPath))
            {
                throw new EmptyTrackException("No track file was selected");
            }

            new GPXFileLoader().LoadPoints(settings.GPXPath);

            UpdateActiveWidgets(ref activeWidgets);

                reader.Open(settings.VideoInputPath);

                float framerate = reader.FrameRate;
                long frameNumnber = (long)(time * framerate);

                videoEnd = reader.FrameCount;
                if (frameNumnber > videoEnd)
                {
                    throw new ApplicationException("The time speciffied is outside the video timespan.");
                }

                VideoDimensions = new Size(reader.Width, reader.Height);

                videoStart = 0;

                // unneeded in this context but needed for a method
                long n = 0;

                Bitmap videoFrame = GetFrame(reader, frameNumnber, ref n);

                RenderFrame(time, videoFrame);

                reader.Close();

                // TODO successfully dispose of previous preview or use a variable
                string previewFileName = string.Format("testPreviewFrame" + previewNumber + ".png");
                previewNumber++;
                if (System.IO.File.Exists(previewFileName))
                {
                    System.IO.File.Delete(previewFileName);
                }
                    
                videoFrame.Save(previewFileName, ImageFormat.Png); // test - atm using the path, alternatively -> return path/BitmapImage
                videoFrame.Dispose();

                return System.IO.Directory.GetCurrentDirectory() + "\\" + previewFileName;
        }

        private static void RenderFrame(float timeInSeconds, Bitmap videoFrame)
        {
            using (Graphics grfx = Graphics.FromImage(videoFrame))
            {
                grfx.SmoothingMode = SmoothingMode.AntiAlias;
                foreach (var widget in activeWidgets)
                {
                    widget.Draw(grfx, timeInSeconds);
                }
            }
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

        private static void UpdateActiveWidgets(ref List<Widget> activeWidgets)
        {
            ProjectSettings settings = ProjectSettings.GetSettings();
            activeWidgets = new List<Widget>();
            if (settings.ShowMap)
            {
                activeWidgets.Add(new WidgetMap());
            }

            if (settings.ShowOverlayImage)
            {
                activeWidgets.Add(new WidgetOverlayImage());
            }

            if (settings.ShowTrack)
            {
                activeWidgets.Add(new WidgetTrack());
            }

            if (settings.ShowPositionMarker)
            {
                activeWidgets.Add(new WidgetPositionMarker());
            }

            if (settings.ShowDistanceWidget)
            {
                activeWidgets.Add(new WidgetDistanceMeter());
            }

            if (settings.ShowSpeedWidget)
            {
                activeWidgets.Add(new WidgetSpeedMeter());
            }
        }
    }
}