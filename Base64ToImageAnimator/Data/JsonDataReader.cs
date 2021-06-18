using Base64ConverterCore.Models;
using Base64ToImageAnimator.Converter;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Media;

namespace Base64ToImageAnimator.Data
{
    public static class JsonDataReader
    {
        private const string JSONDATAMODULE = "results.json";

        public static List<ImageSource> LoadSpriteSheet()
        {
            string json = Read(JSONDATAMODULE);
            List<ImageSource> sheet = new List<ImageSource>();
            Base64Converter converter = new Base64Converter();
            List<DataModel> example = JsonConvert.DeserializeObject<List<DataModel>>(json);
            foreach (string sprite in example[0].bs)
            {
                sheet.Add(converter.ImageSourceFromBitmap(converter.Base64StringToBitmap(sprite)));
            }

            return sheet;
        }

        public static FrameProperties LoadFrameProperties()
        {
            string json = Read(JSONDATAMODULE);
            List<DataModel> example = JsonConvert.DeserializeObject<List<DataModel>>(json);
            FrameProperties props = new FrameProperties(
                example[0].bs_props.fileName,
                example[0].bs_props.height,
                example[0].bs_props.widht);
            
            return props;
        }

        private static string Read(string filename)
        {
            string result = "";
            var assembly = Assembly.GetExecutingAssembly();
            string resourceName = $"Base64ToImageAnimator.Data.{filename}";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader sr = new StreamReader(stream))
            {
                result = sr.ReadToEnd();
            }

            return result;
        }
    }
}
