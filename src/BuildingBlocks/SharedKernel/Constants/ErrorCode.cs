namespace SharedKernel.Constants;

public static class ErrorCode
{
    public const string E000 = "Request error";

    // Identity service
    public const string E101 = "Username already exists";
    public const string E102 = "Email already exists";
    public const string E103 = "User not found";
    public const string E104 = "OTP code has expired";
    public const string E105 = "OTP code is invalid, you have {0} attempts remaining";
    public const string E106 = "User is already active";
    public const string E107 = "Active user failed";
    public const string E108 = "You have reached the maximum number of attempts";
    public const string E109 = "User already exists";
}
