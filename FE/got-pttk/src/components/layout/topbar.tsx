import React from 'react'
import ReactDOM from 'react-dom';
import { GoogleLogin, GoogleLogout } from 'react-google-login';
import {  } from 'react-google-login';
import styled from 'styled-components'
import { useSelector, useDispatch } from 'react-redux'
import tw from 'twin.macro'

import { StoreState } from '../../store/reducer'
import { loginUser, logoutUser } from '../../store/actions'

import { Text } from '../shared/typography'
import Dropdown from '../shared/dropdown'

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

const responseSuccess = (response: any) => {
  console.log(response);
  // var profile = googleUser.getBasicProfile();
  // console.log('ID: ' + profile.getId()); // Do not send to your backend! Use an ID token instead.
  // console.log('Name: ' + profile.getName());
  // console.log('Image URL: ' + profile.getImageUrl());
  // console.log('Email: ' + profile.getEmail()); // This is null if the 'email' scope is not present.
}

const logoutGoogle = (respose: any) => {
  console.log(respose);
}

const responseFailure = (response: any) => {
  console.error(response);
}

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
          <GoogleLogin
            clientId="934648962657-1ogia1tb096mhrdubl72daik0sdlm1k4.apps.googleusercontent.com"
            buttonText="Login with Google"
            onSuccess={responseSuccess}
            onFailure={responseFailure}
            cookiePolicy={'single_host_origin'}
          />
          {/* <GoogleLogout
            clientId="658977310896-knrl3gka66fldh83dao2rhgbblmd4un9.apps.googleusercontent.com"
            buttonText="Logout"
          >
          </GoogleLogout> */}
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
