namespace Netatmo.Net
{
    public static class Urls
    {
        public static string RequestAuthorizationUrl => $"{UrlBase}{UrlAuthorization}";
        public static string RequestTokenUrl => $"{UrlBase}{UrlRequestTokenPath}";
        public static string GetStationsDataUrl => $"{UrlBase}{UrlGetStationsData}";
        public static string GetMeasureUrl => $"{UrlBase}{UrlGetMeasuree}";
        public static string GetPublicDataUrl => $"{UrlBase}{UrlGetPublicData}";

        public static string UrlBase = "https://api.netatmo.com";
        public static string UrlAuthorization = "/oauth2/authorize";
        public static string UrlRequestTokenPath = "/oauth2/token";
        public static string UrlGetStationsData = "/api/getstationsdata";
        public static string UrlGetMeasuree = "/api/getmeasure";
        public static string UrlGetPublicData = "/api/getpublicdata";

        /* iDiamant */
        public static string GetHomesDataUrl => $"{UrlBase}{UrlGetHomesData}";
        public static string GetHomeStatusDataUrl => $"{UrlBase}{UrlGetHomeStatusData}";
        public static string SetHomeSetShutterUrl => $"{UrlBase}{UrlSetShutterData}";

        public static string UrlGetHomesData = "/api/homesdata";
        public static string UrlGetHomeStatusData = "/api/homestatus?home_id=%id%";
        public static string UrlSetShutterData = "/api/setstate";
    }
}
