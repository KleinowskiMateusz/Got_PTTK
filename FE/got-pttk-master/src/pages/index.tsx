import React from 'react'
import { PageProps } from 'gatsby'
import styled from 'styled-components'
import tw from 'twin.macro'

import Seo from 'components/layout/seo'
import Container from 'components/shared/container'

import { Heading, Subheading, Text } from 'components/shared/typography'

const StyledContainer = styled(Container)`
  ${tw`p-6`}
`

const IndexPage: React.FC<PageProps> = () => {
  return (
    <>
      <Seo title="Home" />
      <StyledContainer>
        <Heading margin="none">E-Książeczka GOT PTTK</Heading>
        <Subheading>Demo planowania wycieczki</Subheading>
        <Text>Mateusz Kleinowski</Text>
        <Text>Mikołaj Macioszczyk</Text>
      </StyledContainer>
    </>
  )
}

export default IndexPage
