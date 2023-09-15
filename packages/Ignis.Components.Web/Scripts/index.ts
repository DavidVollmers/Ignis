import { registerComponent, registerStaticComponent } from '@ignis.net/components'
import { FocusComponentBase } from './focus-component-base'
import { ScrollDetector } from './scroll-detector'

registerStaticComponent('Ignis.Components.Web.FocusComponentBase', FocusComponentBase)

registerComponent('Ignis.Components.Web.ScrollDetector', ScrollDetector)
