// See https://aka.ms/new-console-template for more information
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

using Spectre.Console;
using Spectre.Console.Cli;

using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

var app = new CommandApp<GenerateFoldersCommand>();
return app.Run(args);

internal sealed class GenerateFoldersCommand : Command<GenerateFoldersCommand.Settings>
{
    public override int Execute(CommandContext context, [NotNull] Settings settings)
    {
        var files = Directory.EnumerateFiles(settings.InputFolder, "*.*", SearchOption.AllDirectories)
                    .Where(file => settings.AllowedExtensions.Contains(Path.GetExtension(file), StringComparer.OrdinalIgnoreCase))
                    .ToArray();
        var dirTrainA = Directory.CreateDirectory(settings.OutputDir + "/train" + "A");
        var dirTrainB = Directory.CreateDirectory(settings.OutputDir + "/train" + "B");
        var dirValA = Directory.CreateDirectory(settings.OutputDir + "/val" + "A");
        var dirValB = Directory.CreateDirectory(settings.OutputDir + "/val" + "B");
        var dirTestA = Directory.CreateDirectory(settings.OutputDir + "/test" + "A");
        var dirTestB = Directory.CreateDirectory(settings.OutputDir + "/test" + "B");
        var savePathA = string.Empty;
        var savePathB = string.Empty;
        // Synchronous
        AnsiConsole.Status()
            .Start("Thinking...", ctx =>
            {
                // Simulate some work
                ctx.Status("Genetate train set.");
                ctx.Spinner(Spinner.Known.SquareCorners);
                ctx.SpinnerStyle(Style.Parse("yellow"));
                Parallel.ForEach(files, file =>
                {
                    var fileInfo = new FileInfo(file);
                    var fileName = fileInfo.Name.Split('.')[0];
                    var extention = fileInfo.Name.Split('.')[1];
                    using var image = Image.Load(fileInfo.FullName);
                    using var imageb = image.Clone(c => { });
                    image.Mutate(x => x.Crop(new Rectangle(0, 0, image.Width / 2, image.Height)));
                    imageb.Mutate(x => x.Crop(new Rectangle(imageb.Width / 2, 0, imageb.Width / 2, imageb.Height)));

                    switch (WhichCatalog(settings.TestChange, settings.ValidatorChange))
                    {
                        case Catalog.Test:
                            savePathA = dirTestA.FullName;
                            savePathB = dirTestB.FullName;
                            break;
                        case Catalog.Val:
                            savePathA = dirValA.FullName; savePathB = dirValB.FullName;
                            break;
                        case Catalog.Train:
                            savePathA = dirTrainA.FullName;
                            savePathB = dirTrainB.FullName;
                            break;
                        default:
                            break;
                    }
                    image.Save(Path.Combine(savePathA, fileName + "_A." + extention));
                    imageb.Save(Path.Combine(savePathB, fileName + "_B." + extention));
                });
                AnsiConsole.WriteLine($"Output: {dirTrainA.FullName}");
                AnsiConsole.WriteLine($"Output: {dirTrainB.FullName}");
            });
        AnsiConsole.MarkupLine("[green]Done![/]");
        AnsiConsole.WriteLine("Have nice day");

        return 0;
    }
    private Catalog WhichCatalog(float test, float val)
    {
        var chance = (float)Random.Shared.Next(100);
        if (chance < test)
            return Catalog.Test;
        chance-=test;
        if(chance < val) return Catalog.Val;
        return Catalog.Train;
    }

    public sealed class Settings : CommandSettings
    {
        [Description("The folder contains all data.")]
        [CommandOption("-i|--input")]
        public string InputFolder { get; init; }
        [Description("The output folder path. system create all folder inside automaticlly.")]
        [CommandOption("-o|--output")]
        public string OutputDir { get; init; } = "./result";
        [Description("The chance of an image go to validator set.")]
        [CommandOption("-v|--vali")]
        public float ValidatorChange { get; init; } = 10;
        [Description("The chance of an image go to test set.")]
        [CommandOption("-t|--test")]
        public float TestChange { get; init; } = 10;
        [Description("Allowed Extensions")]
        [CommandOption("-a|--allow")]
        public string[] AllowedExtensions { get; init; } = { ".png", ".jpg" };
    }
    enum Catalog
    {
        Test,
        Val,
        Train
    }
}