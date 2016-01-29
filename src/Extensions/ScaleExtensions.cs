using System;
using Netatmo.Net.Model;

namespace Netatmo.Net.Extensions
{
    public static class ScaleExtensions
    {
        public static string GetScaleName(this Scale scale)
        {
            switch (scale)
            {
                case Scale.Max:
                    return "max";
                case Scale.OneDay:
                    return "30min";
                case Scale.OneHour:
                    return "1hour";
                case Scale.OneMonth:
                    return "3hours";
                case Scale.OneWeek:
                    return "1week";
                case Scale.ThirtyMinutes:
                    return "30min";
                case Scale.ThreeHours:
                    return "3hours";
                default:
                    throw new ArgumentOutOfRangeException(nameof(scale), scale, null);
            }
        }
    }
}
