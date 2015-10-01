using System.Drawing;
using TrackApp.Logic.Gps;

namespace TrackApp.Logic.Widgets
{
    /// <summary>
    /// Widget for displaying the speed as text.
    /// </summary>
    public class WidgetSpeedMeter : Widget
    {
        public override void Draw(Graphics grfx, float time)
        {
            var settings = ProjectSettings.GetSettings();

            Point position = PecentToPixels(settings.SpeedWidgetPosition);

            double speed = GPSData.GetData().GetSpeed(time);
            string s = string.Format("{0:0.0} {1}", speed, "km/h");

            Font font = settings.SpeedWidgetFont;
            Brush brush = new SolidBrush(settings.SpeedWidgetColor);
            grfx.DrawString(s, font, brush, position);
        }
    }
}
