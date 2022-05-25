import React from 'react'
import { PageProps, navigate } from 'gatsby'
import styled from 'styled-components'
import tw from 'twin.macro'

import Seo from 'components/layout/seo'
import RouteForm from 'components/route/form'
import Container from 'components/shared/container'
import { Heading, Text } from 'components/shared/typography'

import useUser from 'hooks/useUser'

import { Route } from 'types/gotpttk'

const StyledContainer = styled(Container)`
  ${tw`p-6`}
`

const EditRoutePage: React.FC<PageProps<{}, {}, { route?: Route }>> = ({
  location,
}) => {
  const isTourist = useUser() === 'tourist'
  return (
    <>
      <Seo title={location.state?.route ? 'Edycja odcinka' : 'Nowy odcinek'} />
      <StyledContainer>
        <Heading margin="none">
          {location.state?.route ? 'Edycja odcinka' : 'Nowy odcinek'}
        </Heading>
        <Text as="button" onClick={() => navigate(-1)} margin="md">
          Powrót
        </Text>
        <RouteForm route={location.state?.route} isPrivate={isTourist} />
      </StyledContainer>
    </>
  )
}

export default EditRoutePage
