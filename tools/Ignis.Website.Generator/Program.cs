using System.Reflection;
using Ignis.Website.Generator;

var assemblyFile = new FileInfo(Assembly.GetExecutingAssembly().Location);
var markdownDirectory =
    new DirectoryInfo(Path.Combine(assemblyFile.DirectoryName!, "..", "..", "..", "..", "..", "docs"));
var outputDirectoryServer = new DirectoryInfo(Path.Combine(assemblyFile.DirectoryName!, "..", "..", "..", "..", "..",
    "website", "Ignis.Website.Server", "wwwroot", "_docs"));
var outputDirectoryWebAssembly = new DirectoryInfo(Path.Combine(assemblyFile.DirectoryName!, "..", "..", "..", "..",
    "..", "website", "Ignis.Website.WebAssembly", "wwwroot", "_docs"));

var builder = new DocsBuilder();
builder.Build(markdownDirectory, outputDirectoryServer);
builder.Build(markdownDirectory, outputDirectoryWebAssembly);

var sourceDirectory = new DirectoryInfo(Path.Combine(assemblyFile.DirectoryName!, "..", "..", "..", "..", "..",
    "docs", "api"));
outputDirectoryServer = new DirectoryInfo(Path.Combine(assemblyFile.DirectoryName!, "..", "..", "..", "..", "..",
    "website", "Ignis.Website.Server", "wwwroot", "_api"));
outputDirectoryWebAssembly = new DirectoryInfo(Path.Combine(assemblyFile.DirectoryName!, "..", "..", "..", "..",
    "..", "website", "Ignis.Website.WebAssembly", "wwwroot", "_api"));

FileCopy.Copy(sourceDirectory, "*.json", outputDirectoryServer);
FileCopy.Copy(sourceDirectory, "*.json", outputDirectoryWebAssembly);
