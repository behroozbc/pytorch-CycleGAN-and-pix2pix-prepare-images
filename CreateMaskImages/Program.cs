// See https://aka.ms/new-console-template for more information
using OpenCvSharp;

var loadindatset = "boreas-2021-11-14-09-47";
var images = @"J:\savedatasets\black-background-gray";
var resultSavingPath = ".\\result";
var filesB = Directory.EnumerateFiles(Path.Combine(images,loadindatset), "*.jpg", SearchOption.AllDirectories)
                    .Select(c => new FileInfo(c).Name)
                    .ToArray();
var dirMasks = Directory.CreateDirectory(Path.Combine(resultSavingPath,loadindatset));
var resizeFile = new Size(1888, 1576);
Parallel.ForEach(filesB, file =>
{
    using var img = new Mat(resizeFile, MatType.CV_8SC1, s: new Scalar(250, 0, 0));
    img.SaveImage(Path.Combine(dirMasks.FullName, file));
});

