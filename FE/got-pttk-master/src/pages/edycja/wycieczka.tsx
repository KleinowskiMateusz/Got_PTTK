import React from 'react'
import { PageProps, navigate } from 'gatsby'
import styled from 'styled-components'
import tw from 'twin.macro'

import Seo from 'components/layout/seo'
import TripForm from 'components/trip/form'
import Container from 'components/shared/container'
import { Heading, Text } from 'components/shared/typography'

import { Trip } from 'types/gotpttk'

const StyledContainer = styled(Container)`
  ${tw`p-6`}
`

const EditRoutePage: React.FC<PageProps<{}, {}, { trip?: Trip }>> = ({
  location,
}) => (
  <>
    <Seo title={location.state?.trip ? 'Edycja wycieczki' : 'Nowa wycieczka'} />
    <StyledContainer>
      <Heading margin="none">
        {location.state?.trip ? 'Edycja wycieczki' : 'Nowa wycieczka'}
      </Heading>
      <Text as="button" onClick={() => navigate(-1)} margin="md">
        Powrót
      </Text>
      <TripForm trip={location.state?.trip} />
    </StyledContainer>
  </>
)

export default EditRoutePage
