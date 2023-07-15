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
exports.ScrollDetector = void 0;
const components_1 = require("@ignis.net/components");
class ScrollDetector extends components_1.ComponentBase {
    constructor($ref) {
        super('Ignis.Components.Web.ScrollDetector', $ref);
        this._onScroll = () => {
            const _ = this.onScroll();
        };
        window.addEventListener('scroll', this._onScroll);
    }
    onScroll() {
        return __awaiter(this, void 0, void 0, function* () {
            yield this.$ref.invokeMethodAsync('OnScrollAsync');
        });
    }
    dispose() {
        super.dispose();
        window.removeEventListener('scroll', this._onScroll);
    }
}
exports.ScrollDetector = ScrollDetector;
