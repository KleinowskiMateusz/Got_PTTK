import React, { useMemo, useRef, useState, useEffect } from 'react'
import styled from 'styled-components'
import tw from 'twin.macro'

import { Subheading, Text, Title } from 'components/shared/typography'
import ConfirmationDetails from 'components/confirmation/details'
import ConfirmationForm from 'components/confirmation/form'

import useUser from 'hooks/useUser'
import { removeConfirmation as removeConfirmationFromDB } from 'requests/api'

import { Confirmation, Point, Route } from 'types/gotpttk'

const Wrapper = styled.div`
  ${tw`p-2`}
`

const Forms = styled.div`
  ${tw`mt-2`}
`

type Props = {
  point: Point
  routeID?: Route['id'] | null
  confirmations?: Confirmation[]
}

const PointPreview: React.FC<Props> = ({ routeID, point, confirmations }) => {
  const removedConfirmations = useRef<Confirmation['id'][]>([])
  const [remConState, setRemConState] = useState(removedConfirmations.current)
  const addedConfirmations = useRef<Confirmation[]>([])
  const [currentConfirmations, setCurrentConfirmations] = useState<
    Confirmation[]
  >([...(confirmations ?? []), ...addedConfirmations.current])
  const isTourist = useUser() === 'tourist'
  const filteredConfirmations = useMemo<Confirmation[]>(
    () =>
      currentConfirmations?.filter(
        (confirmation) =>
          !remConState.includes(confirmation.id) &&
          confirmation.pointID === point.id &&
          confirmation.routeID === routeID
      ) ?? [],
    [currentConfirmations, removedConfirmations, remConState, point.id, routeID]
  )

  useEffect(
    () =>
      setCurrentConfirmations([
        ...(confirmations ?? []),
        ...addedConfirmations.current,
      ]),
    [confirmations, addedConfirmations]
  )

  const addConfirmation = (confirmation: Confirmation) => {
    addedConfirmations.current.push(confirmation)
    setCurrentConfirmations([...currentConfirmations, confirmation])
  }

  const removeConfirmation = async (id: Confirmation['id']) => {
    try {
      await removeConfirmationFromDB(id)()
      removedConfirmations.current.push(id)
      setRemConState([...remConState, id])
    } catch (error) {
      console.error(error)
    }
  }

  return (
    <Wrapper>
      <Subheading margin="sm">{point.name}</Subheading>

      <Title margin="none">Potwierdzenia terenowe</Title>
      {filteredConfirmations && filteredConfirmations.length > 0 ? (
        filteredConfirmations.map((confirmation, index) => (
          <ConfirmationDetails
            key={`${index}:${confirmation.id}:${confirmation.src}`}
            id={confirmation.id}
            type={confirmation.type}
            src={confirmation.src}
            date={confirmation.date}
            onDelete={removeConfirmation}
          />
        ))
      ) : (
        <Text>Brak potwierdzeń</Text>
      )}
      {isTourist && routeID && (
        <Forms>
          <Title>Dodaj potwierdzenie</Title>
          <ConfirmationForm
            pointID={point.id}
            routeID={routeID}
            onAdd={addConfirmation}
          />
        </Forms>
      )}
    </Wrapper>
  )
}

export default PointPreview
