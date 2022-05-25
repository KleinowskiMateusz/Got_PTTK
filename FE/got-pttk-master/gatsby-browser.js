/**
 * Implement Gatsby's Browser APIs in this file.
 *
 * See: https://www.gatsbyjs.com/docs/browser-apis/
 */

const React = require('react')
const { ThemeProvider } = require('styled-components')
const { Provider } = require('react-redux')

const { default: createStore } = require('store')

const { default: theme } = require('styles/theme')
const { default: GlobalStyles } = require('styles/global')

const { default: Layout } = require('components/layout')

exports.wrapRootElement = ({ element }) => {
  return (
    <Provider store={createStore()}>
      <ThemeProvider theme={theme}>
        <GlobalStyles />
        <Layout>{element}</Layout>
      </ThemeProvider>
    </Provider>
  )
}
