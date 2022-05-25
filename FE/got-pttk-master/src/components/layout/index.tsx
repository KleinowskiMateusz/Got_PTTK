import React from 'react'
import styled from 'styled-components'

import GlobalStyles from 'styles/global'

import Topbar from 'components/layout/topbar'
import Sidebar from 'components/layout/sidebar'
import tw from 'twin.macro'

const Wrapper = styled.div`
  ${tw`flex min-h-screen`}
`

const Content = styled.main`
  ${tw`flex-1 pt-16 pl-64 bg-gray-50`}
`

const Layout: React.FC = ({ children }) => {
  return (
    <>
      <GlobalStyles />

      <Wrapper>
        <Topbar />
        <Sidebar />
        <Content>{children}</Content>
      </Wrapper>
    </>
  )
}

export default Layout
