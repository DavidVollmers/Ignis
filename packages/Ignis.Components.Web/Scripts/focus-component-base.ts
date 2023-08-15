export abstract class FocusComponentBase {
    private static readonly _elements: {
        [id: string]: {
            element: Element;
            isFocused: boolean;
            $ref: DotNet.DotNetObject;
        }
    } = {};

    private constructor() {
    }

    public static focus(element: Element): void {
    }

    public static onAfterRender(element: Element, isFocused: boolean, $ref: DotNet.DotNetObject): void {
        console.log($ref);
    }

    public static dispose(element: Element): void {
    }
}