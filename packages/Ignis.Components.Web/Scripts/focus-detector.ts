import {ComponentBase} from '@ignis.net/components';

export class FocusDetector extends ComponentBase {
    private _isInitialized: boolean = false;
    private _isFocused: boolean = true;

    private readonly _onClick = () => {
        const _ = this.onClick();
    };

    public constructor($ref: DotNet.DotNetObject, id: string, private readonly _element: HTMLElement) {
        super($ref, id);
        if ($ref != null) {
            (<any>window).addEventListener('click', this._onClick);
            window.setTimeout(() => {
                this._element.focus();
                this._isInitialized = true;
            }, 15);
        }
    }

    private async onClick(): Promise<void> {
        if (!this._isInitialized) return;
        if (this._element.contains(document.activeElement)) {
            if (this._isFocused) return;
            this._isFocused = true;
            await this.$ref.invokeMethodAsync('OnFocusAsync', false);
        } else {
            if (!this._isFocused) return;
            this._isFocused = false;
            await this.$ref.invokeMethodAsync('OnBlurAsync', false);
        }
    }

    protected dispose(): void {
        super.dispose();
        (<any>window).removeEventListener('click', this._onClick);
    }
}
