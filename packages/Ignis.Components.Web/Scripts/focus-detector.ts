import {ComponentBase} from '@ignis.net/components';

export class FocusDetector extends ComponentBase {
    private readonly _onClick = (event: MouseEvent) => {
        const _ = this.onClick(event);
    };

    public constructor($ref: DotNet.DotNetObject, id: string, private readonly _element: HTMLElement) {
        super($ref, id);
        (<any>window).addEventListener('click', this._onClick);
        if (this._element.contains(document.activeElement)) {
            const _ = this.$ref.invokeMethodAsync('OnFocusAsync');
        }
    }

    private async onClick(event: MouseEvent): Promise<void> {
        console.log(this);
        if (this._element.contains(<Node>event.target)) {
            await this.$ref.invokeMethodAsync('OnFocusAsync');
        } else {
            await this.$ref.invokeMethodAsync('OnBlurAsync');
        }
    }

    protected dispose(): void {
        super.dispose();
        (<any>window).removeEventListener('click', this._onClick);
    }
}
