using System.Linq;

namespace Netatmo.Net.Extensions
{
    public static class NetatmoScopeExtensions
    {
        public static string ToScopeString(this NetatmoScope[] scopes)
        {
            var scopeString = scopes.Aggregate("", (current, netatmoScope) => current + $"{netatmoScope} ");
            scopeString = scopeString.TrimEnd();
            return scopeString;
        }
    }
}
