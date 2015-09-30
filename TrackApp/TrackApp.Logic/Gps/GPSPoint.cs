using System;

namespace TrackApp.Logic.Gps
{
    /// <summary>
    /// Contains GPS coordinates as well as the time the point was recorded and distance from previous point.
    /// </summary>
    public class GPSPoint : GPSCoord, IComparable<GPSPoint>
    {
        public float Time;
        public double Distance; // in meters

        /// <summary>
        /// Contains GPS coordinates as well as the time the point was recorded and distance from previous point.
        /// </summary>
        /// <param name="lon">longtitude</param>
        /// <param name="lat">latitude</param>
        /// <param name="ele">elevation</param>
        /// <param name="dt">time</param>
        /// <param name="distance">distance from previous point in meters</param>
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

        /// <summary>
        /// Compare this GPSPoint to another based on time
        /// </summary>
        public int CompareTo(GPSPoint other)
        {
            return this.Time.CompareTo(other.Time);
        }

        /// <summary>
        /// The distance from this point to point 2 assuming the earth is perfectly spherical with radius 6367500 (approximately the average radius)
        /// </summary>
        /// <param name="point2">The next point</param>
        /// <returns>The distance in meters</returns>
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