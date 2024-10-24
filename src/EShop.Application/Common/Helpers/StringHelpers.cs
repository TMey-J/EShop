using Blog.Core.Application.Constants.Common;
using System.Text.RegularExpressions;

namespace EShop.Application.Common.Helpers
{
    public static partial class StringHelpers
    {
        [GeneratedRegex(RegularExperssions.Email)]
        private static partial Regex Email();
        public static bool IsEmail(this string value)
        {
            var match = Email().Match(value);
            return match.Success;
        }

        public static string GenerateUniqueName()
        {
            return Guid.NewGuid().ToString("N");
        }

        
    }
}
