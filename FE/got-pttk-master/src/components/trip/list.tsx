import React from 'react'
import styled from 'styled-components'
import tw from 'twin.macro'

import type { VerificationTrip } from 'types/gotpttk'
import Thumbnail from './thumbnail'

type Props = {
  trips: VerificationTrip[]
  veryfication?: boolean
}

const Wrapper = styled.div`
  ${tw`pt-6 pb-4 grid grid-cols-3 gap-2`}
`

const List: React.FC<Props> = ({ trips, veryfication }) => {
  return (
    <Wrapper>
      {trips.map((trip) => (
        <Thumbnail
          key={trip.id}
          name={trip.name}
          routes={trip.routes}
          owner={trip.owner}
          veryfication={veryfication}
          id={trip.id}
        />
      ))}
    </Wrapper>
  )
}

export default List
