//TODO remoove after project presentation, moove method to Widget base class
using System.Drawing;

namespace TrackApp.Logic.Widgets
{
    public interface IDrawable
    {
        void Draw(Graphics grfx, float time);
    }
}
