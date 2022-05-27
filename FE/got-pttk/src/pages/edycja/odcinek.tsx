import React from 'react'
import { useLocation, useNavigate } from 'react-router-dom'
import styled from 'styled-components'
import tw from 'twin.macro'

import RouteForm from '../../components/route/form'
import Container from '../../components/shared/container'
import { Heading, Text } from '../../components/shared/typography'

import useUser from '../../hooks/useUser'

import { Route } from '../../types/gotpttk'

const StyledContainer = styled(Container)`
  ${tw`p-6`}
`

const EditRoutePage = () => {
  const navigate = useNavigate()
  const location = useLocation() as { state: { route: Route } }
  const isTourist = useUser() === 'tourist'
  return (
    <>
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
