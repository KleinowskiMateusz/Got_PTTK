export type Breakpoints = {
  xs: number
  sm: number
  md: number
  lg: number
  xl: number
  xxl: number
}

export type None = 'none'
export type Sizes = keyof Breakpoints

declare module 'styled-components' {
  export interface DefaultTheme {
    breakpoints: Breakpoints
  }
}
