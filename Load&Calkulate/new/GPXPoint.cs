using System;

namespace LinqXMLTester
{
    public struct GPXPoint
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Elevation { get; set; }
        public DateTime Date { get; set; }
    }
}