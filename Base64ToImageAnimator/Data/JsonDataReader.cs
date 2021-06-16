using Base64ToImageAnimator.Converter;
using Base64ToImageAnimator.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Media;

namespace Base64ToImageAnimator.Data
{
    public static class JsonDataReader
    {
        public static List<ImageSource> LoadSpriteSheet()
        {
            string json = Read("Data.json");
            List<ImageSource> sheet = new List<ImageSource>();
            Base64Converter converter = new Base64Converter();
            DataImportModel example = JsonConvert.DeserializeObject<DataImportModel>(json);
            foreach (string sprite in example.bs)
            {
                sheet.Add(converter.ImageSourceFromBitmap(converter.Base64StringToBitmap(sprite)));
            }

            return sheet;
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
