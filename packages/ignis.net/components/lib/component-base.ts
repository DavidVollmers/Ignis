namespace Ignis.Components.Web {
    export abstract class ComponentBase {
        private static readonly _instances: { [id: string]: ComponentBase } = {};

        protected get element(): HTMLElement {
            return ComponentBase._instances[this._id]._element;
        }

        protected get $ref(): DotNet.DotNetObject {
            return ComponentBase._instances[this._id]._ref;
        }

        protected constructor(private readonly _id: string, private readonly _element: HTMLElement, private readonly _ref: DotNet.DotNetObject) {
            let existingInstance = ComponentBase._instances[_id];
            if (existingInstance) existingInstance.dispose();
            ComponentBase._instances[_id] = this;
        }

        protected dispose(): void {
            delete ComponentBase._instances[this._id];
        }
    }
}
