using System.Globalization;
using CommandLine;
using ImageMagick;

namespace ImageConverter;

class Options
{
    [Option('i', "in", Required = true)]
    public string Input { get; set; } = "";

    [Option('o', "out", Required = true)]
    public string Output { get; set; } = "";
}

public static class Program
{
    public static int Main(string[] args)
    {
        try
        {
            var parseResult = Parser.Default.ParseArguments<Options>(args);
            if (parseResult.Tag == ParserResultType.NotParsed) throw new Exception("Invalid arguments");

            if (parseResult is Parsed<Options> parsed)
            {
                var option = parsed.Value;

                var m = new MagickImage(option.Input, GetMagickFormat(option.Input));
                m.Write(option.Output, GetMagickFormat(option.Output));

                Console.WriteLine($"Converted {option.Input} to {option.Output}");
                return 0;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error: {e.Message}");
        }

        return -1;
    }

    static MagickFormat GetMagickFormat(string path)
    {
        var ext = Path.GetExtension(path).ToLower(CultureInfo.InvariantCulture);
        return ext switch
        {
            ".jpg" => MagickFormat.Jpg,
            ".png" => MagickFormat.Png,
            ".gif" => MagickFormat.Gif,
            ".bmp" => MagickFormat.Bmp,
            ".avif" => MagickFormat.Avif,
            ".heif" => MagickFormat.Heif,
            ".heic" => MagickFormat.Heic,
            _ => throw new Exception("Invalid format")
        };
    }
}
