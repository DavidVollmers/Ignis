using System.Reflection;
using Ignis.Markdown.Processor;

var assemblyFile = new FileInfo(Assembly.GetExecutingAssembly().Location);
var markdownDirectory =
    new DirectoryInfo(Path.Combine(assemblyFile.DirectoryName!, "..", "..", "..", "..", "..", "docs"));
var outputDirectoryServer = new DirectoryInfo(Path.Combine(assemblyFile.DirectoryName!, "..", "..", "..", "..", "..",
    "website", "Ignis.Website.Server", "wwwroot", "docs"));
var outputDirectoryWebAssembly = new DirectoryInfo(Path.Combine(assemblyFile.DirectoryName!, "..", "..", "..", "..",
    "..", "website", "Ignis.Website.WebAssembly", "wwwroot", "docs"));

var builder = new DocsBuilder();
builder.Build(markdownDirectory, outputDirectoryServer);
builder.Build(markdownDirectory, outputDirectoryWebAssembly);
