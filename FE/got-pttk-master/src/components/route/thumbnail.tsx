import React from 'react'
import styled from 'styled-components'
import { Link } from 'gatsby'
import tw from 'twin.macro'

import { Title, Text } from 'components/shared/typography'

import type { Route } from 'types/gotpttk'

type Props = {
  route: Route
}

const Wrapper = styled.article`
  ${tw`flex justify-between bg-white border rounded-sm px-4 py-2 gap-2 h-full`}
`

const Header = styled.header`
  ${tw`flex flex-col justify-between h-full`}
`

const Info = styled.div`
  ${tw`flex flex-col justify-between items-end`}
`

const LocationText = styled(Text)`
  ${tw`text-gray-600 text-xs`}
`

const PointsText = styled(Text)`
  ${tw`text-gray-600 text-xs text-right`}

  font-size: 0.55rem;
`

const Thumbnail: React.FC<Props> = ({ route }) => {
  return (
    <Link to="/odcinek" state={{ route }}>
      <Wrapper>
        <Header>
          <div>
            <Title margin="none" as="h3">
              {route.name}
            </Title>
            <Text margin="md">{route.direction}</Text>
          </div>
          <LocationText margin="none">{route.mountainRange}</LocationText>
        </Header>
        <Info>
          <PointsText>{route.from.name}</PointsText>
          <PointsText>
            {route.points.from} / {route.points.to}
          </PointsText>
          <PointsText>{route.to.name}</PointsText>
        </Info>
      </Wrapper>
    </Link>
  )
}

export default Thumbnail
