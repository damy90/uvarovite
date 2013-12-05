using System.Drawing;
using System.Windows;

public class WidgetPositionMarker : WidgetDrawOnMap
{
    public override void Draw(Graphics grfx, float time)
    {
        GPSData gps = GPSData.GetData();
        var settings = ProjectSettings.GetSettings();

        //int index = gps.GetIndex(time);
        //PointF trackPoint = gps.ToPixelCoordinate(gps.GetTrack()[index], GetSize(), settings.WholeTrackLineWidth);
        PointF position = gps.ToPixelCoordinate(gps.GetPosition(time), GetSize(), settings.WholeTrackLineWidth);
        position.X += GetPosition().X;
        position.Y += GetPosition().Y;
       // Pen pen = new Pen(settings.PositionMarkerColor, 2);
        Brush brush = new SolidBrush(settings.PositionMarkerColor);
       // grfx.FillRectangle(brush, position.X, position.Y, settings.PositionMarkerSize, settings.PositionMarkerSize);
       // grfx.DrawRectangle(pen, position.X, position.Y, settings.PositionMarkerSize, settings.PositionMarkerSize);

        float size = settings.PositionMarkerSize;
        Vector fwd = gps.GetOrientation(time, size)[0];
        Vector side = gps.GetOrientation(time, size)[1];//perpendicular
        PointF[] markerPoints = { new PointF((float)fwd.X + position.X, -(float)fwd.Y + position.Y),
                                  new PointF((float)side.X / 3 + position.X, -(float)side.Y / 3 + position.Y),
                                  new PointF(-(float)side.X / 3 + position.X, +(float)side.Y / 3 + position.Y) };
        /*foreach(PointF point in markerPoints)
        {
            point.X += position.X;
            point.Y += position.Y;
        }*/
        grfx.FillPolygon(brush, markerPoints);
        // TODO: Add custom immage and orientation
    }
}
