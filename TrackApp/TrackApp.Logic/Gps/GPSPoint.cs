using System;

namespace TrackApp.Logic.Gps
{
    public class GPSPoint : GPSCoord, IComparable<GPSPoint>
    {
        public float Time;
        public double Distance; // in meters

        public GPSPoint(double lon, double lat, double ele, float dt, double distance = 0)
            : base(lon, lat, ele)
        {
            this.Time = dt;
            this.Distance = distance;
        }

        public GPSPoint(float dt)
            : base(0, 0, 0)
        {
            this.Time = dt;
            this.Distance = 0;
        }

        public int CompareTo(GPSPoint other)
        {
            return this.Time.CompareTo(other.Time);
        }

        public double DistanceFromPoint(GPSPoint point2)
        {
            var startLatitudeRadians = this.Latitude * (Math.PI / 180.0);
            var startLongitudeRadians = this.Longitude * (Math.PI / 180.0);
            var endLatitudeRadians = point2.Latitude * (Math.PI / 180.0);
            var endLongitudeRadians = point2.Longitude * (Math.PI / 180.0);

            var distanceLongitude = endLongitudeRadians - startLongitudeRadians;
            var distanceLatitude = endLatitudeRadians - startLatitudeRadians;
            var distanceElevation = Math.Abs(this.Elevation - point2.Elevation);

            var result1 = Math.Sin(distanceLatitude / 2.0) * Math.Sin(distanceLatitude / 2.0) +
                          Math.Cos(startLatitudeRadians) * Math.Cos(endLatitudeRadians) *
                          Math.Pow(Math.Sin(distanceLongitude / 2.0), 2.0);

            // Using 6367500 as the radius around the earth
            var result2 = 6367500 * 2.0 *
                          Math.Atan2(Math.Sqrt(result1), Math.Sqrt(1.0 - result1));

            // TODO use elevation
            return result2; // Math.Sqrt(result2 * result2 + distanceElevation * distanceElevation);
        }
    }
}