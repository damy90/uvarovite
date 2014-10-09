namespace TrackApp.Logic.Gps
{
    public class GPSCoord
    {
        public double Longitude;
        public double Latitude;
        public double Elevation;

        public GPSCoord(double longitude, double latitude, double elevation)
        {
            this.Longitude = longitude;
            this.Latitude = latitude;
            this.Elevation = elevation;
        }
    }
}