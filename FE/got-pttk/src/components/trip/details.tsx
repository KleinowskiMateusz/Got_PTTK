import React, { useState, useRef, useMemo } from 'react'
import styled from 'styled-components'
import tw from 'twin.macro'

import Container from '../shared/container'
import { Heading, Subheading, Text, Bold } from '../shared/typography'
import Modal from '../shared/modal'
import Button from '../shared/button'
import { Form, Label, Input } from '../shared/form'
import PointPreview from '../trip/point'

import { Confirmation, Point, VerificationTrip } from '../../types/gotpttk'

import useUser from '../../hooks/useUser'
import { verifyTrip } from '../../requests/api'

const StyledContainer = styled(Container)`
  ${tw`p-6`}
`

const InfoHeading = styled(Subheading).attrs({ as: 'h3', margin: 'sm' })`
  ${tw`pt-6`}
`

const Info = styled.section`
  ${tw`bg-white border rounded-sm px-4 py-2`}
`

const InfoList = styled.ol`
  ${tw`m-0 list-none`}
`

const InfoListItem = styled.li`
  ${tw`m-0`}
`

const GridInfo = styled(Info)`
  ${tw`grid grid-cols-2`}
`

const Data = styled.div`
  ${tw`flex gap-1 py-1 m-0`}
`

const Veryfication = styled.section`
  ${tw`flex gap-2 pt-4`}
`

const DenyForm = styled(Form)`
  ${tw`py-1 w-48`}
`

const FormButtons = styled.div`
  ${tw`flex justify-between`}
`

type Props = {
  trip: VerificationTrip
  status: 'preview' | 'started' | 'finished'
  isVerified: boolean
}

const TripDetails: React.FC<Props> = ({ trip, status, isVerified }) => {
  const user = useUser()
  const denyRef = useRef<null | HTMLInputElement>(null)
  const toVerification =
    !isVerified && status === 'finished' && user === 'leader'

  const points: Point[] = trip.routes
    ? [trip.routes[0].from, ...trip.routes.map((route) => route.to)]
    : []

  const [modalVisible, setModalVisible] = useState(false)
  const [denyModalVisible, setDenyModalVisible] = useState(false)
  const [currentPoint, setCurrentPoint] = useState<null | Point>(null)

  const openModal = () => setModalVisible(true)
  const closeModal = () => setModalVisible(false)
  const openDenyModal = () => setDenyModalVisible(true)
  const closeDenyModal = () => setDenyModalVisible(false)

  const openPoint = (point: Point) => {
    if (status !== 'preview') {
      setCurrentPoint(point)
      openModal()
    }
  }

  const acceptTrip = async () => {
    try {
      await verifyTrip({
        wycieczka: Number(trip.id),
        przodownik: 'Przodownik1',
        zaakceptiowana: true,
        powodOdrzucenia: null,
      })()
      alert('Zaakceptowano!')
    } catch (error) {
      console.error(error)
    }
  }

  const denyTrip = async () => {
    try {
      await verifyTrip({
        wycieczka: Number(trip.id),
        przodownik: 'Przodownik1',
        zaakceptiowana: false,
        powodOdrzucenia: denyRef.current?.value || null,
      })()
      alert(`Odrzucono: ${denyRef.current?.value}`)
    } catch (error) {
      console.error(error)
    }
  }

  const getConfirmations = () => {
    const confirmations = new Map<string, Confirmation>()
    trip.confirmations
      .filter((confirmation) => confirmation.pointID === currentPoint?.id)
      .forEach((confirmation) =>
        confirmations.set(confirmation.id, confirmation)
      )

    return [...confirmations.values()]
  }

  const currentConfirmations = useMemo(() => getConfirmations(), [currentPoint])
  const currentRouteID = useMemo(() => {
    const found = trip.routes.find(
      (route) =>
        route.to.id === currentPoint?.id || route.from.id === currentPoint?.id
    )

    return found ? found.id : null
  }, [currentPoint])

  return (
    <>
      {status !== 'preview' && (
        <Modal visible={modalVisible} onClose={closeModal}>
          {currentPoint && (
            <PointPreview
              routeID={currentRouteID}
              point={currentPoint}
              confirmations={currentConfirmations}
            />
          )}
        </Modal>
      )}

      {toVerification && (
        <Modal visible={denyModalVisible} onClose={closeDenyModal}>
          <DenyForm onSubmit={(e) => e.preventDefault()}>
            <Label>Powód odrzucenia</Label>
            <Input id="deny-reason" name="deny-reason" ref={denyRef} />
            <FormButtons>
              <Button danger onClick={denyTrip}>
                Odrzuć
              </Button>
              <Button onClick={closeDenyModal}>Anuluj</Button>
            </FormButtons>
          </DenyForm>
        </Modal>
      )}

      <StyledContainer>
        <Heading margin="none" as="h2">
          {trip.name}
        </Heading>
        {/* <Text as="button" onClick={() => navigate(-1)}> */}
        <Text as="button" onClick={() => null}>
          Powrót
        </Text>

        <InfoHeading>Informacje</InfoHeading>
        <GridInfo>
          <Data>
            <Bold>Turysta:</Bold>
            <Text>{trip.owner}</Text>
          </Data>
          {(trip as VerificationTrip)?.location && (
            <Data>
              <Bold>Lokalizacja:</Bold>
              <Text>{(trip as VerificationTrip).location}</Text>
            </Data>
          )}
          {(trip as VerificationTrip)?.startDate && (
            <Data>
              <Bold>Data rozpoczęcia:</Bold>
              <Text>
                {(trip as VerificationTrip).startDate.toLocaleDateString('pl')}
              </Text>
            </Data>
          )}
          {(trip as VerificationTrip)?.endDate && (
            <Data>
              <Bold>Data zakończenia:</Bold>
              <Text>
                {(trip as VerificationTrip).endDate.toLocaleDateString('pl')}
              </Text>
            </Data>
          )}

          <Data>
            <Bold>Zdobyte punkty:</Bold>
            <Text>
              {trip.routes.reduce(
                (acc, route) => acc + Number(route.points.to),
                0
              )}
            </Text>
          </Data>
        </GridInfo>

        {points.length > 0 && (
          <>
            <InfoHeading>Punkty terenowe</InfoHeading>
            <Info>
              <InfoList>
                {points.map((point, index) => (
                  <InfoListItem key={point.id}>
                    <Data as="button" onClick={() => openPoint(point)}>
                      <Bold>{index + 1}.</Bold>
                      <Text>{point.name}</Text>
                    </Data>
                  </InfoListItem>
                ))}
              </InfoList>
            </Info>
          </>
        )}

        {toVerification && (
          <Veryfication>
            <Button accept onClick={acceptTrip}>
              Akceptuj
            </Button>
            <Button danger onClick={openDenyModal}>
              Odrzuć
            </Button>
          </Veryfication>
        )}
      </StyledContainer>
    </>
  )
}

export default TripDetails
