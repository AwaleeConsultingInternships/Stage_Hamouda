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

        public static string GetMarketDataDirectory()
        {
            var projectDirectory = GetProjectDirectory();
            return projectDirectory?.FullName + "\\MarketData\\";
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
