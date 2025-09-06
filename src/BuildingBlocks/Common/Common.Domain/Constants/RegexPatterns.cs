namespace Common.Domain.Constants;

public static class RegexPatterns
{
    // Requires: 1 uppercase, 1 lowercase, 1 digit, 1 special char
    public const string Password = @"^(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9])(?=.*[\W_]).+$";

    // Allows: letters (a-z, A-Z), digits (0-9), underscores (_)
    public const string Username = @"^[a-zA-Z0-9_]+$";

    public const string Phone = @"^(0|\+84)\d{9,10}$";
}
