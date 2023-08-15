export abstract class FocusComponentBase {
    private constructor() {
    }

    public static focus(element: Element): void {
    }

    public static onAfterRender(element: Element, isFocused: boolean): void {
    }

    public static dispose(element: Element): void {
    }
}