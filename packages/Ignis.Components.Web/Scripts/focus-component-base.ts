export abstract class FocusComponentBase {
    private static readonly _instances: {
        [id: number]: {
            isFocused: boolean,
            elements: HTMLElement[],
            $ref: DotNet.DotNetObject
        }
    } = {};

    private static _isInitialized: boolean = false;
    private static _lastId: number = 0;

    private constructor() {
    }

    public static async focus($ref: DotNet.DotNetObject): Promise<void> {
        const target = FocusComponentBase._instances[(<any>$ref)._id];
        if (target == null || target.isFocused) return;
        target.isFocused = true;
        for (const element of target.elements) {
            element.focus();
            if (document.activeElement === element) break;
        }
        await target.$ref.invokeMethodAsync('InvokeFocusAsync');
    }

    public static async updateTargets($ref: DotNet.DotNetObject, elements: HTMLElement[], isFocused: boolean, focusOnRender: boolean): Promise<void> {
        await FocusComponentBase.initialize();
        // delay registration so that components don't get blurred if shown by a click outside of the component.
        window.setTimeout(async () => {
            const id = (<any>$ref)._id;
            if (FocusComponentBase._lastId >= id && !FocusComponentBase._instances[id]) return;
            const focusImmediately = focusOnRender && !FocusComponentBase._instances[id];
            FocusComponentBase._instances[id] = {
                isFocused: isFocused,
                elements: elements,
                $ref: $ref
            };
            if (id > FocusComponentBase._lastId) FocusComponentBase._lastId = id;
            if (focusImmediately) await FocusComponentBase.focus($ref);
        }, 0);
    }

    public static dispose($ref: DotNet.DotNetObject): void {
        const id = (<any>$ref)._id;
        FocusComponentBase._instances[id] = <any>undefined;
        delete FocusComponentBase._instances[id];
    }

    private static async onFocus(event: Event): Promise<void> {
        const target = <Node>event.target;
        if (target == null) return;
        for (const id in FocusComponentBase._instances) {
            const instance = FocusComponentBase._instances[id];
            if (instance == null) continue;
            let isMatch = false;
            for (const element of instance.elements) {
                if (!element || !element.contains(target)) continue;
                isMatch = true;
            }
            if (isMatch) {
                if (instance.isFocused) continue;
                instance.isFocused = true;
                await instance.$ref.invokeMethodAsync('InvokeFocusAsync');
            } else {
                if (!instance.isFocused) continue;
                instance.isFocused = false;
                await instance.$ref.invokeMethodAsync('InvokeBlurAsync');
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