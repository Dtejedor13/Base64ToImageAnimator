using Base64Animator.Converter;
using Base64ConverterCore.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;

namespace Base64Animator.Data
{
    public static class JsonDataReader
    {
        private const string JSONDATAMODULE = "Data.json";
        private static XamarinBase64Converter converter = new XamarinBase64Converter();

        public static List<ImageSource> LoadSpriteSheet(int id, int fid, string animationType)
        {
            string json = Read(JSONDATAMODULE);
            List<DataModel> example = JsonConvert.DeserializeObject<List<DataModel>>(json);
            List<ImageSource> sheet = new List<ImageSource>();

            foreach (AnimationSet set in example.Where(x => x.formID == fid && x.id == id).FirstOrDefault().animationsets)
            {
                if(set.animationType == animationType)
                {
                    foreach (string sprite in set.spriteSheet)
                    {
                        sheet.Add(converter.GetImageSourceFromBase64(sprite));
                    }
                }
            }

            return sheet;
        }

        public static FrameProperties LoadFrameProperties(int id, int fid, string animationType)
        {
            string json = Read(JSONDATAMODULE);
            List<DataModel> example = JsonConvert.DeserializeObject<List<DataModel>>(json);
            foreach (AnimationSet set in example.Where(x => x.formID == fid && x.id == id).FirstOrDefault().animationsets)
            {
                if (set.animationType == animationType)
                {
                    return new FrameProperties(
                        set.properties.fileName,
                        set.properties.height,
                        set.properties.widht);
                }
            }
            return null;
        }

        public static List<int> GetAllUnitIDs()
        {
            string json = Read(JSONDATAMODULE);
            List<DataModel> example = JsonConvert.DeserializeObject<List<DataModel>>(json);
            List<int> ids = new List<int>();
            foreach (DataModel item in example)
            {
                if (!ids.Contains(item.id))
                    ids.Add(item.id);
            }
            return ids;
        }

        public static List<int> GetAllFormIDs(int id)
        {
            string json = Read(JSONDATAMODULE);
            List<DataModel> example = JsonConvert.DeserializeObject<List<DataModel>>(json);
            List<int> ids = new List<int>();
            foreach (DataModel item in example)
            {
                if (!ids.Contains(item.formID) && item.id == id)
                    ids.Add(item.formID);
            }
            return ids;
        }

        public static List<string> GetAllAnimationTypes(int id, int fid)
        {
            string json = Read(JSONDATAMODULE);
            List<DataModel> example = JsonConvert.DeserializeObject<List<DataModel>>(json);
            List<string> types = new List<string>();
            foreach (DataModel item in example)
            {
               if(item.formID == fid && item.id == id)
                    foreach (AnimationSet anniSets in item.animationsets)
                    {
                        if (!types.Contains(anniSets.animationType))
                            types.Add(anniSets.animationType);
                    }
            }
            return types;
        }

        private static string Read(string filename)
        {
            string result = "";
            var assembly = Assembly.GetExecutingAssembly();
            string resourceName = $"Base64Animator.Data.{filename}";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader sr = new StreamReader(stream))
            {
                result = sr.ReadToEnd();
            }

            return result;
        }
    }
}
