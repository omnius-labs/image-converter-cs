using System.Globalization;
using ImageMagick;

namespace ImageConverter;

public static class Program
{
    public static int Main(string[] args)
    {
        try
        {
            var input = args[0];
            var output = args[1];

            var m = new MagickImage(input, GetMagickFormat(input));
            m.Write(output, GetMagickFormat(output));

            Console.WriteLine($"Converted {input} to {output}");
            return 0;
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
