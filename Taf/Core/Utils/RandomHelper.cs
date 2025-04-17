using System.Text;

namespace Taf.Core.Utils;
public class RandomHelper
{
    private static readonly Random Rnd = new();
    private static readonly string Numbers = "0123456789";

    public static string NumericString(int size)
    {
        var builder = new StringBuilder();
        var length = Numbers.Length;

        for (var i = 0; i < size; i++)
        {
            builder.Append(Numbers[Rnd.Next(0, length)]);
        }

        return builder.ToString();
    }
}
