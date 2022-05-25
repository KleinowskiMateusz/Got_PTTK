/**
 * SEO component that queries for data with
 *  Gatsby's useStaticQuery React hook
 *
 * See: https://www.gatsbyjs.com/docs/use-static-query/
 */

import * as React from 'react'
import { Helmet } from 'react-helmet'
import { useStaticQuery, graphql } from 'gatsby'

type SeoProps = {
  title: string
  description?: string
  meta?: JSX.IntrinsicElements['meta'][]
  lang?: string
}

type SiteMetadata = {
  site: { siteMetadata: { title: string; description: string; author: string } }
}

const Seo: React.FC<SeoProps> = ({ title, description, lang, meta }) => {
  const {
    site: { siteMetadata },
  } = useStaticQuery<SiteMetadata>(
    graphql`
      query {
        site {
          siteMetadata {
            title
            description
            author
          }
        }
      }
    `
  )

  const defaultTitle = siteMetadata?.title
  const metaDescription = description || siteMetadata.description

  const defaultMeta = [
    {
      name: `description`,
      content: metaDescription,
    },
    {
      property: `og:title`,
      content: title,
    },
    {
      property: `og:description`,
      content: metaDescription,
    },
    {
      property: `og:type`,
      content: `website`,
    },
    {
      name: `twitter:card`,
      content: `summary`,
    },
    {
      name: `twitter:creator`,
      content: siteMetadata?.author || ``,
    },
    {
      name: `twitter:title`,
      content: title,
    },
    {
      name: `twitter:description`,
      content: metaDescription,
    },
  ]
  const combinedMeta = meta ? defaultMeta : [...defaultMeta, ...meta!]

  return (
    <Helmet
      htmlAttributes={{
        lang,
      }}
      title={title}
      titleTemplate={`%s | ${defaultTitle}`}
      meta={combinedMeta}
    />
  )
}

Seo.defaultProps = {
  lang: `en`,
  meta: [],
}

export default Seo
