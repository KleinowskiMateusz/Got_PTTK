import React, { useMemo } from 'react'
import styled from 'styled-components'
import tw from 'twin.macro'

import Container from '../components/shared/container'
import TripDetails from '../components/trip/details'

import { getVerificationTrip } from '../requests/api'

import useAsync from '../hooks/useAsync'
import { useLocation } from 'react-router-dom'

const StyledContainer = styled(Container)`
  ${tw`p-6`}
`

const TripPage = () => {
  const location = useLocation() as any
  const getTrip = useMemo(
    () => getVerificationTrip(location.state?.tripID ?? ''),
    [location.state?.tripID]
  )

  const { value: trip, status, error } = useAsync(getTrip)

  return (
    <>
      <StyledContainer>
        {trip && status === 'success' && !error && (
          <TripDetails
            trip={trip}
            status={location.state.veryfication ? 'finished' : 'preview'}
            isVerified={!location.state.veryfication}
          />
        )}
      </StyledContainer>
    </>
  )
}

export default TripPage
