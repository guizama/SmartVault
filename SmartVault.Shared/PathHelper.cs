namespace SmartVault.Shared
{
    public static class PathHelper
    {
        public static string GetSharedFolder()
        {
            var root = Path.GetFullPath(
                Path.Combine(AppContext.BaseDirectory, "..", "..", "..", ".."));

            var path = Path.Combine(root, "SharedFiles");

            Directory.CreateDirectory(path);

            return path;
        }
    }
}