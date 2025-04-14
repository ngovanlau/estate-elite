namespace IdentityService.Infrastructure.Utilities;

using Application.Interfaces;
using Microsoft.Extensions.Options;
using SharedKernel.Settings;

public class ConfirmationCodeGenerator(IOptions<ConfirmationCodeSetting> options) : IConfirmationCodeGenerator
{
    private ConfirmationCodeSetting _setting = options.Value;
    private readonly Random _random = new Random();

    public string GenerateCode()
    {
        var allowedChars = _setting.AllowedChars;
        var length = _setting.CodeLength;

        var code = new char[length];

        for (int i = 0; i < length; i++)
        {
            code[i] = allowedChars[_random.Next(0, allowedChars.Length)];
        }

        return new string(code);
    }
}
