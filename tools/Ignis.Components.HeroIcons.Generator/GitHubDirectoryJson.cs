namespace Ignis.Components.HeroIcons.Generator;

public class GitHubDirectoryJson
{
    public PayloadJson Payload { get; set; } = null!;

    public class PayloadJson
    {
        public TreeJson Tree { get; set; } = null!;

        public class TreeJson
        {
            public ItemJson[] Items { get; set; } = null!;

            public class ItemJson
            {
                public string Name { get; set; } = null!;

                public string Path { get; set; } = null!;

                public string ContentType { get; set; } = null!;
            }
        }
    }
}
