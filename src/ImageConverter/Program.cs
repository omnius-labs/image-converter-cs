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

            var json = Base64StringToUtf8String(line);
            if (json is null) throw new Exception("failed to base64 decoding");

            var option = ConverterOption.FromJson(json);
            if (option is null) throw new Exception("failed to json deserializing");

            try
            {
                Converter.Run(option);
            }
            catch (Exception)
            {
                Console.Error.WriteLine($"Option: \"{json}\"");
                throw;
            }

            Console.WriteLine($"Converted \"{option.InPath}\" to \"{option.OutPath}\"");

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
