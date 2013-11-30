using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


public class WidgetPositionMarker: WidgetDrawOnMap
{   
    public override void Draw(Graphics grfx, float time)
    {
        GPSData gps = GPSData.GetData();

        int index = gps.GetIndex(time);
        PointF trackPoint = gps.ToPixelCoordinate(gps.GetTrack()[index], GetSize());
        trackPoint.X += GetPosition().X;
        trackPoint.Y += GetPosition().Y;

        Pen pen = new Pen(ProjectSettings.GetSettings().PositionMarkerColor, 2);
        Brush brush = new SolidBrush(ProjectSettings.GetSettings().PositionMarkerColor);
        grfx.FillRectangle(brush, trackPoint.X, trackPoint.Y, ProjectSettings.GetSettings().PositionMarkerSize, ProjectSettings.GetSettings().PositionMarkerSize);
        grfx.DrawRectangle(pen, trackPoint.X, trackPoint.Y, ProjectSettings.GetSettings().PositionMarkerSize, ProjectSettings.GetSettings().PositionMarkerSize);
        /*
        // TODO: Add custom immage and orientation
        GPSData gpsData = GPSData.GetData();
        GPSBox box = GPSData.GetData().GetBox();
        double ratio = widgetSize.Height / (box.Size.Latitude);
        PointF position = WidgetTrack.GetPosition();//new Point(System.Convert.ToInt32(grfx.VisibleClipBounds.Width) - widgetSize.Width, System.Convert.ToInt32(grfx.VisibleClipBounds.Height) - widgetSize.Height);

        GPSCoord coord = gpsData.GetPosition(time);
        Point marker = new Point(0, 0);
        marker.X = position.X + (int)((coord.Longitude - box.Position.Longitude) * ratio * GPSData.longtitudeCorrectionScale);
        marker.Y = position.Y + widgetSize.Height - (int)((coord.Latitude - box.Position.Latitude) * ratio);

        Pen pen = new Pen(ProjectSettings.GetSettings().PositionMarkerColor, 2);
        Brush brush = new SolidBrush(ProjectSettings.GetSettings().PositionMarkerColor);
        grfx.FillRectangle(brush, marker.X, marker.Y, ProjectSettings.GetSettings().PositionMarkerSize, ProjectSettings.GetSettings().PositionMarkerSize);
        grfx.DrawRectangle(pen, marker.X, marker.Y, ProjectSettings.GetSettings().PositionMarkerSize, ProjectSettings.GetSettings().PositionMarkerSize);
         * */
    }
}
