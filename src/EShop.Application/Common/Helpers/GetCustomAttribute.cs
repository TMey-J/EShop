using System.ComponentModel;
using System.Reflection;

namespace Blogger.Application.Common.Helpers;

public static class GetCustomAttribute
{
    public static string? GetDisplayName<TObj>(string propertyName) where TObj : class
    {
        var property=typeof(TObj).GetProperty(propertyName);
        var displayName = property?.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName;
        return displayName;
    }
}
