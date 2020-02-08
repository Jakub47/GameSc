﻿

using Microsoft.ML.Data;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Thesis.Helpers.AI.ObjectDetection.DataStructures
{
    public class ImageNetData
    {
        [LoadColumn(0)]
        public string ImagePath;

        [LoadColumn(1)]
        public string Label;

        public static IEnumerable<ImageNetData> ReadFromFile(string imageFolder)
        {
            return Directory
                .GetFiles(imageFolder)
                .Where(filePath => Path.GetExtension(filePath) == ".png" || Path.GetExtension(filePath) == ".jpg")
                .Select(filePath => new ImageNetData { ImagePath = filePath, Label = Path.GetFileName(filePath) });
        }

        public static IEnumerable<ImageNetData> ReadFrom1File(string pathToFile)
        {
            var list = new List<ImageNetData>();
            list.Add(new ImageNetData { ImagePath = pathToFile, Label = Path.GetFileName(pathToFile) });
            return list;
        }
    }
}