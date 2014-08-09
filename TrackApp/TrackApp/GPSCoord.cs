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

//TODO - leave only the abstract class (if we have class properties) or the interface (if we have only methods)

/*public abstract class GPSLoader : IGPSReader
{
    public abstract int LoadPoints(string sFile);
}*/