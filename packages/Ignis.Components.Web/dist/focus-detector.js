"use strict";
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
Object.defineProperty(exports, "__esModule", { value: true });
exports.FocusDetector = void 0;
const components_1 = require("@ignis.net/components");
class FocusDetector extends components_1.ComponentBase {
    constructor($ref, id, _element) {
        super($ref, id);
        this._element = _element;
        this._onClick = (event) => {
            const _ = this.onClick(event);
        };
        window.addEventListener('click', this._onClick);
        if (_element.contains(document.activeElement)) {
            const _ = this.$ref.invokeMethodAsync('OnFocusAsync');
        }
    }
    onClick(event) {
        return __awaiter(this, void 0, void 0, function* () {
            if (this._element.contains(event.target)) {
                yield this.$ref.invokeMethodAsync('OnFocusAsync');
            }
            else {
                yield this.$ref.invokeMethodAsync('OnBlurAsync');
            }
        });
    }
    dispose() {
        super.dispose();
        window.removeEventListener('click', this._onClick);
    }
}
exports.FocusDetector = FocusDetector;
