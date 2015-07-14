using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace TrackApp.Logic.Widgets
{
    public class WidgetOverlayImage : Widget
    {
        private Bitmap image;

        /// <summary>
        /// Draw any image on top of the video 
        /// </summary>
        public override void Draw(Graphics grfx, float time)
        {
            var frameSize = VideoCompositor.VideoDimensions;
            if (this.image == null)
            {
                this.image = new Bitmap(ProjectSettings.GetSettings().OverlayImageFile);
                float hratio = Math.Abs((float)(this.image.Height - frameSize.Height) / (float)this.image.Height);
                float wratio = Math.Abs((float)(this.image.Width - frameSize.Width) / (float)this.image.Width);
                if (hratio > 0.02 || wratio > 0.02)
                {
                    ////image.SetResolution(frameSize.Width, frameSize.Height);
                    Bitmap result = new Bitmap(frameSize.Width, frameSize.Height);
                    using (Graphics g = Graphics.FromImage(result))
                    {
                        g.InterpolationMode = InterpolationMode.NearestNeighbor;
                        g.DrawImage(this.image, 0, 0, frameSize.Width, frameSize.Height);
                    }

                    this.image = result;
                }
            }

            grfx.DrawImage(this.image, new Point(0, 0));
        }
    }
}
