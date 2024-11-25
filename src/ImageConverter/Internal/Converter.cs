using System.Globalization;
using System.Text.Json;
using ImageMagick;

namespace ImageConverter.Internal;

public class ConverterOption
{
    public required InputOption Input { get; init; }
    public required OutputOption Output { get; init; }

    public static ConverterOption? FromJson(string text)
    {
        var options = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = new SnakeCaseJsonNamingPolicy()
        };
        return JsonSerializer.Deserialize<ConverterOption>(text, options);
    }

    public string ToJson()
    {
        var options = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = new SnakeCaseJsonNamingPolicy()
        };
        return JsonSerializer.Serialize(this, options);
    }
}

public class InputOption
{
    public required string? Format { get; init; }
    public required string FilePath { get; init; }
}
public class OutputOption
{
    public required string Format { get; init; }
    public required string FilePath { get; init; }
}

public static class Converter
{
    public static void Run(ConverterOption option)
    {
        var inputFormat = GetMagickFormat(option.Input.Format);
        var outputFormat = GetMagickFormat(option.Output.Format);

        if (outputFormat == MagickFormat.Unknown) throw new Exception("output format is unknown");

        using var m = new MagickImage(option.Input.FilePath, inputFormat);
        m.Write(option.Output.FilePath, outputFormat);
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

    private static MagickFormat GetMagickFormat(string? value)
    {
        return value switch
        {
            "gif" => MagickFormat.Gif,
            "jpg" => MagickFormat.Jpg,
            "png" => MagickFormat.Png,
            "webp" => MagickFormat.WebP,
            "bmp" => MagickFormat.Bmp,
            "heif" => MagickFormat.Heif,
            "heic" => MagickFormat.Heic,
            "avif" => MagickFormat.Avif,
            "svg" => MagickFormat.Svg,
            _ => MagickFormat.Unknown
        };
    }
}
