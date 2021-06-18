using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using Xamarin.Forms;

namespace Base64Animator.Converter
{
    public class XamarinBase64Converter
    {
        public Bitmap Base64StringToBitmap(string base64String)
        {
            Bitmap bmpReturn = null;
            byte[] byteBuffer = Convert.FromBase64String(base64String);
            MemoryStream memoryStream = new MemoryStream(byteBuffer);

            memoryStream.Position = 0;

            bmpReturn = (Bitmap)Bitmap.FromStream(memoryStream);

            memoryStream.Close();
            // clear some ram
            memoryStream = null;
            byteBuffer = null;

            return bmpReturn;
        }

        [Obsolete]
        public ImageSource ConvertBitmapToImageSource(Bitmap image)
        {
            ImageSourceConverter c = new ImageSourceConverter();
            return (ImageSource)c.ConvertFrom(image);
        }

        public ImageSource GetImageSourceFromBase64(string base64)
        {
            return ImageSource.FromStream(() => new MemoryStream(Convert.FromBase64String(base64)));
        }
    }
}
