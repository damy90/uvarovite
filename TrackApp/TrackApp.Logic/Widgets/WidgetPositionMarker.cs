using System.Drawing;
using System.Windows;
using TrackApp.Logic.Gps;

namespace TrackApp.Logic.Widgets
{
    public class WidgetPositionMarker : WidgetDrawOnMap
    {
        public override void Draw(Graphics grfx, float time)
        {
            GPSData gps = GPSData.GetData();
            var settings = ProjectSettings.GetSettings();

            PointF position = gps.ToPixelCoordinate(gps.GetPosition(time), GetSize(), settings.WholeTrackLineWidth);
            position.X += GetPosition().X;
            position.Y += GetPosition().Y;
            Brush brush = new SolidBrush(settings.PositionMarkerColor);
            float size = settings.PositionMarkerSize;
            if (settings.ShowOrientation)
            {
                Vector fwd = gps.GetOrientation(time, size)[0];
                Vector side = gps.GetOrientation(time, size)[1];//perpendicular
                PointF[] markerPoints =
                {
                    new PointF((float)fwd.X + position.X, -(float)fwd.Y + position.Y),
                    new PointF((float)side.X / 3 + position.X, -(float)side.Y / 3 + position.Y),
                    new PointF(-(float)side.X / 3 + position.X, +(float)side.Y / 3 + position.Y)
                };
                grfx.FillPolygon(brush, markerPoints);
            }
            else
                grfx.FillRectangle(brush, position.X, position.Y, settings.PositionMarkerSize, settings.PositionMarkerSize);
            // TODO: Add custom immage
        }
    }
}
