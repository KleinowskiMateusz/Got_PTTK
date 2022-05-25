import React from 'react'
import styled from 'styled-components'
import { Link } from 'gatsby'
import tw from 'twin.macro'

import { Subheading, Text } from 'components/shared/typography'

import type { Trip } from 'types/gotpttk'

type Props = Pick<Trip, 'id' | 'name' | 'routes' | 'owner'> & {
  veryfication?: boolean
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

const Thumbnail: React.FC<Props> = ({
  id,
  name,
  routes,
  owner,
  veryfication,
}) => {
  return (
    <Link to="/wycieczka" state={{ veryfication: !!veryfication, tripID: id }}>
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
}

export default Thumbnail
