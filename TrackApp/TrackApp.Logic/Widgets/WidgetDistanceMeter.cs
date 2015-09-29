using System.Drawing;
using TrackApp.Logic.Gps;

namespace TrackApp.Logic.Widgets
{
    /// <summary>
    /// Widget for displaying the distance traveled.
    /// </summary>
    public class WidgetDistanceMeter : Widget
    {
        /// <summary>
        /// Render the distancetraveled as text 
        /// </summary>
        public override void Draw(Graphics grfx, float time)
        {
            var settings = ProjectSettings.GetSettings();
            Point position = PecentToPixels(settings.DistanceWidgetPosition);

            // 200px, bottom
            double distance = GPSData.GetData().GetDistance(time) / 1000;
            string s = string.Format("{0:0.0} {1}", distance, "km");

            Font font = settings.DistanceWidgetFont;
            Brush brush = new SolidBrush(settings.DistanceWidgetColor);
            grfx.DrawString(s, font, brush, position);
        }
    }
}
