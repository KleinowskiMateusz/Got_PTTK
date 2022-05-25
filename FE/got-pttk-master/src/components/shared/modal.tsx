import React, { useState, useEffect } from 'react'
import styled from 'styled-components'
import tw from 'twin.macro'

type Props = {
  visible: boolean
  onClose: React.MouseEventHandler
}

const Shadow = styled.div<Pick<Props, 'visible'>>`
  ${tw`fixed z-50 top-0 bottom-0 left-0 right-0 bg-black/50`}
  ${tw`flex items-center justify-center`}
  ${tw`opacity-100 visible ease-in-out duration-200`}

  ${({ visible }) => !visible && tw`opacity-0 invisible`}
`

const Wrapper = styled.div`
  ${tw`bg-white border rounded-sm px-4 py-2`}
`

const Modal: React.FC<Props> = ({ visible, children, onClose }) => {
  const [isVisible, setVisibility] = useState(visible)

  useEffect(() => {
    setVisibility(visible)
  }, [visible])

  const close = (e: React.MouseEvent) => {
    onClose(e)
  }

  return (
    <Shadow visible={isVisible} onClick={close}>
      <Wrapper onClick={(e) => e.stopPropagation()}>{children}</Wrapper>
    </Shadow>
  )
}

export default Modal
