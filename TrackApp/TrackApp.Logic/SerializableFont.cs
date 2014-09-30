using System.Drawing;

namespace TrackApp.Logic
{
    public class SerializableFont
    {
        public string FontFamily { get; set; }

        public GraphicsUnit GraphicsUnit { get; set; }

        public float Size { get; set; }

        public FontStyle Style { get; set; }

        private SerializableFont()
        {
        }

        public SerializableFont(Font f)
        {
            FontFamily = f.FontFamily.Name;
            GraphicsUnit = f.Unit;
            Size = f.Size;
            Style = f.Style;
        }

        public Font ToFont()
        {
            return new Font(FontFamily, Size, Style,
                GraphicsUnit);
        }
    }
}