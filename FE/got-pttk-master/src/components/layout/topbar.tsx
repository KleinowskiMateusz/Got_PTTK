import React from 'react'
import styled from 'styled-components'
import { useSelector, useDispatch } from 'react-redux'
import tw from 'twin.macro'

import { StoreState } from 'store/reducer'
import { loginUser, logoutUser } from 'store/actions'

import { Text } from 'components/shared/typography'
import Dropdown from 'components/shared/dropdown'

const Wrapper = styled.header`
  ${tw`fixed top-0 left-0 z-20`}
  ${tw`w-full h-16 bg-white border-b`}
  ${tw`flex items-center`}
`

const Content = styled.div`
  ${tw`py-4 px-10 w-full flex justify-between items-center`}
`

const Items = styled.ul`
  ${tw`flex items-center m-0 gap-3`}
`

const Item = styled.li`
  ${tw`my-0 flex items-center gap-2`}
`

const Heading = styled.h1`
  ${tw`text-gray-700 text-lg font-bold`}
`

const DropdownText = styled(Text).attrs({ as: 'button' })`
  ${tw`py-1 px-2 text-left text-xs`}
`

const Navbar: React.FC = () => {
  const isLogged = useSelector<StoreState, boolean>(
    (state) => state.userType !== null
  )
  const dispatch = useDispatch()

  const handleUserLogout = () => {
    dispatch(logoutUser())
  }

  const handleUserLogin = (role: Parameters<typeof loginUser>[0]) => {
    dispatch(loginUser(role))
  }

  return (
    <Wrapper>
      <Content>
        <Items>
          <Item>
            <Heading>E-Książeczka GOT PTTK</Heading>
          </Item>
        </Items>

        <Items>
          <Item>
            {isLogged ? (
              <Text as="button" onClick={handleUserLogout}>
                Wyloguj
              </Text>
            ) : (
              <Dropdown head={<Text as="button">Zaloguj</Text>}>
                <DropdownText onClick={() => handleUserLogin('tourist')}>
                  Turysta
                </DropdownText>
                <DropdownText onClick={() => handleUserLogin('leader')}>
                  Przodownik
                </DropdownText>
                <DropdownText onClick={() => handleUserLogin('worker')}>
                  Pracownik
                </DropdownText>
              </Dropdown>
            )}
          </Item>
        </Items>
      </Content>
    </Wrapper>
  )
}

export default Navbar
