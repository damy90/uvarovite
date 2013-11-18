
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqXMLTester
{
    class Program
    {
        static void Main(string[] args)
        { 
            string fileName  = "../../FILE4.gpx";
            Data path = new Data();
            path.LoadGPXTracks(fileName);

            double time = 0;

            Console.WriteLine("LonMinValue: {0}", path.LonMinValue);
            Console.WriteLine("LonMaxValue: {0}", path.LonMaxValue);
            Console.WriteLine("LatMinValue: {0}", path.LatMinValue);
            Console.WriteLine("LatMaxValue: {0}", path.LatMaxValue);

            Console.WriteLine("Press Key.");
            Console.ReadLine();

            for (int i = 1; i < path.Count(); i++)
            {

                time += (path.DeltaTime(i - 1, i));
                Console.WriteLine("{0}.", i);
                Console.WriteLine("deltaTime: {0:f2} second(s)", path.DeltaTime(i - 1, i));// 
                Console.WriteLine("distance: {0:f3} km.", path.Distance(i - 1, i));// return distance in km
                Console.WriteLine("current speed: {0:f2} km/h", path.CurrentSpeed(i - 1, i));
                Console.WriteLine("current time: {0:f2} sec.", time);
                Console.WriteLine("Lat: {0}", path.points[i].Latitude);
                Console.WriteLine("Lon: {0}", path.points[i].Longitude);
                Console.WriteLine("Ele: {0}", path.points[i].Elevation);
                Console.WriteLine("Time: {0}", path.points[i].Date);
                Console.WriteLine("============================");
            }
            
        }

    }
}
