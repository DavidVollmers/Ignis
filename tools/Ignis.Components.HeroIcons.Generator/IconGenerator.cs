using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Ignis.Components.HeroIcons.Generator;

internal class IconGenerator
{
    private readonly HttpClient _httpClient;

    public IconGenerator(HttpClient httpClient)
    {
        _httpClient = httpClient;

        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task GenerateAsync(string repositoryPath, string outputPath)
    {
        var json = await _httpClient.GetFromJsonAsync<GitHubDirectoryJson>(
            $"https://github.com/tailwindlabs/heroicons/tree/master/{repositoryPath}",
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        foreach (var item in json!.Payload.Tree.Items.Where(i => i.ContentType == "file"))
        {
            var svg = await _httpClient.GetStringAsync(
                $"https://raw.githubusercontent.com/tailwindlabs/heroicons/master/{item.Path}");

            var name = GetIconName(item.Name);

            CreateRazorClass(name, svg, outputPath);
        }
    }

    private static void CreateRazorClass(string name, string svg, string outputPath)
    {
        var fileName = name.Replace(".svg", "Icon.razor");

        var razorSvg = svg.Replace("aria-hidden=\"true\"", "aria-hidden=\"true\" @attributes=\"AdditionalAttributes\"");

        var builder = new StringBuilder();

        builder.AppendLine("@inherits HeroIconBase");
        builder.AppendLine();
        builder.AppendLine(razorSvg);

        var content = builder.ToString().Trim();

        using var fileStream = File.Create(Path.Combine(outputPath, fileName));
        using var writer = new StreamWriter(fileStream);
        writer.Write(content);
    }

    private static string GetIconName(string name)
    {
        var builder = new StringBuilder();

        var isNextCharUpper = true;

        foreach (var c in name)
        {
            if (c == '-')
            {
                isNextCharUpper = true;

                continue;
            }

            builder.Append(isNextCharUpper ? char.ToUpper(c) : c);

            isNextCharUpper = false;
        }

        return builder.ToString();
    }
}
