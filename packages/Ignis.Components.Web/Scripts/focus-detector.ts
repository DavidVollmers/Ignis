import {ComponentBase} from '@ignis.net/components';

export class FocusDetector extends ComponentBase {
    private _isInitialized: boolean = false;
    private _isFocused: boolean = true;

    private readonly _onClick = async (event: MouseEvent) => {
        // https://github.com/dotnet/aspnetcore/issues/26809
        window.setTimeout(async () => {
            await this.onClick(event);
        }, 0);
    };

    private readonly _onFocus = () => {
        // https://github.com/dotnet/aspnetcore/issues/26809
        window.setTimeout(async () => {
            await this.onFocus();
        }, 0);
    };

    private readonly _onBlur = async (event: FocusEvent) => {
        // https://github.com/dotnet/aspnetcore/issues/26809
        window.setTimeout(async () => {
            await this.onBlur(event);
        }, 0);
    };

    public constructor($ref: DotNet.DotNetObject, id: string, private readonly _element: HTMLElement) {
        super($ref, id);
        if ($ref != null) {
            (<any>window).addEventListener('click', this._onClick);
            this._element.addEventListener('focusin', this._onFocus);
            this._element.addEventListener('focusout', this._onBlur);
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
        console.log(this._element, event.target);
        if (this._element.contains(<Node>event.target)) {
            await this.onFocus();
        } else {
            await this.onBlur(new FocusEvent('blur'));
        }
    }

    private async onFocus(): Promise<void> {
        console.log('TRY onFocus');
        if (!this._isInitialized || this._isFocused) return;
        this._isFocused = true;
        await this.$ref.invokeMethodAsync('OnFocusAsync', true);
    }

    private async onBlur(event: FocusEvent): Promise<void> {
        console.log('TRY onBlur');
        if (!this._isInitialized || !this._isFocused) return;
        if (event.relatedTarget && this._element.contains(<Node>event.relatedTarget)) return;
        this._isFocused = false;
        await this.$ref.invokeMethodAsync('OnBlurAsync', false);
    }

    protected dispose(): void {
        super.dispose();
        (<any>window).removeEventListener('click', this._onClick);
        this._element.removeEventListener('focusin', this._onFocus)
        this._element.removeEventListener('focusout', this._onBlur)
    }
}
