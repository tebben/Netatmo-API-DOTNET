using System.Linq;
using Netatmo.Net.Model;

namespace Netatmo.Net.Extensions
{
    public static class MeasurementTypeExtensions
    {
        public static string ToMeasurementTypesString(this MeasurementType[] types)
        {
            return types.Aggregate("", (current, measurementType) => current + (string.IsNullOrEmpty(current) ? measurementType.ToString() : $",{measurementType}"));
        }
    }
}
