using System.Text;
using ImageConverter.Internal;

namespace ImageConverter;

public static class Program
{
    public static int Main(string[] args)
    {
        try
        {
            var line = Console.ReadLine();
            if (line is null) throw new Exception("stdin is null");

            var text = Base64StringToUtf8String(line);
            if (text is null) throw new Exception("failed to base64 decoding");

            var option = ConverterOption.FromJson(text);
            if (option is null) throw new Exception("failed to json deserializing");

            Converter.Run(option);

            Console.WriteLine($"Converted \"{option.Input.FilePath}\" to \"{option.Output.FilePath}\"");

            return 0;
        }
        catch (Exception e)
        {
            Console.Error.WriteLine($"Error: {e.Message}");
        }

        return -1;
    }

    public static string Base64StringToUtf8String(string text)
    {
        var base64 = text
            .Replace("-", "+", StringComparison.InvariantCulture)
            .Replace("_", "/", StringComparison.InvariantCulture);
        var paddingNeeded = 4 - base64.Length % 4;

        if (paddingNeeded < 4)
        {
            base64 += new string('=', paddingNeeded);
        }

        var utf8 = new UTF8Encoding(false);
        return utf8.GetString(Convert.FromBase64String(base64));
    }
}
