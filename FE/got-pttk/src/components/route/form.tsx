import React, { useMemo, useEffect, useState } from 'react'
import styled from 'styled-components'
import { Formik } from 'formik'
import tw from 'twin.macro'

import { Form, Label, Input, Select } from '../shared/form'
import Button from '../shared/button'

import useAsync from '../../hooks/useAsync'

import { Route } from '../../types/gotpttk'
import {
  getMountainRanges,
  getPoints,
  updateRoute,
  uploadRoute,
} from '../../requests/api'
import { RoutePostData } from '../../types/api'
import {
  useJsApiLoader,
  GoogleMap,
  Marker,
  DirectionsRenderer,
} from '@react-google-maps/api'

type Props = {
  route?: Route
  isPrivate: boolean
}

type FormDataTypes = {
  name: Route['name']
  from: string | null
  to: string | null
  points: number
  pointsBack: number
  mountainRange: string | null
}
const Map = styled.div`
  ${tw`m-0 mt-2  w-full border`}
  height: 50vh;
`

const FormGrid = styled.div`
  ${tw`grid grid-cols-2 gap-4`}
`

const RouteForm: React.FC<Props> = ({ route, isPrivate }) => {
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

  const {
    value: ranges,
    error: rangesError,
    status: rangesStatus,
  } = useAsync(getMountainRanges)
  const {
    value: points,
    status: pointsStatus,
    error: pointsError,
  } = useAsync(getPoints)
  const getMountaintRangeID = (name: string): number => {
    if (!rangesError && ranges && ranges.length > 0) {
      const index = ranges?.findIndex((range) => range.name === name)
      if (index === -1) return -1
      return Number(ranges[index].id)
    }
    return -1
  }

  const mountainRangeID = useMemo<number>(
    () =>
      route?.mountainRange ? getMountaintRangeID(route.mountainRange) : -1,
    [route, route?.mountainRange, ranges]
  )

  const initialValues: FormDataTypes = route
    ? {
        name: route.name,
        from: String(route.from.id),
        to: String(route.to.id),
        points: Number(route.points.from),
        pointsBack: Number(route.points.to),
        mountainRange: String(mountainRangeID),
      }
    : {
        name: '',
        from: null,
        to: null,
        points: 0,
        pointsBack: 0,
        mountainRange: null,
      }

  const parseFormData = (data: FormDataTypes): RoutePostData => ({
    name: data.name,
    points: data.points,
    pointsBack: data.pointsBack,
    fromId: Number(data.from),
    targetId: Number(data.to),
    mountainRangeId: Number(data.mountainRange),
  })

  if (!isLoaded) {
    return null
  }

  return (
    <Formik
      initialValues={initialValues}
      enableReinitialize
      onSubmit={async (values, { setSubmitting }) => {
        try {
          const request = route
            ? updateRoute(route.id, parseFormData(values))
            : uploadRoute(parseFormData(values), isPrivate)

          const response = await request()
          console.log(response)
        } catch (error) {
          console.error(error)
        }

        alert(JSON.stringify(values, null, 2))
        setSubmitting(false)
      }}
    >
      {({ values, handleChange, handleSubmit }) => {
        return (
          <Form onSubmit={handleSubmit}>
            <Label>Nazwa</Label>
            <Input onChange={handleChange} value={values.name} name="name" />
            <Label>Pasmo</Label>
            <Select
              onChange={handleChange}
              value={values.mountainRange ?? ''}
              name="mountainRange"
              disabled={
                rangesStatus !== 'success' ||
                rangesError ||
                !ranges ||
                ranges.length === 0
              }
            >
              <option value="" selected disabled hidden>
                Wybierz pasmo
              </option>
              {ranges?.map((range) => (
                <option key={range.id} value={range.id}>
                  {range.name}
                </option>
              ))}
            </Select>
            <FormGrid>
              <div>
                <Label>Od</Label>
                <Select
                  onChange={handleChange}
                  // @ts-ignore
                  value={values.from}
                  type="number"
                  name="from"
                  disabled={
                    values.mountainRange === null ||
                    pointsError ||
                    pointsStatus === 'error' ||
                    points?.length === 0
                  }
                >
                  <option value="" selected disabled hidden>
                    Wybierz punkt startowy
                  </option>
                  {points &&
                    points
                      .filter((point) => String(point.id) !== String(values.to))
                      .map((point) => (
                        <option key={point.id} value={point.id}>
                          {point.name}
                        </option>
                      ))}
                </Select>
              </div>
              <div>
                <Label>Do</Label>
                <Select
                  onChange={handleChange}
                  // @ts-ignore
                  value={values.to}
                  type="number"
                  name="to"
                  disabled={
                    values.mountainRange === null ||
                    pointsError ||
                    pointsStatus === 'error' ||
                    points?.length === 0
                  }
                >
                  <option value="" selected disabled hidden>
                    Wybierz punkt końcowy
                  </option>
                  {points &&
                    points
                      .filter(
                        (point) => String(point.id) !== String(values.from)
                      )
                      .map((point) => (
                        <option key={point.id} value={point.id}>
                          {point.name}
                        </option>
                      ))}
                </Select>
              </div>
            </FormGrid>
            <FormGrid>
              <div>
                <Label>{'Punkty Od --> Do'}</Label>
                <Input
                  onChange={handleChange}
                  value={values.points}
                  type="number"
                  name="points"
                  disabled={values.from === null}
                  min="0"
                />
              </div>
              <div>
                <Label>{'Punkty Do --> Od'}</Label>
                <Input
                  onChange={handleChange}
                  value={values.pointsBack}
                  type="number"
                  name="pointsBack"
                  disabled={values.to === null}
                  min="0"
                />
              </div>
            </FormGrid>
            <Button primary disabled={values.name.length === 0} type="submit">
              Zapisz
            </Button>

            {isLoaded && (
              <Map>
                <GoogleMap
                  mapContainerStyle={{ width: 'width:100%', height: '100%' }}
                  center={{
                    lat:
                      (points || []).find((point) => point.id == values.from)
                        ?.lat || 40.714,

                    lng:
                      (points || []).find((point) => point.id == values.from)
                        ?.lng || -74.006,
                  }}
                  zoom={12}
                  options={{
                    streetViewControl: false,
                    mapTypeControl: false,
                    fullscreenControl: false,
                  }}
                >
                  {markerVisible && (
                    <>
                      {values.from && (
                        <Marker
                          position={{
                            lat:
                              (points || []).find(
                                (point) => point.id == values.from
                              )?.lat || 40.714,

                            lng:
                              (points || []).find(
                                (point) => point.id == values.from
                              )?.lng || -74.006,
                          }}
                        ></Marker>
                      )}
                      {values.to && (
                        <Marker
                          position={{
                            lat:
                              (points || []).find(
                                (point) => point.id == values.to
                              )?.lat || 40.714,

                            lng:
                              (points || []).find(
                                (point) => point.id == values.to
                              )?.lng || -74.006,
                          }}
                        ></Marker>
                      )}
                    </>
                  )}
                </GoogleMap>
              </Map>
            )}
          </Form>
        )
      }}
    </Formik>
  )
}

export default RouteForm
