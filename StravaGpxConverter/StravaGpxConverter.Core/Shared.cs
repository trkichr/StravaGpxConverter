using System.Configuration;

namespace StravaGpxConverter.Core
{
    public static class Shared
    {
        public static bool IsFake { get; } =
            ConfigurationManager.AppSettings["IsFake"] == "1";
        public static string FakePath { get; } =
            ConfigurationManager.AppSettings["FakePath"];
    }
}
