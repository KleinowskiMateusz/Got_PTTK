import React, { useMemo } from 'react'
import { PageProps } from 'gatsby'
import styled from 'styled-components'
import tw from 'twin.macro'

import Seo from 'components/layout/seo'
import Container from 'components/shared/container'
import TripDetails from 'components/trip/details'

import { getVerificationTrip } from 'requests/api'

import useAsync from 'hooks/useAsync'

const StyledContainer = styled(Container)`
  ${tw`p-6`}
`

const TripPage: React.FC<
  PageProps<{}, {}, { veryfication?: boolean; tripID?: string }>
> = ({ location }) => {
  const getTrip = useMemo(
    () => getVerificationTrip(location.state?.tripID ?? ''),
    [location.state?.tripID]
  )

  const { value: trip, status, error } = useAsync(getTrip)

  return (
    <>
      <Seo title={`Wycieczka ${trip?.name && ''}`} />
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
