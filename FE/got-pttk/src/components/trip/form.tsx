import React, { useState, useRef, useMemo } from 'react'
import styled from 'styled-components'
import tw from 'twin.macro'
import { Formik } from 'formik'

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

const TripForm: React.FC<Props> = ({ trip }) => {
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
        </Form>
      )}
    </Formik>
  )
}

export default TripForm
