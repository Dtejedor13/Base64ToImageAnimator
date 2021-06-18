using System.Drawing;

namespace GifToBase64Extractor
{
    class operatingImageModel
    {
        public operatingImageModel(Bitmap[] frames, string fileName, int height, int widht)
        {
            this.frames = frames;
            this.fileName = fileName;
            this.height = height;
            this.widht = widht;
        }

        public Bitmap[] frames { get; set; }
        public string fileName { get; set; }
        public int height { get; set; }
        public int widht { get; set; }
    }
}
