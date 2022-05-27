import React, { useState } from 'react'
import styled from 'styled-components'
import tw from 'twin.macro'

import Container from '../../components/shared/container'
import { Input } from '../../components/shared/form'
import { Heading } from '../../components/shared/typography'
import Button from '../../components/shared/button'
import List from '../../components/route/list'

import useUser from '../../hooks/useUser'
import useAsync from '../../hooks/useAsync'

import { getRoutes } from '../../requests/api'

import { includes } from '../../utils/string'
import { Link } from 'react-router-dom'

const StyledContainer = styled(Container).attrs({ as: 'section' })`
  ${tw`p-6`}
`

const Header = styled.header`
  ${tw`flex justify-between items-start`}
`

const RoutesPage = () => {
  const [filter, setFilter] = useState('')
  const isWorker = useUser() === 'worker'
  const { value: routes, status } = useAsync(getRoutes)

  return (
    <>
      <StyledContainer>
        <Header>
          <div>
            <Heading margin="none" as="h2">
              Odcinki
            </Heading>
            <Input
              value={filter}
              onChange={(e) => setFilter(e.target.value)}
              placeholder="Wprowadź filtry"
            />
          </div>
          {isWorker && (
            <Button as={Link} primary to="/edycja/odcinek">
              Nowy odcinek
            </Button>
          )}
        </Header>

        {status === 'success' && routes && (
          <List
            routes={routes.filter((route) =>
              filter.length > 0 ? includes(route.name, filter) : true
            )}
          />
        )}
      </StyledContainer>
    </>
  )
}

export default RoutesPage
