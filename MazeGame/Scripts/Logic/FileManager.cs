public static class FileManager
{
    public static string FindPathToFolder(string folderName)
    {
        string currentPath = Directory.GetCurrentDirectory();
        DirectoryInfo directory = new(currentPath);

        while (directory != null && directory.Name != folderName)
        {
            directory = directory.Parent;
        }

        return directory?.FullName;
    }
}
