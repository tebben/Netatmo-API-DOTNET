namespace Netatmo.Net
{
    public static class Urls
    {
        public static string RequestTokenUrl => $"{UrlBase}{UrlRequestTokenPath}";
        public static string GetStationsDataUrl => $"{UrlBase}{UrlGetStationsData}";
        public static string GetMeasureUrl => $"{UrlBase}{UrlGetMeasuree}";
        public static string GetPublicDataUrl => $"{UrlBase}{UrlGetPublicData}";

        public static string UrlBase = "https://api.netatmo.com";
        public static string UrlRequestTokenPath = "/oauth2/token";
        public static string UrlGetStationsData = "/api/getstationsdata";
        public static string UrlGetMeasuree = "/api/getmeasure";
        public static string UrlGetPublicData = "/api/getpublicdata";
    }
}
