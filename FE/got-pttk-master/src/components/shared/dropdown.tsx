import React from 'react'
import styled from 'styled-components'
import tw from 'twin.macro'

const Head = styled.div``

const Body = styled.div`
  ${tw`absolute right-0 opacity-0 invisible transition-all duration-300 transform `}
  ${tw`flex flex-col w-24 mt-2 bg-white border border-gray-200 divide-y divide-gray-100 rounded-md outline-none`}
`

const Wrapper = styled.div`
  ${tw`relative`}

  &:hover, &:focus-within {
    ${Body} {
      ${tw`opacity-100 visible`}
    }
  }
`

type Props = {
  head: React.ReactNode
}

const Dropdown: React.FC<Props> = ({ head, children }) => {
  return (
    <Wrapper>
      <Head>{head}</Head>
      <Body>{children}</Body>
    </Wrapper>
  )
}

export default Dropdown
