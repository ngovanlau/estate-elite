namespace SharedKernel.Constants;

public static class RegexPatterns
{
    // Password: At least 1 uppercase, 1 lowercase, 1 digit, 1 special char
    public const string Password = @"^(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9])(?=.*[\W_]).+$";
}
