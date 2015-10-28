using System.Drawing;
using System.Windows;
using TrackApp.Logic.Gps;

namespace TrackApp.Logic.Widgets
{
    /// <summary>
    /// Uses a triangle to indicate position and direction on the map
    /// </summary>
    public class WidgetPositionMarker : WidgetDrawOnMap
    {
        /// <summary>
        /// Draw position marker
        /// </summary>
        /// <param name="grfx">The frame</param>
        /// <param name="time"></param>
        public override void Draw(Graphics grfx, float time)
        {
            // TODO: Add custom immage
            GPSData gps = GPSData.GetData();
            ProjectSettings settings = ProjectSettings.GetSettings();

            PointF position = gps.ToPixelCoordinate(gps.GetPosition(time), GetSize(), settings.WholeTrackLineWidth);
            position.X += GetPosition().X;
            position.Y = GetPosition().Y + position.Y;
            
            float size = settings.PositionMarkerSize;
            Vector[] coordinateSystem = gps.GetOrientation(time, size);

            Vector fwd = coordinateSystem[0];
            Vector side = coordinateSystem[1]; // perpendicular
            PointF[] markerPoints =
            {
                new PointF((float)fwd.X + position.X, -(float)fwd.Y + position.Y),
                new PointF((float)side.X / 3 + position.X, -(float)side.Y / 3 + position.Y),
                new PointF(-(float)side.X / 3 + position.X, +(float)side.Y / 3 + position.Y)
            };

            Brush brush = new SolidBrush(settings.PositionMarkerColor);
            grfx.FillPolygon(brush, markerPoints);
        }
    }
}
