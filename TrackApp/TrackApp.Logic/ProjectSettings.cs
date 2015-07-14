using System.Drawing;
using System.IO;
using System.Xml.Serialization;

namespace TrackApp.Logic
{
    /// <summary>
    /// Project settings. This class is a singleton and cannot be initialized with a constructor.
    /// </summary>
    public sealed class ProjectSettings
    {
        public string VideoInputPath; // = @"C:\Users\Dany\Desktop\GOPR2340.avi";
        public string VideoOutputPath; // = @"C:\Users\Dany\Desktop\video-out.avi";
        public string GPXPath; // = @"C:\Users\Dany\Desktop\Debug\workout.gpx";
        public VideoFormats Format = VideoFormats.WMV2;
        ////public int BitRate = 50000000;//2 Mbit/s

        ////public TimeZone TimeZone = TimeZone.CurrentTimeZone;

        public float VideoStart = 60; // in seconds
        public float TrackStart = 0;
        public float VideoEnd = 240; // zero if video length is not trimmed
        public float TrackEnd = 0;
        public int VideoSpeed = 8; // speed multiplier 
        public int VideoQuality = 7; // Bitrate = quality * 1 000 000 

        public bool ShowTrack = true;

        public Point TrackPostion = new Point(80, 80);
        public int TrackHeight = 20; // the width is calculated from the track points 
        [XmlIgnoreAttribute]
        public Color TraveledTrackColor = Color.Red;
        public int TraveledTrackLineWidth = 2;
        [XmlIgnoreAttribute]
        public Color WholeTrackColor = Color.Yellow;
        public int WholeTrackLineWidth = 6;
        public bool ShowTraveledTrack = true;

        public bool ShowPositionMarker = false;
        public bool ShowOrientation = true; // TODO Add to gui
        public int PositionMarkerSize = 10;
        [XmlIgnoreAttribute]
        public Color PositionMarkerColor = Color.DarkGreen;

        public bool ShowOverlayImage = false;
        public string OverlayImageFile;
        public Point OverlayImagePosition = new Point(300, 300);

        // todo remoove
        public bool ShowElevationWidget = false;
        [XmlIgnoreAttribute]
        public Font ElevationWidgetFont = new Font("Ariel", 28);
        public Point ElevationWidgetPosition = new Point(300, 0);
        [XmlIgnoreAttribute]
        public Color ElevationWidgetColor = Color.White;
        public bool ShowMap = false;
        public int MapHeight = 240; // should be bigger than the track height, width is bigger by the same value
        public float MapOpacity;

        public bool ShowDistanceWidget = false;
        [XmlIgnoreAttribute]
        public Font DistanceWidgetFont = new Font("Ariel", 28);
        public Point DistanceWidgetPosition = new Point(30, 95);
        [XmlIgnoreAttribute]
        public Color DistanceWidgetColor = Color.White;
        public bool ShowSpeedWidget = false;
        [XmlIgnoreAttribute]
        public Font SpeedWidgetFont = new Font("Ariel", 28);
        public Point SpeedWidgetPosition = new Point(3, 95);
        [XmlIgnoreAttribute]
        public Color SpeedWidgetColor = Color.White;

        private static ProjectSettings _instance;  // променлива за единствената инстанция на този клас

        // Constructor is 'protected' - конструтора е защитен и не може да бъде извикан
        protected ProjectSettings()
        {
        }

        // Color serialization
        [XmlElement("TraveledTrackColor")]
        public int TraveledTrackColorAsArgb
        {
            get
            {
                return this.TraveledTrackColor.ToArgb();
            }

            set
            {
                this.TraveledTrackColor = Color.FromArgb(value);
            }
        }

        [XmlElement("WholeTrackColor")]
        public int WholeTrackColorAsArgb
        {
            get
            {
                return this.WholeTrackColor.ToArgb();
            }

            set
            {
                this.WholeTrackColor = Color.FromArgb(value);
            }
        }

        [XmlElement("PositionMarkerColor")]
        public int PositionMarkerColorAsArgb
        {
            get
            {
                return this.PositionMarkerColor.ToArgb();
            }

            set
            {
                this.PositionMarkerColor = Color.FromArgb(value);
            }
        }

        [XmlElement("ElevationWidgetColor")]
        public int ElevationWidgetColorAsArgb
        {
            get
            {
                return this.ElevationWidgetColor.ToArgb();
            }

            set
            {
                this.ElevationWidgetColor = Color.FromArgb(value);
            }
        }

        [XmlElement("DistanceWidgetColor")]
        public int DistanceWidgetColorAsArgb
        {
            get
            {
                return this.DistanceWidgetColor.ToArgb();
            }

            set
            {
                this.DistanceWidgetColor = Color.FromArgb(value);
            }
        }

        [XmlElement("SpeedWidgetColor")]
        public int SpeedWidgetColorAsArgb
        {
            get
            {
                return this.SpeedWidgetColor.ToArgb();
            }

            set
            {
                this.SpeedWidgetColor = Color.FromArgb(value);
            }
        }

        // Font serialization
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
                return new SerializableFont(this.DistanceWidgetFont);
            }

            set
            {
                this.DistanceWidgetFont = value.ToFont();
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

        // единтвения начин за инстанцииране е от тук
        public static ProjectSettings GetSettings()
        {
            if (_instance == null)
            {
                _instance = new ProjectSettings();
            }

            return _instance;
        }

        /// <summary>
        /// Writes all project settings to a .xml file
        /// </summary>
        /// <param name="path">File save location</param>
        public void Serialize(string path = "saved-settings.xml")
        {
            XmlSerializer x = new XmlSerializer(GetType());
            StreamWriter file = new StreamWriter(path);
            x.Serialize(file, this);
            file.Close();
        }

        /// <summary>
        /// Reads all project settings from a .xml file
        /// </summary>
        /// <param name="path">Path to .xml file</param>
        /// <returns>The ProjectSettings instance</returns>
        public ProjectSettings Deserialize(string path = "saved-settings.xml")
        {
            XmlSerializer x = new XmlSerializer(GetType());
            StreamReader file = new StreamReader(path);
            _instance = (ProjectSettings)x.Deserialize(file);
            file.Close();
            return _instance;
        }
    }
}
