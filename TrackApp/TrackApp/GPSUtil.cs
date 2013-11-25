using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Windows;


public struct GPSPoint : IComparable<GPSPoint>
{
    public double longitude;
    public double latitude;
    public double elevation;
    public float time;
    public double Distance;//in meters
    public GPSPoint(double lon, double lat, double ele, float dt, double distance) 
    {
        this.longitude = lon;
        this.latitude = lat;
        this.elevation = ele;
        this.time = dt;
        this.Distance = distance;
    }
    public GPSPoint(float dt){
        this.time = dt;
        this.longitude = this.latitude = this.elevation = this.Distance = 0;
    }
    public int CompareTo(GPSPoint other)
    {
        return time.CompareTo(other.time);
    }
    public double DistanceFromPoint(GPSPoint point2)
    {
        var startLatitudeRadians = latitude * (Math.PI / 180.0);
        var startLongitudeRadians = longitude * (Math.PI / 180.0);
        var endLatitudeRadians = point2.latitude * (Math.PI / 180.0);
        var endLongitudeRadians = point2.longitude * (Math.PI / 180.0);

        var distanceLongitude = endLongitudeRadians - startLongitudeRadians;
        var distanceLatitude = endLatitudeRadians - startLatitudeRadians;
        var distanceElevation = Math.Abs(elevation - point2.elevation);

        var result1 = Math.Pow(Math.Sin(distanceLatitude / 2.0), 2.0) +
                        Math.Cos(startLatitudeRadians) * Math.Cos(endLatitudeRadians) *
                        Math.Pow(Math.Sin(distanceLongitude / 2.0), 2.0);

        // Using 40075040 as the number of meters around the earth
        var result2 = 40075040 * 2.0 *
                        Math.Atan2(Math.Sqrt(result1), Math.Sqrt(1.0 - result1));

        return Math.Sqrt(result2 * result2 + distanceElevation * distanceElevation);
    }
}

public struct GPSCoord
{
    public double Longtitude;
    public double Lattitude;
    public double Elevation;
    public GPSCoord(double longtitude, double lattitude, double elevation)
    {
        this.Longtitude = longtitude;
        this.Lattitude = lattitude;
        this.Elevation = elevation;
    }
}

public struct GPSBox
{
    public GPSCoord Position;
    public GPSCoord Size;
    public GPSBox(GPSCoord position, GPSCoord size)
    {
        this.Position = position;
        this.Size = size;
    }
}

public class GPSData
{
    GPSPoint[] gpsPoints;

    private double maxLong = double.MinValue;
    private double maxLat = double.MinValue;
    private double maxEle = double.MinValue;

    private double minLong = double.MaxValue;
    private double minLat = double.MaxValue;
    private double minEle = double.MaxValue;

    private static GPSData _instance;

    private GPSData() { }

    public static GPSData GetData()
    {
        if( _instance==null )
            _instance = new GPSData();
        return _instance;
    }


    public int GetPointCount()
    {
        return gpsPoints.Length;
    }

    public void LoadGPXFile(string sFile)
    {
        //
    }

    public void Update(List<GPSPoint> pts )
    {
        //TODO - trim, leave only the gpsPoints that we will need (using TrackStart and min((VideoEnd-VideoStart),TrackLength) )
        gpsPoints = new GPSPoint[pts.Count];//we need an array rather than list for faster access
        int n = 0;
        foreach(GPSPoint point in pts)
        {
            gpsPoints[n] = point;
            maxLong = Math.Max(gpsPoints[n].longitude, maxLong);
            maxLat = Math.Max(gpsPoints[n].latitude, maxLat);
            maxEle = Math.Max(gpsPoints[n].elevation, maxEle);

            minLong = Math.Min(gpsPoints[n].longitude, maxLong);
            minLat = Math.Min(gpsPoints[n].latitude, maxLat);
            minEle = Math.Min(gpsPoints[n].elevation, maxEle);

            n++;
        }

        double distanceSum = 0;
        for (int i=1; i<gpsPoints.Length;i++)
        {
            distanceSum += gpsPoints[i].DistanceFromPoint(gpsPoints[i - 1]);
            gpsPoints[i].Distance = distanceSum;
        }
    }

    public GPSBox GetBox()
    {
        GPSCoord position = new GPSCoord(minLong, minLat, minEle);
        GPSCoord size = new GPSCoord(maxLong - minLong, maxLat - minLat, maxEle - minEle);
        return new GPSBox(position, size);
    }
    //TODO average speed (gradually change speed)
    public double GetSpeed(float time)
    {
        int index=IndexThisOrPreviousReading(time);
        if (index == gpsPoints.Length - 1)
            return 0;
        double distance = gpsPoints[index].DistanceFromPoint(gpsPoints[index + 1]);
        float timeSpan = gpsPoints[index + 1].time - gpsPoints[index].time;
        return distance / timeSpan;
    }
    public GPSCoord GetPosition(float time)
    {
        int index = IndexThisOrPreviousReading(time);
        if (index >= gpsPoints.Length - 1)
            return new GPSCoord(gpsPoints[gpsPoints.Length - 1].longitude, gpsPoints[gpsPoints.Length - 1].longitude, gpsPoints[gpsPoints.Length - 1].elevation);
        double Long = Interpolate(time, gpsPoints[index].longitude, gpsPoints[index + 1].longitude, gpsPoints[index].time, gpsPoints[index + 1].time);
        double lat = Interpolate(time,gpsPoints[index].longitude, gpsPoints[index + 1].latitude, gpsPoints[index].time, gpsPoints[index + 1].time);
        double ele = Interpolate(time, gpsPoints[index].elevation, gpsPoints[index + 1].elevation, gpsPoints[index].time, gpsPoints[index + 1].time);
        return new GPSCoord(Long, lat, ele);
    }
    //In case some readings are made over greater time intervals (if you are in a tunel or loose signal, etc)
    private int IndexThisOrPreviousReading(float time)
    {
        int index = Array.BinarySearch(gpsPoints, new GPSPoint(time));
        if (index < 0) // the value 10 wasn't found
        {
            index = ~index;
            index -= 1;
        }
        return index;
    }
    private double Interpolate(float time, double previousReading, double nextReading, float previousTime, float nextTime)
    {
        if (nextTime == previousTime)
            throw new DivideByZeroException("Division by zero: Two readings were taken at the same time");
        return previousReading + (nextReading - previousReading) * (time - previousTime) / (nextTime - previousTime);
    }
}

//TODO - leave only the abstract class (if we have class properties) or the interface (if we have only methods)
public interface IGPSReader
{
    int LoadPoints(string sFile);
}

public abstract class GPSLoader : IGPSReader
{
    public abstract int LoadPoints(string sFile);
}
//TO DO: Implement Track Start time
public class GPXFileLoader : GPSLoader
{
    //GPSData gpsData = new GPSData();
    List<GPSPoint> pts = new List<GPSPoint>();
    public override int LoadPoints(string sFile)
    {            
        XDocument gpxDoc = XDocument.Load(sFile);
        XNamespace gpxNamespace = XNamespace.Get("http://www.topografix.com/GPX/1/1");
            
        var points = from point in gpxDoc.Descendants(gpxNamespace + "trkpt")  //trk/trkseg/trkpt
                        select new
                        {
                            Latitude  = point.Attribute("lat").Value,
                            Longitude = point.Attribute("lon").Value,
                            Elevation = point.Element(gpxNamespace + "ele") != null ?
                                        point.Element(gpxNamespace + "ele").Value : null,
                            Dt        = point.Element(gpxNamespace + "time") != null ?
                                        point.Element(gpxNamespace + "time").Value : null
                        };            

        StringBuilder sb = new StringBuilder();
        DateTime startTime = new DateTime();
        bool isStart = true;
        foreach (var pt in points)
        {
            if (isStart)
            {
                startTime = System.Convert.ToDateTime(pt.Dt);
                isStart = false;
            }
            // This is where we'd instantiate data
            // containers for the information retrieved.
            sb.Append(
                string.Format("Latitude:{0} Longitude:{1} Elevation:{2} Date:{3}\n",
                pt.Latitude, pt.Longitude,
                pt.Elevation, pt.Dt));
            //int timeSpan=
            pts.Add(new GPSPoint(System.Convert.ToDouble(pt.Latitude),
                                            System.Convert.ToDouble(pt.Longitude),
                                            System.Convert.ToDouble(pt.Elevation),
                                            //System.Convert.ToDateTime(pt.Dt)
                                            (float)(System.Convert.ToDateTime(pt.Dt) - startTime).TotalSeconds,
                                            0
                                            )); //new GPSPoint(20f, 30f, 40f, DateTime.Now)

            MessageBox.Show(string.Format("Latitude:{0} Longitude:{1} Elevation:{2} Date:{3}\n", pt.Latitude, pt.Longitude, pt.Elevation, pt.Dt));
        }

        MessageBox.Show(pts.Count.ToString());
        GPSData.GetData().Update(pts);
        return GPSData.GetData().GetPointCount();
    } 
}
