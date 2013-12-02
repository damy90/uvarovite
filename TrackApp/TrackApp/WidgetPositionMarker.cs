using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


public class WidgetPositionMarker : WidgetDrawOnMap
{
    public override void Draw(Graphics grfx, float time)
    {
        GPSData gps = GPSData.GetData();

        int index = gps.GetIndex(time);
        PointF trackPoint = gps.ToPixelCoordinate(gps.GetTrack()[index], GetSize(), Settings.WholeTrackLineWidth);
        trackPoint.X += GetPosition().X;
        trackPoint.Y += GetPosition().Y;

        Pen pen = new Pen(ProjectSettings.GetSettings().PositionMarkerColor, 2);
        Brush brush = new SolidBrush(ProjectSettings.GetSettings().PositionMarkerColor);
        grfx.FillRectangle(brush, trackPoint.X, trackPoint.Y, ProjectSettings.GetSettings().PositionMarkerSize, ProjectSettings.GetSettings().PositionMarkerSize);
        grfx.DrawRectangle(pen, trackPoint.X, trackPoint.Y, ProjectSettings.GetSettings().PositionMarkerSize, ProjectSettings.GetSettings().PositionMarkerSize);

        // TODO: Add custom immage and orientation
    }
}
