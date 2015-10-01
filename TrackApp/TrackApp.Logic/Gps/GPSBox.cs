namespace TrackApp.Logic.Gps
{
    /// <summary>
    /// A box in an elevation map with position and size in latitude, longitude and elevation
    /// </summary>
    public struct GPSBox
    {
        //TODO: encapsulate
        public GPSCoord Position;
        public GPSCoord Size;

        /// <summary>
        /// A box in an eleavtion map with position and size in latitude, longitude and elevation
        /// </summary>
        /// <param name="position">The position of the bottom-left-front angle of the box</param>
        /// <param name="size">max latitude - min latitude, max longitude - min longitude, max elevation - min elevation</param>
        public GPSBox(GPSCoord position, GPSCoord size)
        {
            this.Position = position;
            this.Size = size;
        }
    }
}