import React from 'react'
import styled from 'styled-components'
import tw from 'twin.macro'

import Container from '../components/shared/container'
import {
  Heading,
  Subheading,
  Text,
  Bold,
} from '../components/shared/typography'
import Button from '../components/shared/button'

import { Route } from '../types/gotpttk'

import useUser from '../hooks/useUser'
import { Link, useLocation, useNavigate } from 'react-router-dom'

const StyledContainer = styled(Container).attrs({ as: 'article' })`
  ${tw`p-6`}
`

const Header = styled.header`
  ${tw`flex justify-between items-start`}
`

const InfoHeading = styled(Subheading).attrs({ as: 'h3', margin: 'sm' })`
  ${tw`pt-6`}
`

const Info = styled.section`
  ${tw`bg-white border rounded-sm px-4 py-2`}
`

const GridInfo = styled(Info)`
  ${tw`grid grid-cols-2`}
`

const Data = styled.div`
  ${tw`flex gap-1 py-1 m-0`}
`

const RoutePage = () => {
  const isWorker = useUser() === 'worker'
  const navigate = useNavigate()
  const location = useLocation()
  const { route } = location.state as { route: Route }

  return (
    <>
      <StyledContainer>
        <Header>
          <div>
            <Heading margin="none" as="h2">
              {route.name}
            </Heading>
            <Text as="button" onClick={() => navigate(-1)}>
              Powrót
            </Text>
          </div>
          {isWorker && (
            <Button as={Link} primary to="/edycja/odcinek" state={{ route }}>
              Edytuj odcinek
            </Button>
          )}
        </Header>

        <InfoHeading>Informacje</InfoHeading>
        <GridInfo>
          <Data>
            <Bold>Od:</Bold>
            <Text>{route.from.name}</Text>
          </Data>
          <Data>
            <Bold>Do:</Bold>
            <Text>{route.to.name}</Text>
          </Data>
          <Data>
            <Bold>Pasmo górskie:</Bold>
            <Text>{route.mountainRange}</Text>
          </Data>
          <Data>
            <Bold>Punktacja:</Bold>
            <Text>
              {route.points.from} / {route.points.to}
            </Text>
          </Data>
        </GridInfo>
      </StyledContainer>
    </>
  )
}

export default RoutePage
