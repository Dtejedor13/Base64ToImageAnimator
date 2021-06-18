namespace Base64ConverterCore.Models
{
    public class FrameProperties
    {
        public FrameProperties(string fileName, int height, int widht)
        {
            this.fileName = fileName;
            this.height = height;
            this.widht = widht;
        }

        public string fileName { get; set; }
        public int height { get; set; }
        public int widht { get; set; }
    }
}
