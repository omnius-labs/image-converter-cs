using System.Text.Json;
using System.Text.Json.Serialization;
using ImageMagick;

namespace ImageConverter.Internal;

[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.SnakeCaseLower)]
[JsonSerializable(typeof(ConverterOption))]
internal partial class SourceGenerationContext : JsonSerializerContext
{
}

public class ConverterOption
{
    public required string InPath { get; init; }
    public required string InType { get; init; }
    public required string OutPath { get; init; }
    public required string OutType { get; init; }

    public static ConverterOption? FromJson(string text)
    {
        return (ConverterOption?)JsonSerializer.Deserialize(text, typeof(ConverterOption), SourceGenerationContext.Default);
    }

    public string ToJson()
    {
        return JsonSerializer.Serialize(this, typeof(ConverterOption), SourceGenerationContext.Default);
    }
}

public static class Converter
{
    static Converter()
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

    public static void Run(ConverterOption option)
    {
        var inputFormat = GetMagickFormat(option.InType);
        var outputFormat = GetMagickFormat(option.OutType);

        if (outputFormat == MagickFormat.Unknown) throw new Exception("output format is unknown");

        using var m = new MagickImage(option.InPath, inputFormat);
        m.Write(option.OutPath, outputFormat);
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
