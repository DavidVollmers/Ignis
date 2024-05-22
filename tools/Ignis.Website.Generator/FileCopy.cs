namespace Ignis.Website.Generator;

internal class FileCopy
{
    private FileCopy() { }

    public static void Copy(DirectoryInfo sourceDirectory, string searchPattern, DirectoryInfo outputDirectory)
    {
        var files = sourceDirectory.EnumerateFiles(searchPattern, SearchOption.AllDirectories);

        foreach (var file in files)
        {
            var relativePath = file.FullName.Replace(sourceDirectory.FullName, string.Empty)
                .TrimStart(Path.DirectorySeparatorChar);

            var destinationPath = Path.Combine(outputDirectory.FullName, relativePath);

            var destinationDirectory = new FileInfo(destinationPath).Directory!;

            if (!destinationDirectory.Exists)
            {
                destinationDirectory.Create();
            }

            File.Copy(file.FullName, destinationPath, true);
        }
    }
}
