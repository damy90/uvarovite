using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using AForge.Video.FFMPEG;
using TrackApp.Logic.Gps;
using TrackApp.Logic.Widgets;

namespace TrackApp.Logic
{
    /// <summary>
    /// Class for initiating the rendering process
    /// </summary>
    public static class VideoCompositor
    {
        private static readonly VideoFileReader reader = new VideoFileReader();
        //private static List<Widget> activeWidgets;
        private static int previewNumber = 0;
        private static long videoEnd;
        private static long videoStart;

        public static Size VideoDimensions { get; private set; }

        /// <summary>
        /// Method for rendering  and saving the video to a new file. All settings are set in the ProjectSettings class.
        /// </summary>
        // TODO add sound, use constructor with parameters in a factory patern
        public static void RenderVideo()
        {
            ProjectSettings settings = ProjectSettings.GetSettings();

            // TODO moove to Widget
            if (string.IsNullOrEmpty(settings.GPXPath))
            {
                throw new ArgumentNullException("No track file was selected!");
            }

            new GPXFileLoader().LoadPoints(settings.GPXPath);

            List<Widget> activeWidgets = UpdateActiveWidgets();

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

                RenderFrame(currentFrameNumber / framerate - settings.VideoStart, videoFrame, activeWidgets);

                writer.WriteVideoFrame(videoFrame);
                videoFrame.Dispose();
                ////string progress = string.Format("{0} {1}", (int)(100 * n / videoEnd), '%');
            }

            reader.Close();
            writer.Close();
        }

        /// <summary>
        /// Method for rendering a preview frame at a specified time in the input video. A quick way of checking widget settings and positions before rendering a video. All settings are set in the ProjectSettings class.
        /// </summary>
        /// <param name="time">Time in seconds.</param>
        /// <returns>A bitmap image (an example frame).</returns>
        public static Bitmap Preview(float time)
        {
            ProjectSettings settings = ProjectSettings.GetSettings();

            // TODO moove to Widget
            if (string.IsNullOrEmpty(settings.GPXPath))
            {
                throw new EmptyTrackException("No track file was selected");
            }

            new GPXFileLoader().LoadPoints(settings.GPXPath);

            List<Widget> activeWidgets = UpdateActiveWidgets();

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

            RenderFrame(time, videoFrame, activeWidgets);

            reader.Close();

            return videoFrame;
        }

        /// <summary>
        /// Method for rendering all widgets to a frame given as a bitmap image.
        /// </summary>
        /// <param name="timeInSeconds">Time in seconds</param>
        /// <param name="videoFrame">A bitmap image (video frame) with rendered widgets</param>
        private static void RenderFrame(float timeInSeconds, Bitmap videoFrame, List<Widget> activeWidgets)
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

        /// <summary>
        /// Method for reading the next frame from the video input stream. 
        /// Can skip frames to speed up the resulting video
        /// </summary>
        /// <param name="reader">VideoFileReader object (the video input stream)</param>
        /// <param name="skipFrames">Frames to skip</param>
        /// <param name="currentFrame">Refference to the number of already processed frames from the input video.</param>
        /// <returns>A frame from the input video stream</returns>
        private static Bitmap GetFrame(VideoFileReader reader, long skipFrames, ref long currentFrame)
        {
            for (int i = 0; (i < skipFrames - 1 || currentFrame < videoStart) && currentFrame < videoEnd; i++)
            {
                reader.ReadVideoFrame().Dispose();
                currentFrame++;
            }

            return reader.ReadVideoFrame();
        }

        /// <summary>
        ///  A method for poppulating a list with all enabled widgets from ProjectSettings.
        /// </summary>
        /// <param name="activeWidgets">An empty list of type Widget</param>
        /// <returns>List of enabled widgets</returns>
        private static List<Widget> UpdateActiveWidgets()
        {
            List<Widget> activeWidgets = new List<Widget>();
            ProjectSettings settings = ProjectSettings.GetSettings();
            activeWidgets = new List<Widget>();

            //TODO: pass project settings by constructor
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

            return activeWidgets;
        }
    }
}