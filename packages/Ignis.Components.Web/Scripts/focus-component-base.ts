export abstract class FocusComponentBase {
    private static readonly _elements: {
        [id: string]: {
            isFocused: boolean,
            elements: Element[],
            $ref: DotNet.DotNetObject
        }
    } = {};

    private constructor() {
    }

    public static focus(elements: Element[]): void {
    }

    public static onAfterRender(elements: Element[], isFocused: boolean, $ref: DotNet.DotNetObject): void {
        console.log($ref);
    }

    public static dispose(elements: Element[]): void {
    }
}