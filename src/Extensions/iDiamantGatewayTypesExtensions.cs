using System.Linq;
using Netatmo.Net.Model;

namespace Netatmo.Net.Extensions
{
    public static class iDiamantGatewayTypesExtensions
    {
        public static string ToGatewayString(this iDiamantGatewayTypes[] gatewaytypes)
        {
            var iDiamantGatewayTypesString = gatewaytypes.Aggregate("", (current, iDiamantGatewayTypes) => string.IsNullOrEmpty(current) ? current +  $"{iDiamantGatewayTypes}" : current + $" {iDiamantGatewayTypes}");
            return iDiamantGatewayTypesString;
        }
    }
}
