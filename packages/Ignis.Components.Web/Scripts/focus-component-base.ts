export abstract class FocusComponentBase {
    private static _isInitialized: boolean = false;
    private static _elements: {
        [id: number]: {
            isFocused: boolean,
            elements: HTMLElement[],
            $ref: DotNet.DotNetObject
        }
    } = {};

    private constructor() {
    }

    public static async focus($ref: DotNet.DotNetObject): Promise<void> {
        const target = FocusComponentBase._elements[(<any>$ref)._id];
        if (target == null || target.isFocused) return;
        target.isFocused = true;
        if (target.elements.length) target.elements[0].focus();
        await target.$ref.invokeMethodAsync('InvokeFocusAsync');
    }

    public static async onAfterRender($ref: DotNet.DotNetObject, elements: HTMLElement[], isFocused: boolean): Promise<void> {
        await FocusComponentBase.initialize();
        FocusComponentBase._elements[(<any>$ref)._id] = {
            isFocused: isFocused,
            elements: elements,
            $ref: $ref
        };
    }

    public static dispose($ref: DotNet.DotNetObject): void {
        delete FocusComponentBase._elements[(<any>$ref)._id];
    }

    private static async onFocus(event: Event): Promise<void> {
        const target = <Node>event.target;
        if (target == null) return;
        for (const key in FocusComponentBase._elements) {
            const current = FocusComponentBase._elements[key];
            let isMatch = false;
            for (const element of current.elements) {
                if (element !== target || !element.contains(target)) continue;
                isMatch = true;
            }
            if (isMatch) {
                if (current.isFocused) return;
                current.isFocused = true;
                await current.$ref.invokeMethodAsync('InvokeFocusAsync');
            } else {
                if (!current.isFocused) return;
                current.isFocused = false;
                await current.$ref.invokeMethodAsync('InvokeBlurAsync');
            }
        }
    }

    private static async initialize(): Promise<void> {
        if (FocusComponentBase._isInitialized) return;
        FocusComponentBase._isInitialized = true;
        document.addEventListener('click', FocusComponentBase.onFocus);
        document.addEventListener('focusin', FocusComponentBase.onFocus);
    }
}