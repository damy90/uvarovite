﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


public class WidgetTrack : Widget
{
    Bitmap trackBitmap;
    Bitmap map;
    Point position;

    public override void Draw(Graphics grfx, float time)
    {
        GPSData gps = GPSData.GetData();
        GPSBox box = gps.GetBox();
        ProjectSettings settings = ProjectSettings.GetSettings();
        int wholeTrackLineWidth = settings.WholeTrackLineWidth;
        Pen wholeTrackPen = new Pen(settings.WholeTrackColor, wholeTrackLineWidth);
        

        if (trackBitmap == null)
        {
            PointF[] trackPoints;
            GPSPoint[] trackData = gps.GetTrack();
            trackPoints = new PointF[trackData.Length];

            Size widgetSize = new Size();
            widgetSize.Height = settings.TrackHeight;
            
            double ratio = (widgetSize.Height - wholeTrackLineWidth) / (box.Size.Lattitude);//avaiable size is slighly smaller due to line width
            double longtitudeCorrectionScale = GPSData.longtitudeCorrectionScale;
            position = settings.TrackPostion;
            widgetSize.Width = (int)Math.Ceiling(ratio * (box.Size.Longtitude * longtitudeCorrectionScale) + wholeTrackLineWidth);
            //TODO if default is enabled set this position
            position.X = System.Convert.ToInt32(grfx.VisibleClipBounds.Width) - widgetSize.Width - 20;
            position.Y = System.Convert.ToInt32(grfx.VisibleClipBounds.Height) - widgetSize.Height - 20;
            
            trackBitmap = new Bitmap(widgetSize.Width, widgetSize.Height);

            using (Graphics drawTrack = Graphics.FromImage(trackBitmap))
            {
                for (int i = 0; i < trackData.Length; i++)
                {
                    trackPoints[i].X = (float)((trackData[i].longitude - box.Position.Longtitude) * ratio * longtitudeCorrectionScale + ((float)wholeTrackLineWidth) / 2);
                    trackPoints[i].Y = widgetSize.Height - (float)((trackData[i].latitude - box.Position.Lattitude) * ratio + ((float)wholeTrackLineWidth) / 2);
                }
                drawTrack.DrawLines(wholeTrackPen, trackPoints);
            }
        }
        //TODO leave some space between the map frame and the track points, the map has to resize according to the track size
        //image for track
        if (map == null)
        {

            WebClient webClient = new WebClient();
            string path = @"http://maps.googleapis.com/maps/api/staticmap?size=200x200&path=color:0x00000000|weight:5|"
                           + (box.Position.Lattitude - 0.00).ToString() + ","
                           + (box.Position.Longtitude - 0.00).ToString() + "|"
                           + (box.Position.Lattitude + 0.00 + box.Size.Lattitude).ToString() + ','
                           + (box.Position.Longtitude + 0.00 + box.Size.Longtitude).ToString()
                           + "+%20&sensor=false";
            //MessageBox.Show(path);
            webClient.DownloadFile(path, "test.png");
            map = new Bitmap("test.png");
        }

        //todo oversize check 
        int x = System.Convert.ToInt32(grfx.VisibleClipBounds.Width) - map.Width;
        int y = System.Convert.ToInt32(grfx.VisibleClipBounds.Height) - map.Height;

        //MessageBox.Show(h.ToString()+" "+w.ToString());

        grfx.DrawImage(map, position);
        grfx.DrawImage(trackBitmap, position);
    }
}

/*
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
*/
/*
public class WidgetTrack:Widget
{
    Bitmap trackBitmap;
    PointF[] trackPoints;
    int prevIndex = 0;
    public override void Draw(Graphics grfx, float time)
    {
        ProjectSettings prj = ProjectSettings.GetSettings();
        GPSData gps = GPSData.GetData();
        // draw whole track
        if (trackBitmap == null)
        {
            int wholeTrackLineWidth = prj.WholeTrackLineWidth;
            Pen wholeTrackPen = new Pen(prj.WholeTrackColor, wholeTrackLineWidth );

            GPSPoint[] trackData = gps.GetTrack();
            trackPoints = new PointF[trackData.Length];
            GPSBox box = gps.GetBox();

            Size widgetSize = new Size();
            widgetSize.Height = prj.TrackHeight;
            double ratio = (widgetSize.Height - wholeTrackLineWidth) / (box.Size.Lattitude);//avaiable size is slighly smaller due to line width
            double longtitudeCorrectionScale = GPSData.longtitudeCorrectionScale;
            widgetSize.Width = (int)Math.Ceiling( ratio * (box.Size.Longtitude * longtitudeCorrectionScale) + wholeTrackLineWidth );

            trackBitmap = new Bitmap(widgetSize.Width, widgetSize.Height);

            using (Graphics drawTrack = Graphics.FromImage(trackBitmap))
            {
                for (int i = 0; i < trackData.Length; i++)
                {
                    trackPoints[i].X = (float)((trackData[i].longitude - box.Position.Longtitude) * ratio * longtitudeCorrectionScale + ((float)wholeTrackLineWidth) / 2);
                    trackPoints[i].Y = widgetSize.Height - (float)((trackData[i].latitude - box.Position.Lattitude) * ratio + ((float)wholeTrackLineWidth) / 2 );
                }
                drawTrack.DrawLines(wholeTrackPen, trackPoints);
            }
        }
        //draw track (traveled)
        //TODO: use interpolation
        int index = gps.GetIndex(time);
        if (prevIndex != null && index!=prevIndex)
        {
            PointF[] subTrackPoints = new PointF[index - prevIndex + 1];
            Array.Copy(trackPoints, prevIndex, subTrackPoints, 0, index - prevIndex + 1);//index - prevIndex + 1 = 2

            int traveledTrackLineWidth = prj.TraveledTrackLineWidth;
            Pen traveledTrackPen = new Pen(prj.TraveledTrackColor, traveledTrackLineWidth);

            using (Graphics drawTrack = Graphics.FromImage(trackBitmap))
            {
                if (subTrackPoints.Length>1)
                    drawTrack.DrawLines(traveledTrackPen, subTrackPoints);
            }
            prevIndex = index;
        }
        grfx.DrawImage(trackBitmap, prj.TrackPostion);
    }
}
*/