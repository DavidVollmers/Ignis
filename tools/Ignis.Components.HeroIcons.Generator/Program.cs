using System.Reflection;
using Ignis.Components.HeroIcons.Generator;

using var httpClient = new HttpClient();

var generator = new IconGenerator(httpClient);

var assemblyLocation = new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName!;

var outputPath = Path.Combine(assemblyLocation, "..", "..", "..", "..", "..", "packages", "Tailwind",
    "Ignis.Components.HeroIcons");

await generator.GenerateAsync("optimized/24/solid", Path.Combine(outputPath, "Solid"));
await generator.GenerateAsync("optimized/24/outline", Path.Combine(outputPath, "Outline"));
await generator.GenerateAsync("optimized/20/solid", Path.Combine(outputPath, "Mini"));
