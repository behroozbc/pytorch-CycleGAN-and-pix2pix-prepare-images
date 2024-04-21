// See https://aka.ms/new-console-template for more information
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

using Spectre.Console;
using Spectre.Console.Cli;

using System.ComponentModel;
using System.IO;

var app = new CommandApp<GenerateFoldersCommand>();
return app.Run(args);
internal sealed class GenerateFoldersCommand : Command<GenerateFoldersCommand.Settings>
{
    public override int Execute(CommandContext context, Settings settings)
    {
        var files = Directory.EnumerateFiles(settings.InputFolder, "*.*", SearchOption.AllDirectories)
                    .Where(file => settings.AllowedExtensions.Contains(Path.GetExtension(file), StringComparer.OrdinalIgnoreCase))
                    .ToArray();
        AnsiConsole.Status().Start("Thinking...", ctx =>
        {
            ctx.Status("Genetate train set.");
            ctx.Spinner(Spinner.Known.SquareCorners);
            ctx.SpinnerStyle(Style.Parse("yellow"));
            var dirImgs = Directory.CreateDirectory(settings.OutputDir + "/imgs");
            var dirMasks = Directory.CreateDirectory(settings.OutputDir + "/masks");
            Parallel.ForEach(files, file =>
            {
                var fileInfo = new FileInfo(file);
                using var imag = Image.Load(fileInfo.FullName);
                using var mask = imag.Clone(c => { });
                imag.Mutate(x => x.Crop(new Rectangle(0, 0, imag.Width / 2, imag.Height)));
                mask.Mutate(x => x.Crop(new Rectangle(mask.Width / 2, 0, mask.Width / 2, mask.Height)));
                imag.Save(Path.Combine(dirImgs.FullName, fileInfo.Name));
                mask.Save(Path.Combine(dirImgs.FullName, fileInfo.Name));
            });
            AnsiConsole.WriteLine($"Output: {dirImgs.FullName}");
            AnsiConsole.WriteLine($"Output: {dirMasks.FullName}");
        });
        AnsiConsole.MarkupLine("[green]Done![/]");
        AnsiConsole.WriteLine("Have nice day");
        return 0;
    }

    public sealed class Settings : CommandSettings
    {
        [Description("The folder contains all data.")]
        [CommandOption("-i|--input")]
        public string InputFolder { get; init; }
        [Description("The output folder path. system create all folder inside automaticlly.")]
        [CommandOption("-o|--output")]
        public string OutputDir { get; init; } = "./result";
        [Description("Allowed Extensions")]
        [CommandOption("-a|--allow")]
        public string[] AllowedExtensions { get; init; } = { ".png", ".jpg" };
    }
}