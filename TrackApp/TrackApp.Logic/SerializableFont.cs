using System.Drawing;

namespace TrackApp.Logic
{
    /// <summary>
    /// Class for serializing and deserializing Font objects.
    /// </summary>
    public class SerializableFont
    {
        public SerializableFont(Font f)
        {
            this.FontFamily = f.FontFamily.Name;
            this.GraphicsUnit = f.Unit;
            this.Size = f.Size;
            this.Style = f.Style;
        }

        private SerializableFont()
        {
        }

        public string FontFamily { get; set; }

        public GraphicsUnit GraphicsUnit { get; set; }

        public float Size { get; set; }

        public FontStyle Style { get; set; }

        public Font ToFont()
        {
            return new Font(this.FontFamily, this.Size, this.Style, this.GraphicsUnit);
        }
    }
}