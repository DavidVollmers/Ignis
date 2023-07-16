import {ComponentBase} from '@ignis.net/components'

export class ScrollDetector extends ComponentBase {
    private readonly _onScroll = () => {
        const _ = this.onScroll();
    };

    public constructor($ref: DotNet.DotNetObject) {
        super('Ignis.Components.Web.ScrollDetector', $ref);
        (<any>window).addEventListener('scroll', this._onScroll);
        this._onScroll();
    }

    private async onScroll(): Promise<void> {
        await this.$ref.invokeMethodAsync('OnScrollAsync', window.scrollX, window.scrollY);
    }

    protected dispose(): void {
        super.dispose();
        (<any>window).removeEventListener('scroll', this._onScroll);
    }
}