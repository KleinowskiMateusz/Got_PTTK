import styled from 'styled-components'
import tw from 'twin.macro'

import Container from '../components/shared/container'
import { Heading, Subheading } from '../components/shared/typography'
import Button from '../components/shared/button'

const StyledContainer = styled(Container).attrs({ as: 'article' })`
  ${tw`p-6`}
`

const Header = styled.header`
  ${tw`flex justify-between items-start`}
`

const InfoHeading = styled(Subheading).attrs({ as: 'h3', margin: 'sm' })`
  ${tw`pt-6`}
`

export const Login = () => {
  const onLoginPress = () => {
    console.log('call sso here')
  }

  return (
    <StyledContainer>
      <Header>
        <div>
          <Heading margin="none" as="h2">
            Login
          </Heading>
        </div>
      </Header>
      <InfoHeading>Informacje</InfoHeading>
      <Button onClick={onLoginPress}>Zaloguj</Button>
    </StyledContainer>
  )
}
