using OpenCvSharp;

using System;

var folderA = @"E:\Work\University\sensor\datasets\IJRR-Dataset-1-subset\IMAGES\Cam0";
var folderB = @"E:\Work\University\sensor\win\Tools_Merge_Image_PointCloud\result\img";
var folderAB = "./result";
var files = Directory.EnumerateFiles(folderA, "*.*", SearchOption.AllDirectories)
                    .Where(file => (new string[] { ".ppm" }).Contains(Path.GetExtension(file), StringComparer.OrdinalIgnoreCase))
                    .Select(c => new FileInfo(c))
                    .ToArray();
var dirMasks = Directory.CreateDirectory(folderAB + "/train");
Parallel.ForEach(files, file =>
{
    var imageppm = Cv2.ImRead(file.FullName, ImreadModes.Unchanged);
    imageppm.SaveImage(Path.Combine(dirMasks.FullName, file.Name.Split('.')[0]+".jpg"));
});