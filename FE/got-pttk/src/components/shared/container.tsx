import styled from 'styled-components'
import tw from 'twin.macro'
import media from '../../styles/media'

type ContainerProps = {
  fullHeight?: boolean
  fullWidth?: boolean
}

const Container = styled.div<ContainerProps>`
  ${tw`container mx-auto px-4`}

  ${({ fullWidth }) => fullWidth && tw`w-full`}
  ${({ fullHeight }) => fullHeight && tw`h-full`}

  ${media.md.max} {
    ${tw`px-2`}
  }
`

export default Container
