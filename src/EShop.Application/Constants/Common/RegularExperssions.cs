namespace Blog.Core.Application.Constants.Common;

public static class RegularExperssions
{
    public const string EmailOrPhoneNumber = @"^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})|(\+98|0)?9\d{9}$";
    public const string Email = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";

}
