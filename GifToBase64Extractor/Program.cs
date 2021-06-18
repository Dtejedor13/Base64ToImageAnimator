using Base64ConverterCore.Models;
using Base64ToImageAnimator.Converter;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.Json;

namespace GifToBase64Extractor
{
    class Program
    {
        static void Main(string[] args)
        {
            List<DataModel> datalist = new List<DataModel>();
            Base64Converter converter = new Base64Converter();
            operatingImageModel data = readAndGetFramesFromOperations(converter);

            Console.WriteLine("# of Frames: " + data.frames.Length);

            // converting all frames to base64
            datalist.Add(new DataModel());
            datalist[0].bs_props = new FrameProperties(
                data.fileName,
                data.height,
                data.widht);
            datalist[0].bs = new List<string>();

            List<string> spriteSheet = new List<string>();
            for (int i = 0; i < data.frames.Length; i++)
            {
                spriteSheet.Add(converter.ConvertBitmapToBase64(data.frames[i]));
            }
            datalist[0].bs = spriteSheet;

            Console.WriteLine("Saving data...");

            SaveAsJason(datalist);

            Console.WriteLine("Saving finish");
            Console.ReadLine();
        }
        
        private static void SaveAsJason(List<DataModel> datalist)
        {
            string json = JsonSerializer.Serialize(datalist);
            File.WriteAllText(@"results/results.json", json);
        }

        private static operatingImageModel readAndGetFramesFromOperations(Base64Converter converter)
        {
            string path = "./operations/00_2_bs_1_1.gif";
            path = Path.GetFullPath(path);
            int height;
            int width;

            using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (var image = Image.FromStream(fileStream, false, false))
                {
                    height = image.Height;
                    width = image.Width;
                }
            }

            Console.WriteLine("Extracting: " + path);

            return new operatingImageModel(
                converter.extractFramesFromGif(Image.FromFile(path)),
                Path.GetFileName(path),
                height,
                width);
        }
    }
}
