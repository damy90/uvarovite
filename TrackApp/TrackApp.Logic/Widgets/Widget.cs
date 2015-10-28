using System.Drawing;

namespace TrackApp.Logic.Widgets
{
    /// <summary>
    /// Base class for all widgets
    /// </summary>
    public abstract class Widget
    {
        //public Widget(Point postion)
        //{
        //    this.position = position;
        //}

        //private Point position { get; set; }
        /// <summary>
        /// Render a widget onto a frame
        /// </summary>
        /// <param name="grfx">The frame</param>
        /// <param name="time">time in seconds</param>
        public abstract void Draw(Graphics grfx, float time);

        /// <summary>
        /// Converts percents from video frame dimensions to pixels for position
        /// </summary>
        /// <param name="position">Poind object with x and y in percent</param>
        /// <returns>A point in 2d coordinate system</returns>
        protected static Point PecentToPixels(Point position)
        {
            //TODO: handle x and y >100 and <1
            //TODO: use constructor, remove side effect causing magick such as using VideoCompositor.VideoDimensions
            //TODO: base PecentToPixels method (DRY)
            position.X = (position.X * VideoCompositor.VideoDimensions.Width) / 100;
            position.Y = (position.Y * VideoCompositor.VideoDimensions.Height) / 100;
            return position;
        }

        //TODO: remove
        protected static Size PecentToPixels(Size size)
        {
            size.Width = (size.Width * VideoCompositor.VideoDimensions.Width) / 100;
            size.Height = (size.Height * VideoCompositor.VideoDimensions.Height) / 100;
            return size;
        }
    }
}
