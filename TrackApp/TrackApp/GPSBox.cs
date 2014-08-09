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