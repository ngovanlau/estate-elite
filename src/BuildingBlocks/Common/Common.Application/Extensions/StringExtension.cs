using System.Text;

namespace Common.Application.Extensions;

public static class StringExtension
{
    private static readonly Random random = new Random();
    private const string Characters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

    public static string GenerateHashId(int length)
    {
        var stringBuilder = new StringBuilder(length);

        for (int i = 0; i < length; i++)
        {
            int randomIndex = random.Next(Characters.Length);
            stringBuilder.Append(Characters[randomIndex]);
        }

        return stringBuilder.ToString();
    }
}
