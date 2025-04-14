namespace SharedKernel.Constants;

public static class ErrorCode
{
    public const string E000 = "Request error";
    public const string E001 = "{0} not empty or whitespace";
    public const string E002 = "{0} not null";
    public const string E003 = "Invalid email";
    public const string E004 = "{0} must be at least {1} characters long";
    public const string E005 = "{0} must be at most {1} characters long";
    public const string E006 = "Password must contain at least 1 uppercase, 1 lowercase, 1 digit, and 1 special character";
    public const string E007 = "Username must allows: letters (a-z, A-Z), digits (0-9), underscores (_)";
    public const string E008 = "{0} not found";
    public const string E009 = "File size must not exceed {0}";
    public const string E010 = "Only image files are allowed (JPG, PNG, GIF, JPEG)";

    // Identity service
    public const string E101 = "Username already exists";
    public const string E102 = "Email already exists";
    public const string E103 = "User not found"; //TODO
    public const string E104 = "OTP code has expired";
    public const string E105 = "OTP code is invalid, you have {0} attempts remaining";
    public const string E106 = "User is already active";
    public const string E107 = "Active user failed";
    public const string E108 = "You have reached the maximum number of attempts";
    public const string E109 = "User already exists";
    public const string E110 = "The confirmation password does not match the password";
    public const string E111 = "Username not found"; //TODO
    public const string E112 = "Email not found"; //TODO
    public const string E113 = "Either Username or Email must be provided";
    public const string E114 = "Incorrect password";
    public const string E115 = "User is already active or not found";
    public const string E116 = "OTP code is still valid";
    public const string E117 = "Access token is invalid ";
    public const string E118 = "Refresh token is invalid or expired";
}
