namespace IdentityService.Infrastructure.Utilities;

using Application.Interfaces;
using SharedKernel.Constants;

public class ConfirmationCodeGenerator(ConfirmationCodeSetting setting) : IConfirmationCodeGenerator
{
    private readonly Random _random = new Random();

    public string GenerateCode()
    {
        var allowedChars = setting.AllowedChars;
        var length = setting.CodeLength;

        var code = new char[length];

        for (int i = 0; i < length; i++)
        {
            code[i] = allowedChars[_random.Next(0, allowedChars.Length)];
        }

        return new string(code);
    }
}
