/* eslint-disable import/prefer-default-export */
import axios from 'axios'
import {
  ROUTES_URL,
  PRIVATE_ROUTES_URL,
  POINTS_URL,
  TRIPS_URL,
  ADJUSTED_ROUTES_URL,
  VERIFICATION_TRIPS_URL,
  VERIFICATION_TRIP_URL,
  MOUNTAIN_RANGES_URL,
  VERIFICATION_URL,
  CONFIRMATION_URL,
  CONFIRMATION_ADD_URL,
} from '../constants/api'
import { RoutePostData, TripPostData, VerificationPostData } from '../types/api'
import { MountainRange, Point, Route, VerificationTrip } from '../types/gotpttk'
import {
  parseMountainRange,
  parsePoint,
  parseRoute,
  parseVerificationTrip,
} from '../utils/parseApi'

export const getPoints = async () => {
  const result = await axios.get(POINTS_URL)
  const points: Point[] = result.data.map(parsePoint)
  return points
}

export const getRoutes = async () => {
  const result = await axios.get(ROUTES_URL)
  const routes: Route[] = result.data.map((route: any) => parseRoute(route))
  return routes
}

export const uploadRoute =
  (data: RoutePostData, isPrivate = false) =>
  async () => {
    const result = isPrivate
      ? await axios.post(ROUTES_URL, data)
      : await axios.post(PRIVATE_ROUTES_URL, {
          ...data,
          wlasciciel: 'Turysta1',
        })
    return result
  }

export const updateRoute = (id: string, data: RoutePostData) => async () => {
  const result = await axios.put(`${ROUTES_URL}/${id}`, data)
  return result
}

export const uploadTrip = (data: TripPostData) => async () => {
  const result = await axios.post(TRIPS_URL, data)
  return result
}

export const verifyTrip = (data: VerificationPostData) => async () => {
  const result = await axios.post(VERIFICATION_URL, data)
  return result
}

export const getAdjustedRoutes = (pointID: string) => async () => {
  const result = await axios.get(`${ADJUSTED_ROUTES_URL}/${pointID}`)
  const routes: Route[] = result.data.map((route: any) =>
    parseRoute(route, route.powrot)
  )
  return routes
}

export const getVerificationTrips = async () => {
  const result = await axios.get(VERIFICATION_TRIPS_URL)
  const trips: VerificationTrip[] = result.data.map(parseVerificationTrip)
  return trips
}

export const getVerificationTrip = (id: string) => async () => {
  const result = await axios.get(`${VERIFICATION_TRIP_URL}/${id}`)
  const trip: VerificationTrip = parseVerificationTrip(result.data)
  return trip
}

export const getMountainRanges = async () => {
  const result = await axios.get(MOUNTAIN_RANGES_URL)
  const ranges: MountainRange[] = result.data.map(parseMountainRange)
  return ranges
}

export const addConfirmation = (data: FormData) => async () => {
  const result = await axios.post(CONFIRMATION_ADD_URL, data, {
    headers: {
      accept: '*/*',
      'Content-Type': 'multipart/form-data',
    },
  })
  return result
}

export const removeConfirmation = (id: string) => async () => {
  const result = await axios.delete(`${CONFIRMATION_URL}/${id}`)
  return result
}
