using System.Globalization;
using ImageMagick;

namespace ImageConverter;

public static class Program
{
    public static int Main(string[] args)
    {
        try
        {
            InitializeImageMagick();

            var input = args[0];
            var output = args[1];

            var m = new MagickImage(input, MagickFormat.Unknown);
            m.Write(output, GetMagickFormat(output));

            Console.WriteLine($"Converted {input} to {output}");
            return 0;
        }
        catch (Exception e)
        {
            Console.Error.WriteLine($"Error: {e.Message}");
        }

        return -1;
    }

    private static void InitializeImageMagick()
    {
        var configFiles = ImageMagick.Configuration.ConfigurationFiles.Default;
        configFiles.Policy.Data = @"
<policymap>
  <policy domain=""delegate"" rights=""none"" pattern=""*"" />
  <policy domain=""filter"" rights=""none"" pattern=""*"" />
  <policy domain=""coder"" rights=""none"" pattern=""*"" />
  <policy domain=""coder"" rights=""read|write"" pattern=""{GIF,JPEG,PNG,WEBP,BMP,HEIF,HEIC,AVIF,SVG}"" />
</policymap>";
        MagickNET.Initialize(configFiles);
    }

    static MagickFormat GetMagickFormat(string path)
    {
        var ext = Path.GetExtension(path).ToLower(CultureInfo.InvariantCulture);
        return ext switch
        {
            ".gif" => MagickFormat.Gif,
            ".jpg" => MagickFormat.Jpg,
            ".png" => MagickFormat.Png,
            ".webp" => MagickFormat.WebP,
            ".bmp" => MagickFormat.Bmp,
            ".heif" => MagickFormat.Heif,
            ".heic" => MagickFormat.Heic,
            ".avif" => MagickFormat.Avif,
            ".svg" => MagickFormat.Svg,
            _ => throw new Exception("Invalid format")
        };
    }
}
