import * as React from 'react'
import {
  GatsbyImage,
  getImage,
  IGatsbyImageData,
  ImageDataLike,
} from 'gatsby-plugin-image'

type LazyImageProps = {
  src: ImageDataLike
  alt: string
  width?: number
  height?: number
  objectFit?: React.CSSProperties['objectFit']
  objectPosition?: React.CSSProperties['objectPosition']

  className?: string
}

const LazyImage: React.FC<LazyImageProps> = ({
  src,
  alt,
  width,
  height,
  objectFit,
  objectPosition,
  className,
}) => {
  let image = getImage(src) as IGatsbyImageData
  if (width && height) {
    image = { ...image, width, height }
  } else if (width) {
    image = { ...image, width, height: width * (image.width / image.height) }
  } else if (height) {
    image = { ...image, height, width: height * (image.width / image.height) }
  }

  return (
    <GatsbyImage
      image={image}
      alt={alt}
      objectFit={objectFit}
      objectPosition={objectPosition}
      className={className}
    />
  )
}

export default LazyImage
