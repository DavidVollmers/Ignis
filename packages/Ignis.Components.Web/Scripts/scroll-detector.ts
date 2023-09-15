import { ComponentBase } from '@ignis.net/components'

export class ScrollDetector extends ComponentBase {
  private readonly _onScroll = () => {
    const _ = this.onScroll()
  }

  public constructor($ref: DotNet.DotNetObject) {
    super($ref, 'Ignis.Components.Web.ScrollDetector')
    window.addEventListener('scroll', this._onScroll)
    this._onScroll()
  }

  private async onScroll(): Promise<void> {
    await this.$ref.invokeMethodAsync('InvokeScrollAsync', window.scrollX, window.scrollY)
  }

  protected dispose(): void {
    super.dispose()
    window.removeEventListener('scroll', this._onScroll)
  }
}
