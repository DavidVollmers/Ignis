import {ComponentBase} from "./component-base";

export interface ComponentConstructor<T extends ComponentBase> {
    new($ref: DotNet.DotNetObject, ...args: any[]): T;
}

export function registerComponent<T extends ComponentBase>(name: string, component: ComponentConstructor<T>): void {
    let scope: any = window;
    const nameParts = name.split('.');
    while (nameParts.length > 1) {
        const namePart = <string>nameParts.shift();
        if (!scope[namePart]) scope[namePart] = {};
        scope = scope[namePart];
    }
    const lastNamePart = <string>nameParts.shift();
    scope[lastNamePart] = ($ref: DotNet.DotNetObject, ...args: any[]) => {
        return new component($ref, ...args);
    };
}