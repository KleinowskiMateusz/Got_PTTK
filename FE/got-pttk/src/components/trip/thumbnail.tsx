import React from 'react'
import styled from 'styled-components'
import tw from 'twin.macro'

import { Subheading, Text } from '../shared/typography'

import type { Trip } from '../../types/gotpttk'
import { Link } from 'react-router-dom'

type Props = Pick<Trip, 'id' | 'name' | 'routes' | 'owner'> & {
  verification?: boolean
}

const Wrapper = styled.article`
  ${tw`bg-white border rounded-sm px-4 py-2 h-full`}
`

const Header = styled.header``

const Info = styled.div`
  ${tw`flex justify-between pt-4`}

  ${Text} {
    ${tw`text-gray-600 text-xs`}
  }
`

const Thumbnail = ({
  id,
  name,
  routes,
  owner,
  verification,
}: React.PropsWithChildren<Props>) => (
  <Link to="/wycieczka" state={{ verification: !!verification, tripID: id }}>
    <Wrapper>
      <Header>
        <Subheading margin="none" as="h3">
          {name}
        </Subheading>
        {owner && <Text>{owner}</Text>}
      </Header>
      <Info>
        <Text>Ilość odcinków</Text>
        <Text>{routes.length}</Text>
      </Info>
    </Wrapper>
  </Link>
)

export default Thumbnail
