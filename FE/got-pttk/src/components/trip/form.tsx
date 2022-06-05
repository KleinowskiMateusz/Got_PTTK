import React, { useState, useRef, useMemo, useEffect } from 'react'
import styled from 'styled-components'
import tw from 'twin.macro'
import { Formik } from 'formik'
import {
  useJsApiLoader,
  GoogleMap,
  Marker,
  DirectionsRenderer,
} from '@react-google-maps/api'

import { Form, Label, Input, Select } from '../shared/form'
import Button from '../shared/button'
import { Text } from '../shared/typography'

import useAsync from '../../hooks/useAsync'
import { getRoutes, getAdjustedRoutes, uploadTrip } from '../../requests/api'
import { getLast } from '../../utils/array'

import { Route, Trip } from '../../types/gotpttk'

type Props = {
  trip?: Trip
}

type FormDataTypes = {
  name: Trip['name']
}

const RouteWrapper = styled.div`
  ${tw`flex justify-between gap-2 items-center mb-3`}
`

const SelectWrapper = styled.div`
  ${tw`flex justify-between gap-2 items-stretch mb-3`}
`

const RouteSelect = styled(Select)`
  ${tw`flex-1 m-0`}
`

const MarginLabel = styled(Label)`
  ${tw`mb-3`}
`
const Map = styled.div`
  ${tw`m-0 mt-2  w-full border`}
  height: 50vh;
`

const TripForm: React.FC<Props> = ({ trip }) => {
  const [directions, setDirections] =
    useState<google.maps.DirectionsResult | null>(null)
  const [markerVisible, setmarkerVisible] = useState(false)
  const { isLoaded } = useJsApiLoader({
    googleMapsApiKey: 'AIzaSyBM20VmQuYH_pg-oyF2V5ByzNxhWnd40LY',
  })
  useEffect(() => {
    if (isLoaded) {
      setTimeout(() => setmarkerVisible(true), 0)
    }
  }, [isLoaded])

  const refRouteSelect = useRef<HTMLInputElement | null>(null)
  const [selectedRoutes, setSelectedRoutes] = useState<Route[]>([])
  const [currentRoute, setCurrentRoute] = useState<Route | null>(null)
  const getFittingRoutes = useMemo(
    () =>
      selectedRoutes.length === 0
        ? getRoutes
        : getAdjustedRoutes(getLast(selectedRoutes)!.to.id),
    [selectedRoutes]
  )
  const { value: routes, status, error } = useAsync(getFittingRoutes)

  const initialValues: FormDataTypes = trip
    ? { name: trip.name }
    : {
        name: '',
      }

  const onRouteChange = (e: React.MouseEvent<HTMLInputElement>) => {
    setCurrentRoute(
      routes?.find((route) => `${route.id}` === `${(e.target as any).value}`) ??
        null
    )
  }

  const addRoute = (e: React.MouseEvent<HTMLButtonElement>) => {
    e.preventDefault()
    if (currentRoute) setSelectedRoutes([...selectedRoutes, currentRoute])
    setCurrentRoute(null)
  }

  const removeFirstRoute = (e: React.MouseEvent<HTMLButtonElement>) => {
    e.preventDefault()
    setSelectedRoutes([...selectedRoutes.slice(1, selectedRoutes.length)])
  }

  const removeLastRoute = (e: React.MouseEvent<HTMLButtonElement>) => {
    e.preventDefault()
    setSelectedRoutes([...selectedRoutes.slice(0, selectedRoutes.length - 1)])
  }

  const parseDataToAPI = (form: FormDataTypes, formRoutes: Route[]) => ({
    touristsBookId: 'Turysta1',
    name: form.name,
    segments: formRoutes.map((route, index) => ({
      order: index + 1,
      segmentId: Number(route.id),
      isBack: route.return,
    })),
  })

  console.log(selectedRoutes)

  if (!isLoaded) {
    return null
  }

  return (
    <Formik
      initialValues={initialValues}
      onSubmit={async (values, { setSubmitting }) => {
        try {
          const response = await uploadTrip(
            parseDataToAPI(values, selectedRoutes)
          )()
          console.log(response)
          alert('Dodano wycieczkę')
        } catch (err) {
          console.error(err)
        }
        // eslint-disable-next-line no-console
        console.log(parseDataToAPI(values, selectedRoutes))
        setSubmitting(false)
      }}
    >
      {({ values, handleChange, handleSubmit }) => (
        <Form onSubmit={handleSubmit}>
          <Label>Nazwa</Label>
          <Input onChange={handleChange} value={values.name} name="name" />

          <MarginLabel>
            Punkty:{' '}
            {selectedRoutes.reduce((acc, route) => acc + route.points.to, 0)}
          </MarginLabel>

          <Label>Odcinki</Label>
          {selectedRoutes.map((route, index) => (
            <RouteWrapper>
              <Text margin="none">
                {route.name} ({route.direction})
              </Text>
              {(index === 0 || index === selectedRoutes.length - 1) && (
                <Button
                  danger
                  onClick={index === 0 ? removeFirstRoute : removeLastRoute}
                >
                  Usuń
                </Button>
              )}
            </RouteWrapper>
          ))}

          {status === 'success' && routes && !error && (
            <>
              <SelectWrapper>
                {/* @ts-ignore */}
                <RouteSelect ref={refRouteSelect} onChange={onRouteChange}>
                  <>
                    <option value="" selected disabled hidden>
                      Wybierz odcinek
                    </option>
                    {routes.map((route) => (
                      <option key={route.id} value={route.id}>
                        {route.name} ({route.direction})
                      </option>
                    ))}
                  </>
                </RouteSelect>
                <Button primary disabled={!currentRoute} onClick={addRoute}>
                  Dodaj
                </Button>
              </SelectWrapper>
            </>
          )}

          <Button primary disabled={selectedRoutes.length === 0}>
            Zapisz
          </Button>

          {isLoaded && (
            <Map>
              <GoogleMap
                mapContainerStyle={{ width: 'width:100%', height: '100%' }}
                center={{
                  lat: selectedRoutes[0]?.from?.lat || 40.714,
                  lng: selectedRoutes[0]?.from?.lng || -74.006,
                }}
                zoom={12}
                options={{
                  streetViewControl: false,
                  mapTypeControl: false,
                  fullscreenControl: false,
                }}
              >
                {markerVisible &&
                  selectedRoutes &&
                  selectedRoutes.length > 0 &&
                  selectedRoutes.map((route, index) =>
                    index === 0 ? (
                      <>
                        <Marker
                          position={{
                            lat: route.from.lat,
                            lng: route.from.lng,
                          }}
                        />
                        <Marker
                          position={{
                            lat: route.to.lat,
                            lng: route.to.lng,
                          }}
                        />
                      </>
                    ) : (
                      <Marker
                        position={{
                          lat: route.to.lat,
                          lng: route.to.lng,
                        }}
                      />
                    )
                  )}
              </GoogleMap>
            </Map>
          )}
        </Form>
      )}
    </Formik>
  )
}

export default TripForm
