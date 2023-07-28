namespace Ignis.Website.Generator;

internal class XmlCopy
{
    private XmlCopy() { }

    public static void Copy(DirectoryInfo sourceDirectory, DirectoryInfo outputDirectory)
    {
        var xmlFiles = sourceDirectory.EnumerateFiles("*.xml", SearchOption.AllDirectories);

        foreach (var xmlFile in xmlFiles)
        {
            var relativePath = xmlFile.FullName.Replace(sourceDirectory.FullName, string.Empty)
                .TrimStart(Path.DirectorySeparatorChar);

            var destinationPath = Path.Combine(outputDirectory.FullName, relativePath);

            var destinationDirectory = new FileInfo(destinationPath).Directory!;

            if (!destinationDirectory.Exists)
            {
                destinationDirectory.Create();
            }

            File.Copy(xmlFile.FullName, destinationPath, true);
        }
    }
}
