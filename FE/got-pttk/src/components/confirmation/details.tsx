import React, { useState } from 'react'
import styled from 'styled-components'
import tw from 'twin.macro'

import { Confirmation } from '../../types/gotpttk'
import { Text } from '../../components/shared/typography'
import Button from '../../components/shared/button'
import Image from '../../components/shared/image'
import { ASSETS_URL } from '../../constants/api'
import useUser from '../../hooks/useUser'

const Wrapper = styled.div`
  ${tw`mt-1 mb-3 divide-y border-solid border-gray-300`}
`

const Buttons = styled.div`
  ${tw`flex gap-2`}
`

type Props = {
  onDelete: (id: Confirmation['id']) => void
} & Pick<Confirmation, 'id' | 'date' | 'src' | 'type'>

const ConfirmationDetails: React.FC<Props> = ({
  type,
  src,
  date,
  id,
  onDelete,
}) => {
  const isTourist = useUser() === 'tourist'
  const [visible, setVisible] = useState(false)

  const toggleVisible = () => setVisible(!visible)
  const deleteConfirmation = () => {
    onDelete(id)
  }

  return (
    <Wrapper>
      <Text>Data wykonania: {date.toLocaleDateString('pl')}</Text>
      <Buttons>
        <Button primary onClick={toggleVisible}>
          {visible ? 'Ukryj' : 'Zobacz'}{' '}
          {type === 'qr' ? 'kod QR' : 'Fotografie'}
        </Button>
        {isTourist && (
          <Button danger onClick={deleteConfirmation}>
            Usuń
          </Button>
        )}
      </Buttons>

      {visible && (
        <Image
          src={`${ASSETS_URL}/${src}`}
          alt=""
          height={175}
          objectFit="contain"
          objectPosition="left center"
        />
      )}
    </Wrapper>
  )
}

export default ConfirmationDetails
