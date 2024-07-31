using OpenCvSharp;

using System.Xml.Linq;

var folderA = @"J:\boreas\boreas-2021-07-27-14-43\camera";
var folderB = @"J:\cloud-point-maped\black-background";
var folderAB = "./result5";
var filesB = Directory.EnumerateFiles(folderB, "*.jpg", SearchOption.AllDirectories)
                    .Select(c => Path.GetFileNameWithoutExtension(new FileInfo(c).Name))
                    .ToArray();
var filesA = Directory.EnumerateFiles(folderA, "*.png", SearchOption.AllDirectories)
                    .Select(c => new FileInfo(c))
                    .Where(c=> filesB.Contains(Path.GetFileNameWithoutExtension(c.Name)))
                    .ToArray();
var filesBFull = Directory.EnumerateFiles(folderB, "*.jpg", SearchOption.AllDirectories)
                    .Select(c => { return new { name = Path.GetFileNameWithoutExtension(new FileInfo(c).Name), file = new FileInfo(c) }; })
                    .ToArray();

var dirMasks = Directory.CreateDirectory(folderAB + "/train");
var resizeFile = new Size(1888, 1576);
Parallel.ForEach(filesA,new ParallelOptions { MaxDegreeOfParallelism = 5 }, file =>
{
    if (!filesB.Contains(Path.GetFileNameWithoutExtension(file.Name)))
        return;
    using var imageA = Cv2.ImRead(file.FullName, ImreadModes.Unchanged);

    using var imageB = Cv2.ImRead(filesBFull.First(c => c.name == Path.GetFileNameWithoutExtension(file.Name)).file.FullName, ImreadModes.Unchanged);
    using var cropedImage = new Mat();
    Cv2.Resize(imageA, cropedImage, resizeFile);

    if (imageA.Channels() != imageB.Channels() || imageA.Dims != imageB.Dims)
    {
        Console.WriteLine("image A:" + imageA.Channels());
        Console.WriteLine("image B:" + imageB.Channels());
        Console.WriteLine("Image A" + imageA.Dims);
        Console.WriteLine("Image B" + imageB.Dims);
    }
    try
    {

        using var mergedImage = new Mat();
        Cv2.HConcat(cropedImage, imageB, mergedImage);
        mergedImage.ImWrite(Path.Combine(dirMasks.FullName, Path.GetFileNameWithoutExtension(file.Name) + ".jpg"));
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        Console.WriteLine(file.Name);
    }

});