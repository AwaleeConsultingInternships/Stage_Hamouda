namespace Utilities
{
    public class Directories
    {
        private static DirectoryInfo? GetProjectDirectory()
        {
            // Create relative path
            string exeDirectory = AppDomain.CurrentDomain.BaseDirectory;
            return Directory.GetParent(exeDirectory)?.Parent?.Parent?.Parent?.Parent;
        }

        private static string GetMarketDataDirectory()
        {
            var projectDirectory = GetProjectDirectory();
            return projectDirectory?.FullName + "\\MarketData\\";
        }

        public static string GetJsonContent()
        {
            var marketDataDirectory = GetMarketDataDirectory();
            var filePath = Path.Combine(marketDataDirectory, "MarketData.json");

            return File.ReadAllText(filePath);
        }

        public static string GetGraphDirectory()
        {
            var projectDirectory = GetProjectDirectory();
            var folderPath = projectDirectory?.FullName + "\\Graph\\";
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            return folderPath;
        }
    }
}
