using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class WidgetOverlayImage : Widget
{
    private Bitmap image;
    public override void Draw(Graphics grfx, float time)
    {
        var frameSize = VideoCompositor.VideoDimensions;
        if (image == null)
        {
            image = new Bitmap(ProjectSettings.GetSettings().overlayImageFile);
            float hratio = Math.Abs((float) (image.Height - frameSize.Height)/(float) image.Height);
            float wratio = Math.Abs((float) (image.Width - frameSize.Width)/(float) image.Width);
            if (hratio > 0.02 || wratio > 0.02)
            {
                //image.SetResolution(frameSize.Width, frameSize.Height);
                Bitmap result = new Bitmap(frameSize.Width, frameSize.Height);
                using (Graphics g = Graphics.FromImage(result))
                {
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                    g.DrawImage(image, 0, 0, frameSize.Width, frameSize.Height);
                }
                image = result;
            }
        }
        grfx.DrawImage(image, new Point(0,0));//PecentToPixels(ProjectSettings.GetSettings().TrackPostion));
    }
}
