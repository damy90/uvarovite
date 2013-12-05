using System.Drawing;

public abstract class Widget
{
    public abstract void Draw(Graphics grfx, float time);

    protected static Point PecentToPixels(Point position)
    {
        position.X = (position.X * VideoCompositor.VideoDimensions.Width) / 100;
        position.Y = (position.Y * VideoCompositor.VideoDimensions.Height) / 100;
        return position;
    }
    protected static Size PecentToPixels(Size size)
    {
        size.Width = (size.Width * VideoCompositor.VideoDimensions.Width) / 100;
        size.Height = (size.Height * VideoCompositor.VideoDimensions.Height) / 100;
        return size;
    }
}
