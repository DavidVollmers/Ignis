namespace Ignis.Website.Generator;

internal class XmlCopy
{
    private XmlCopy() { }

    public static void Copy(DirectoryInfo sourceDirectory, DirectoryInfo outputDirectory)
    {
        var xmlFiles = sourceDirectory.EnumerateFiles("*.xml", SearchOption.AllDirectories);

        foreach (var xmlFile in xmlFiles)
        {
            Console.WriteLine($"XML: {xmlFile.FullName}");
            
            var relativePath = xmlFile.FullName.Replace(sourceDirectory.FullName, string.Empty)
                .TrimStart(Path.DirectorySeparatorChar);

            Console.WriteLine($"RP: {relativePath}");
            
            var destinationPath = Path.Combine(outputDirectory.FullName, relativePath);

            Console.WriteLine($"DP: {destinationPath}");
            
            var destinationDirectory = new FileInfo(destinationPath).Directory!;

            if (!destinationDirectory.Exists)
            {
                destinationDirectory.Create();
            }

            File.Copy(xmlFile.FullName, destinationPath, true);
        }
    }
}
