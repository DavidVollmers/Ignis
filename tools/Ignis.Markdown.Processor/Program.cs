using System.Reflection;
using Ignis.Markdown.Processor;

var assemblyFile = new FileInfo(Assembly.GetExecutingAssembly().Location);
var markdownDirectory = new DirectoryInfo(Path.Combine(assemblyFile.DirectoryName!, "..", "..", "..", "..", "..", "docs"));
var outputDirectory = new DirectoryInfo(Path.Combine(assemblyFile.DirectoryName!, "..", "..", "..", "..", "..", "Ignis.Website", "wwwroot", "docs"));

var builder = new DocsBuilder();
builder.Build(markdownDirectory, outputDirectory);
