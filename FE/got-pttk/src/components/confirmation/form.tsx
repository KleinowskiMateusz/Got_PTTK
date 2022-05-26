import React, { useState } from 'react'
import styled from 'styled-components'
import tw from 'twin.macro'

import Button from '../../components/shared/button'
import { Confirmation, Point, Route } from '../../types/gotpttk'
import ImageForm from './imageForm'
import QRForm from './qrForm'

type Props = {
  pointID: Point['id']
  routeID: Route['id']
  onAdd: (confirmation: Confirmation) => void
}

const Buttons = styled.div`
  ${tw`flex gap-2 justify-between mb-2`}
`

const ConfirmationForm: React.FC<Props> = ({ routeID, pointID, onAdd }) => {
  const [formType, setFormType] = useState<Confirmation['type']>('image')

  return (
    <>
      <Buttons>
        <Button
          primary={formType === 'image'}
          onClick={() => setFormType('image')}
        >
          Obrazek
        </Button>
        <Button
          primary={formType === 'qr'}
          onClick={() => setFormType('qr')}
          disabled
        >
          Kod QR
        </Button>
      </Buttons>
      {formType === 'image' ? (
        <ImageForm routeID={routeID} pointID={pointID} onAdd={onAdd} />
      ) : (
        <QRForm routeID={routeID} pointID={pointID} />
      )}
    </>
  )
}

export default ConfirmationForm
