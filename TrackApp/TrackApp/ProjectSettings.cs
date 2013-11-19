using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class ProjectSettings
{
    public string VideoInputPath = "video.avi";
    public string VideoOutputPath = "video-output.avi";
    public string GPXPath = "track.gpx";
    public VideoFormats Format= VideoFormats.xvid;

    //public TimeZone TimeZone = TimeZone.CurrentTimeZone;
    public float VideoStart = 0;//in seconds
    public float TrackStart = 0;//VideoStart and TrackStart is the only synchronization we need!!!
    public float VideoEnd = 0;//zero if video length is not trimmed

    public bool ShowTrack=true;
    public Point TrackCoordinates = new Point(0, 0);
    public int TrackHeight = 200;//the width is calculated from the track points 
    public Color TraveledTrackColor = Color.Red;
    public int TraveledTrackLineWidth = 3;
    public Color WholeTrackColor = Color.White;
    public int WholeTrackWidth = 9;

    public bool ShowPositionMarker=false;
    public int PositionMarkerSize = 10;
    public Color PositionMarker = Color.DarkGreen;

    public bool ShowOverlayImage=false;
    public string overlayImageFile="image.png";//draw a background image, like this: http://i.imgur.com/jjKmk.jpg
    public Point overlayImagePosition=new Point(300,300);

    public bool ShowElevationWidget=false;
    public Point ElevationWidgetPosition=new Point(300,0);
    public Font ElevationWidgetFont=new Font("Ariel", 28);
    public Color ElevationWidgetColor=Color.White;

    public bool ShowMap=false;
    public int MapHeight = 240;//should be bigger than the track height, width is bigger by the same value
    public float MapOpacity;

    public Point DistanceWidgetPosition = new Point(0, 300);
    public Font DistanceWidgetFont = new Font("Ariel", 28);
    public Color DistanceWidgetColor = Color.White;

    public Point SpeedWidgetPosition = new Point(0, 400);
    public Font SpeedWidgetFont = new Font("Ariel", 28);
    public Color SpeedWidgetColor = Color.White;

    private static ProjectSettings _instance;  //променлива за единствената инстанция на този клас

    // Constructor is 'protected' - конструтора е защитен и не може да бъде извикан
    protected ProjectSettings()
    {
 
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
  }
