using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class ProjectSettings
{
    public string VideoInputPath = @"video.avi";
    public string VideoOutputPath="video-out.avi";
    public string GPXPath = "workout.gpx";
    public VideoFormats Format= VideoFormats.xvid;
    public int BitRate = 20000000;//2 Mbit/s

    //public TimeZone TimeZone = TimeZone.CurrentTimeZone;
    public float VideoStart = 0;//in seconds
    public float TrackStart = 0;//VideoStart and TrackStart is the only synchronization we need!!!
    public float VideoEnd = 0;//zero if video length is not trimmed
    public int VideoSpeed = 8; //speed multiplier
    public int VideoQuality = 5;//Bitrate = quality * 1 000 000

    public bool ShowTrack=true;//TO DO: Finish track widget and change to true
    public Point TrackPostion = new Point(100, 0);
    public int TrackHeight = 250;//the width is calculated from the track points 
    public Color TraveledTrackColor = Color.Red;
    public int TraveledTrackLineWidth = 3;
    public Color WholeTrackColor = Color.White;
    public int WholeTrackLineWidth = 7;

    public bool ShowPositionMarker=false;
    public int PositionMarkerSize = 10;
    public Color PositionMarkerColor = Color.DarkGreen;
    //TODO add classes for the rest of the widgets
    public bool ShowOverlayImage=false;
    public string overlayImageFile;//draw a background image, like this: http://i.imgur.com/jjKmk.jpg
    public Point overlayImagePosition=new Point(300,300);

    public bool ShowElevationWidget=false;
    public Point ElevationWidgetPosition=new Point(300,0);
    public Font ElevationWidgetFont=new Font("Ariel", 28);
    public Color ElevationWidgetColor=Color.White;

    public bool ShowMap=false;
    public int MapHeight = 240;//should be bigger than the track height, width is bigger by the same value
    public float MapOpacity;

    public bool ShowDistanceWidget = false;
    public Point DistanceWidgetPosition = new Point(0, 300);
    public Font DistanceWidgetFont = new Font("Ariel", 28);
    public Color DistanceWidgetColor = Color.White;

    public bool ShowSpeedWidget = false;
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
