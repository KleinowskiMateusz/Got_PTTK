import { css, createGlobalStyle } from 'styled-components'

import Normalize from './normalize'
import './global.css'

const Global = css`
  body {
    font-family: Karla, sans-serif;
  }

  h1,
  h2,
  h3,
  h4,
  h5,
  h6 {
    font-family: Rubik, sans-serif;
  }
`

const GlobalStyles = createGlobalStyle`
    ${Normalize}
    ${Global} 
`

export default GlobalStyles
