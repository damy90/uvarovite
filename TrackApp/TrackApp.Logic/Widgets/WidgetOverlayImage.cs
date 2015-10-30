using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace TrackApp.Logic.Widgets
{
    public class WidgetOverlayImage : Widget
    {
        private Bitmap image;

        /// <summary>
        /// Draw any image on top of the video. Supports transparency.
        /// </summary>
        public override void Draw(Graphics grfx, float time)
        {
            Size frameSize = VideoCompositor.VideoDimensions;
            if (this.image == null)
            {
                this.image = new Bitmap(ProjectSettings.GetSettings().OverlayImageFile);
                float hratio = Math.Abs((float)(this.image.Height - frameSize.Height) / (float)this.image.Height);
                float wratio = Math.Abs((float)(this.image.Width - frameSize.Width) / (float)this.image.Width);

                // don't resize if the difference is small
                if (hratio > 0.02 || wratio > 0.02)
                {
                    this.image = ImageEffects.ResizeImage(image, frameSize.Width, frameSize.Height);
                }
            }

            grfx.DrawImage(this.image, new Point(0, 0));
        }
    }
}
