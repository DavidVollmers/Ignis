using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

namespace Ignis.Tests.E2E;

[TestFixture]
[Parallelizable(ParallelScope.Self)]
public class DialogTests : PageTest
{
    [Test]
    public async Task Test_Dialog_Custom_OpenCloseMultipleTimes()
    {
        await Page.GotoAsync("https://e2e.ignis.dvolper.dev/tests/dialog/custom");

        await OpenClose();
        await OpenClose();
        await OpenClose();

        return;

        async Task OpenClose()
        {
            var dialog = Page.GetByTestId("dialog");

            await Expect(dialog).ToBeHiddenAsync();

            var openButton = Page.GetByTestId("open-button");

            await openButton.ClickAsync();

            await Task.Delay(400);

            await Expect(dialog).ToBeInViewportAsync();

            await Page.ClickAsync(".fixed.inset-0.bg-gray-500",
                new PageClickOptions { Position = new Position { X = 0, Y = 0 } });

            await Task.Delay(300);

            await Expect(dialog).ToBeHiddenAsync();
        }
    }
}
