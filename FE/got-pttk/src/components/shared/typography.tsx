/* eslint-disable import/prefer-default-export */
import styled from 'styled-components'
import tw from 'twin.macro'

import { None, Sizes } from '../../types/theme'

type TextProps = {
  margin?: None | Extract<Sizes, 'sm' | 'md' | 'lg'>
}

export const Heading = styled.h1<TextProps>`
  ${tw`text-3xl text-gray-900`}

  ${({ margin }) => {
    switch (margin) {
      case 'sm':
        return tw`mb-2`
      case 'md':
        return tw`mb-4`
      case 'lg':
        return tw`mb-8`
      default:
        return tw`mb-0`
    }
  }}
`

Heading.defaultProps = {
  margin: 'md',
}

export const Subheading = styled(Heading).attrs({ as: 'h2' })`
  ${tw`text-2xl`}
`

export const Title = styled(Heading).attrs({ as: 'h2' })`
  ${tw`text-xl`}

  ${({ margin }) => {
    switch (margin) {
      case 'sm':
        return tw`mb-2`
      case 'md':
        return tw`mb-4`
      case 'lg':
        return tw`mb-6`
      default:
        return tw`mb-0`
    }
  }}
`

Title.defaultProps = {
  margin: 'sm',
}

export const Text = styled.p<TextProps>`
  ${tw`text-sm tracking-wide text-gray-700`}

  ${({ margin }) => {
    switch (margin) {
      case 'sm':
        return tw`mb-2`
      case 'md':
        return tw`mb-4`
      case 'lg':
        return tw`mb-6`
      default:
        return tw`mb-0`
    }
  }}

  a {
    color: inherit;
  }
`

Text.defaultProps = {
  margin: 'none',
}

export const Bold = styled(Text)`
  ${tw`font-bold`}
`
