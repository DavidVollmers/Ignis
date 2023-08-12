import {ComponentBase} from '@ignis.net/components';

export class FocusDetector extends ComponentBase {
    private _isInitialized: boolean = false;
    private _isFocused: boolean = true;

    private readonly _onClick = async (event: MouseEvent) => {
        await this.onClick(event);
    };

    private readonly _onFocus = async () => {
        await this.onFocus();
    };

    private readonly _onBlur = async () => {
        await this.onBlur();
    };

    public constructor($ref: DotNet.DotNetObject, id: string, private readonly _element: HTMLElement) {
        super($ref, id);
        if ($ref != null) {
            (<any>window).addEventListener('click', this._onClick);
            this._element.addEventListener('blur', this._onBlur);
            this._element.addEventListener('focus', this._onFocus);
            window.setTimeout(() => {
                if (this._element.contains(document.activeElement)) {
                    this._isFocused = true;
                    this.$ref.invokeMethodAsync('OnFocusAsync', true);
                }
                this._isInitialized = true;
            }, 15);
        }
    }

    private async onClick(event: MouseEvent): Promise<void> {
        if (this._element.contains(<Node>event.target)) {
            await this.onFocus();
        } else {
            await this.onBlur();
        }
    }

    private async onFocus(): Promise<void> {
        if (!this._isInitialized) return;
        if (this._isFocused) return;
        this._isFocused = true;
        await this.$ref.invokeMethodAsync('OnFocusAsync', true);
    }
    
    private async onBlur(): Promise<void> {
        if (!this._isInitialized) return;
        if (!this._isFocused) return;
        this._isFocused = false;
        await this.$ref.invokeMethodAsync('OnBlurAsync', false);
    }

    protected dispose(): void {
        super.dispose();
        (<any>window).removeEventListener('click', this._onClick);
        this._element.removeEventListener('focus', this._onFocus)
    }
}
