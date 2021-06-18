using Base64ConverterCore.Models;
using Base64ToImageAnimator.Converter;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace GifToBase64Extractor
{
    class Program
    {
        private static List<string> animationTypes = new List<string>()
        {
            "a1",
            "cha",
            "ult",
            "a2",
            "blo",
            "dmg",
            "sup"
        };
        private static Base64Converter converter = new Base64Converter();

        static void Main(string[] args)
        {
            // settings
            const int maxids = 40;
            const int fids = 40;
            const int startid = 0;
            const int starfid = 0;

            List<DataModel> datalist = new List<DataModel>();

            for (int i = startid; i < maxids; i++)
                for (int j = starfid; j < fids; j++)
                    datalist = extractorMain(i, j, datalist);

            Console.WriteLine("Saving data...");

            SaveAsJason(datalist);

            Console.WriteLine("Saving finish");
            Console.ReadLine();
        }

        private static List<DataModel> extractorMain(int id, int fid, List<DataModel> datalist)
        {
            DataModel datamodel = new DataModel();
            datamodel.animationsets = new List<AnimationSet>();

            foreach (string animationType in animationTypes)
            {
                List<operatingImageModel> operationData = readAndGetFramesFromOperations(converter, animationType, id, fid);

                foreach (operatingImageModel model in operationData)
                {
                    Console.WriteLine("# of Frames: " + model.frames.Length);

                    #region file parts
                    string[] parts = model.fileName.Split("_");
                    int UID = Convert.ToInt16(parts[0]);
                    int FID = Convert.ToInt16(parts[1]);
                    int AnniID = Convert.ToInt16(parts[3]);
                    #endregion

                    datamodel.formID = FID;
                    datamodel.id = UID;

                    List<string> spriteSheet = new List<string>();
                    for (int i = 0; i < model.frames.Length; i++)
                    {
                        spriteSheet.Add(converter.ConvertBitmapToBase64(model.frames[i]));
                    }

                    #region create new AnimationSet and set properties
                    AnimationSet set = new AnimationSet()
                    {
                        animationType = animationType,
                        spriteSheet = spriteSheet,
                        properties = new FrameProperties(
                        model.fileName,
                        model.height,
                        model.widht),
                        animationTypeID = AnniID
                    };
                    #endregion

                    datamodel.animationsets.Add(set);
                }
            }
            if(datamodel.animationsets.Count > 0)
                datalist.Add(datamodel);

            return datalist;  
        }
        
        private static void SaveAsJason(List<DataModel> datalist)
        {
            string json = JsonSerializer.Serialize(datalist);
            File.WriteAllText(@"results/results.json", json);
        }

        private static List<operatingImageModel> readAndGetFramesFromOperations(Base64Converter converter, string animationType, int id, int fid)
        {
            string path = "./operations";
            path = Path.GetFullPath(path);
            List<string> files = getFilesFromOperationDirectory(path);
            List<operatingImageModel> data = new List<operatingImageModel>();

            foreach (string file in files)
            {
                string[] parts = Path.GetFileName(file).Split('_');
                bool enemySite = Convert.ToBoolean(Convert.ToInt16(parts[4].Split('.')[0]));
                int ID = Convert.ToInt16(parts[0]);
                int FID = Convert.ToInt16(parts[1]);
                string AnimationType = parts[2];

                if (AnimationType == animationType && !enemySite && ID == id && FID == fid)
                {
                    int height;
                    int width;

                    using (var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        using (var image = Image.FromStream(fileStream, false, false))
                        {
                            height = image.Height;
                            width = image.Width;
                        }
                    }

                    Console.WriteLine("Extracting: " + file);

                    data.Add(new operatingImageModel(
                         converter.extractFramesFromGif(Image.FromFile(file)),
                         Path.GetFileName(file),
                         height,
                         width));
                }
            }

            return data;
        }

        private static List<string> getFilesFromOperationDirectory(string path)
        {
            if (Directory.Exists(path))
            {
                return Directory.GetFiles(path).ToList();
            }
            else
                return new List<string>();
        }
    }
}
