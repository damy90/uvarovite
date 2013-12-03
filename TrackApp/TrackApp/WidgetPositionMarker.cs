using System;
using System.Drawing;
using System.Linq;


public class WidgetPositionMarker : WidgetDrawOnMap
{
    public override void Draw(Graphics grfx, float time)
    {
        GPSData gps = GPSData.GetData();
        var settings = ProjectSettings.GetSettings();

        int index = gps.GetIndex(time);
        PointF trackPoint = gps.ToPixelCoordinate(gps.GetTrack()[index], GetSize(), settings.WholeTrackLineWidth);
        trackPoint.X += GetPosition().X;
        trackPoint.Y += GetPosition().Y;

        Pen pen = new Pen(settings.PositionMarkerColor, 2);
        Brush brush = new SolidBrush(settings.PositionMarkerColor);
        grfx.FillRectangle(brush, trackPoint.X, trackPoint.Y, settings.PositionMarkerSize, settings.PositionMarkerSize);
        grfx.DrawRectangle(pen, trackPoint.X, trackPoint.Y, settings.PositionMarkerSize, settings.PositionMarkerSize);

        // TODO: Add custom immage and orientation
    }
}
