import React, { useState } from 'react'
import { PageProps } from 'gatsby'
import styled from 'styled-components'
import tw from 'twin.macro'

import Seo from 'components/layout/seo'
import Container from 'components/shared/container'
import { Input } from 'components/shared/form'
import { Heading } from 'components/shared/typography'
import List from 'components/trip/list'

import { includes } from 'utils/string'

import { getVerificationTrips } from 'requests/api'

import useAsync from 'hooks/useAsync'

const StyledContainer = styled(Container)`
  ${tw`p-6`}
`

const VerificationPage: React.FC<PageProps> = () => {
  const [filter, setFilter] = useState('')
  const { value: trips, status, error } = useAsync(getVerificationTrips)

  return (
    <>
      <Seo title="Weryfikacja" />
      <StyledContainer>
        <Heading margin="none" as="h2">
          Weryfikacja
        </Heading>
        <Input
          value={filter}
          onChange={(e) => setFilter(e.target.value)}
          placeholder="Wprowadź filtry"
        />
        {status === 'success' && trips && !error && (
          <List
            trips={trips.filter((trip) =>
              filter.length > 0 ? includes(trip.name, filter) : true
            )}
            veryfication
          />
        )}
      </StyledContainer>
    </>
  )
}

export default VerificationPage
