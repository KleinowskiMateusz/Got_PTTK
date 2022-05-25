import React from 'react'
import { PageProps } from 'gatsby'

import Seo from 'components/layout/seo'

const NotFoundPage: React.FC<PageProps> = () => (
  <>
    <Seo title="404: Not found" />
    <h1>404: Not Found</h1>
    <p>You just hit a route that doesn't exist... the sadness.</p>
  </>
)

export default NotFoundPage
