using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

public class SerializableFont
{
    public string FontFamily { get; set; }
    public GraphicsUnit GraphicsUnit { get; set; }
    public float Size { get; set; }
    public FontStyle Style { get; set; }

    private SerializableFont() { }

    public SerializableFont(Font f)
    {
        FontFamily = f.FontFamily.Name;
        GraphicsUnit = f.Unit;
        Size = f.Size;
        Style = f.Style;
    }

    public Font ToFont()
    {
        return new Font(FontFamily, Size, Style,
            GraphicsUnit);
    }
}

public sealed class ProjectSettings
{
    public string VideoInputPath = @"C:\Users\Dany\Desktop\GOPR2340.avi";
    public string VideoOutputPath = @"C:\Users\Dany\Desktop\video-out.avi";
    public string GPXPath = @"C:\Users\Dany\Desktop\Debug\workout.gpx";
    public VideoFormats Format= VideoFormats.WMV2;
    //public int BitRate = 50000000;//2 Mbit/s

    //public TimeZone TimeZone = TimeZone.CurrentTimeZone;

    public float VideoStart = 60;//in seconds
    public float TrackStart = 0;
    public float VideoEnd = 240;//zero if video length is not trimmed
    public float TrackEnd = 0;
    public int VideoSpeed = 8; //speed multiplier 
    public int VideoQuality = 7;//Bitrate = quality * 1 000 000 

    public bool ShowTrack=true;//TO DO: Finish track widget and change to true

    public Point TrackPostion = new Point(80, 80);
    public int TrackHeight = 20;//the width is calculated from the track points 
    public Color TraveledTrackColor = Color.Red;
    public int TraveledTrackLineWidth = 2;
    public Color WholeTrackColor = Color.White;
    public int WholeTrackLineWidth = 6;
    public bool ShowTraveledTrack = true;

    public bool ShowPositionMarker=false;
    public bool ShowOrientation = true;//TODO Add to gui
    public int PositionMarkerSize = 10;
    public Color PositionMarkerColor = Color.DarkGreen;
    //TODO add classes for the rest of the widgets
    public bool ShowOverlayImage=false;
    public string overlayImageFile;//draw a background image, like this: http://i.imgur.com/jjKmk.jpg
    public Point overlayImagePosition=new Point(300,300);
    //todo remoove
    public bool ShowElevationWidget=false;
    [XmlIgnoreAttribute]
    public Font ElevationWidgetFont = new Font("Ariel", 28);
    public Point ElevationWidgetPosition=new Point(300,0);
    public Color ElevationWidgetColor=Color.White;

    public bool ShowMap=false;
    public int MapHeight = 240;//should be bigger than the track height, width is bigger by the same value (remoove
    public float MapOpacity;

    public bool ShowDistanceWidget = false;
    [XmlIgnoreAttribute]
    public Font DistanceWidgetFont = new Font("Ariel", 28);
    public Point DistanceWidgetPosition = new Point(30, 95);
    public Color DistanceWidgetColor = Color.White;

    public bool ShowSpeedWidget = false;
    [XmlIgnoreAttribute]
    public Font SpeedWidgetFont = new Font("Ariel", 28);
    public Point SpeedWidgetPosition = new Point(3, 95);
    public Color SpeedWidgetColor = Color.White;

    private static ProjectSettings _instance;  //променлива за единствената инстанция на този клас

    // Constructor is 'protected' - конструтора е защитен и не може да бъде извикан
    protected ProjectSettings()
    {
    }

    public SerializableFont PSpeedWidgetFont
    {
        get
        {
            return new SerializableFont(this.SpeedWidgetFont);
        }
        set
        {
            this.SpeedWidgetFont = value.ToFont();
        }
    }

    public SerializableFont PDistanceWidgetFont
    {
        get
        {
            return new SerializableFont(this.SpeedWidgetFont);
        }
        set
        {
            this.SpeedWidgetFont = value.ToFont();
        }
    }

    public SerializableFont PElevationWidgetFont
    {
        get
        {
            return new SerializableFont(this.SpeedWidgetFont);
        }
        set
        {
            this.SpeedWidgetFont = value.ToFont();
        }
    }

    //единтвения начин за инстанцииране е от тук
    public static ProjectSettings GetSettings()
    {
      if (_instance == null)
      {
        _instance = new ProjectSettings();
      }
      return _instance;
    }

    public void Serialize(string path = "saved-settings.xml")
    {
        XmlSerializer x = new XmlSerializer(GetType());
        StreamWriter file = new StreamWriter(path);
        x.Serialize(file, this);
        file.Close();
    }
    public ProjectSettings Deserialize(string path = "saved-settings.xml")
    {
        XmlSerializer x = new XmlSerializer(GetType());
        StreamReader file = new StreamReader(path);
        _instance = (ProjectSettings)x.Deserialize(file);
        file.Close();
        return _instance;
    }
  }
