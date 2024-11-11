using System.Reflection;

namespace EShop.Application.Common.Helpers;

public static class GetCustomAttribute
{
    public static string? GetDisplayName<TObj>(string propertyName) where TObj : class
    {
        PropertyInfo? property = null;
        var spllitedProrpery = propertyName.Split('.');
        if (spllitedProrpery.Length > 0)
        {
            var type = typeof(TObj);
            for(var i = 0; i < spllitedProrpery.Length-1; i++)
            {
                type= type?.GetProperty(spllitedProrpery[i])?.PropertyType;
            }
            property= type?.GetProperty(spllitedProrpery[^1]);
        }
        else
        { 
            property=typeof(TObj).GetProperty(propertyName);
        }
        var displayName = property?.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName;
        return displayName;
    }
}
