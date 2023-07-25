import {ComponentBase} from '@ignis.net/components';

export class FocusDetector extends ComponentBase {
    private readonly _onClick = (event: MouseEvent) => {
        const _ = this.onClick(event);
    };

    public constructor($ref: DotNet.DotNetObject, id: string, private readonly _element: HTMLElement, private _isFocused: boolean) {
        super($ref, id);
        if ($ref != null) {
            (<any>window).addEventListener('click', this._onClick);
        }
    }

    private async onClick(event: MouseEvent): Promise<void> {
        if (this._element.contains(<Node>event.target)) {
            if (this._isFocused) return;
            await this.$ref.invokeMethodAsync('OnFocusAsync');
            this._isFocused = true;
        } else {
            if (!this._isFocused) return;
            await this.$ref.invokeMethodAsync('OnBlurAsync');
            this._isFocused = false;
        }
    }

    protected dispose(): void {
        super.dispose();
        (<any>window).removeEventListener('click', this._onClick);
    }
}
