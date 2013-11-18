using LinqXMLTester;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace LinqXMLTester
{
    class Data : GPXLoader
    {
        public Data()
            : base()
        {
            
        }

        public double Distance(int firstPoint, int secondPoint)
        {
            double lat1 = base.points[firstPoint].Latitude;
            double lon1 = base.points[firstPoint].Longitude;
            double lat2 = base.points[secondPoint].Latitude;
            double lon2 = base.points[secondPoint].Longitude;

            double theta = lon1 - lon2;

            double dist = Math.Sin(deg2rad(lat1)) * Math.Sin(deg2rad(lat2)) + Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) * Math.Cos(deg2rad(theta));

            dist = Math.Acos(dist);

            dist = rad2deg(dist);

            dist = dist * 60 * 1.1515;

            dist = dist * 1.609344;

            return (dist);// in km
        }

        private double deg2rad(double deg)
        {
            return (deg * Math.PI / 180.0);
        }

        private double rad2deg(double rad)
        {
            return (rad / Math.PI * 180.0);
        }

        public double DeltaTime(int firstPoint, int secondPoint)
        {
            DateTime firstDate = this.points[firstPoint].Date;
            DateTime secondDate = this.points[secondPoint].Date;
            TimeSpan deltaTime = (firstDate - secondDate);
            return Math.Abs(double.Parse(deltaTime.TotalSeconds.ToString())); //seconds
        }

        public double CurrentSpeed(int firstPoint, int secondPoint)
        {
            double time = DeltaTime(firstPoint, secondPoint);
            double path = Distance(firstPoint, secondPoint);

            return (path / time * 60 * 60); //return speed (km per hour)
        }

        public int Count()
        {
            return base.points.Count();
        }
    }
}
