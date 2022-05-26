import { Provider } from 'react-redux'
import { createStore } from 'redux'
import styled, { ThemeProvider } from 'styled-components'
import tw from 'twin.macro'
import Layout from './components/layout'
import Container from './components/shared/container'
import { Heading, Subheading, Text } from './components/shared/typography'
import GlobalStyles from './styles/global'
import theme from './styles/theme'
import reducer from './store/reducer'
import { BrowserRouter, Route, Routes } from 'react-router-dom'
import RoutePage from './pages/odcinek'
import TripPage from './pages/wycieczka'
import VerificationPage from './pages/weryfikacja'
import RoutesPage from './pages/publiczne/odcinki'
import EditRoutePage from './pages/edycja/odcinek'
import EditTripPage from './pages/edycja/wycieczka'

const StyledContainer = styled(Container)`
  ${tw`p-6`}
`

const DefaultView = () => (
  <StyledContainer>
    <Heading margin="none">E-Książeczka GOT PTTK</Heading>
    <Text>Mikołaj Macioszczyk</Text>
    <Text>Matuesz Kleinowski</Text>
  </StyledContainer>
)

const App = () => (
  <BrowserRouter>
    <Provider store={createStore(reducer)}>
      <ThemeProvider theme={theme}>
        <GlobalStyles />
        <Layout>
          <Routes>
            <Route path="/" element={<DefaultView />} />
            <Route path="/odcinek" element={<RoutePage />} />
            <Route path="/wycieczka" element={<TripPage />} />
            <Route path="/weryfikacja" element={<VerificationPage />} />
            <Route path="/publiczne/odcinki" element={<RoutesPage />} />
            <Route path="/edycja/odcinek" element={<EditRoutePage />} />
            <Route path="/edycja/wycieczka" element={<EditTripPage />} />
          </Routes>
        </Layout>
      </ThemeProvider>
    </Provider>
  </BrowserRouter>
)

export default App
