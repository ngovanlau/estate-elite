namespace IdentityService.Infrastructure.Utilities;

using IdentityService.Application.Interfaces;

public class ConfirmationCodeGenerator : IConfirmationCodeGenerator
{
    private readonly Random _random = new Random();

    public string GenerateCode()
    {
        var allowedChars = "0123456789";
        var length = 6;

        var code = new char[length];

        for (int i = 0; i < length; i++)
        {
            code[i] = allowedChars[_random.Next(0, allowedChars.Length)];
        }

        return new string(code);
    }
}
