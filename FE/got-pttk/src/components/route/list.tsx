import React from 'react'
import styled from 'styled-components'
import tw from 'twin.macro'

import Thumbnail from './thumbnail'

import type { Route } from '../../types/gotpttk'

type Props = {
  routes: Route[]
}

const Wrapper = styled.div`
  ${tw`pt-6 pb-4 grid grid-cols-2 gap-2`}
`

const List: React.FC<Props> = ({ routes }) => {
  return (
    <Wrapper>
      {routes.map((route) => (
        <Thumbnail key={route.id} route={route} />
      ))}
    </Wrapper>
  )
}

export default List
