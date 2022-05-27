/* eslint-disable import/prefer-default-export */
import {
  MountainRange,
  Confirmation,
  Point,
  Route,
  Trip,
  VerificationTrip,
} from '../types/gotpttk'

export const parseMountainRange = (apiMountainRange: any): MountainRange => ({
  id: apiMountainRange.id,
  name: apiMountainRange.name,
})

export const parseConfirmation = (
  apiConfirmation: any,
  routeID: Route['id'] | undefined = undefined
): Confirmation => ({
  id: apiConfirmation.id,
  type: apiConfirmation.type === 1 ? 'image' : 'qr',
  src: apiConfirmation.url,
  date: new Date(apiConfirmation.date),
  pointID: apiConfirmation.terrainPointId,
  routeID,
})

export const parsePoint = (apiPoint: any): Point => ({
  id: apiPoint.id,
  lat: apiPoint.lat,
  lng: apiPoint.lng,
  mnpm: apiPoint.mnpm,
  name: apiPoint.name,
})

export const parseRoute = (apiRoute: any, reverse = false): Route => {
  const parsedToPoint = parsePoint(apiRoute.target)
  const parsedFromPoint = parsePoint(apiRoute.from)

  return {
    id: apiRoute.id,
    name: apiRoute.name,
    direction: `Z ${reverse ? parsedToPoint.name : parsedFromPoint.name} do ${
      reverse ? parsedFromPoint.name : parsedToPoint.name
    }`,
    points: {
      to: reverse ? apiRoute.points : apiRoute.pointsBack,
      from: reverse ? apiRoute.pointsBack : apiRoute.points,
    },
    from: reverse ? parsedToPoint : parsedFromPoint,
    to: reverse ? parsedFromPoint : parsedToPoint,
    mountainRange: apiRoute.mountainRange.name,
    owned: Boolean(apiRoute.touristsBookId),
    return: Boolean(reverse),
  }
}

export const parseTrip = (apiTrip: any): Trip => ({
  id: apiTrip.id,
  name: apiTrip.name,
  status: String(apiTrip.status),
  owner: apiTrip.touristsBook.owner,
  routes:
    apiTrip.segments.map((route: any) =>
      parseRoute(route.segment, route.isBack)
    ) || [],
})

export const parseVerificationTrip = (
  apiVerificationTrip: any
): VerificationTrip => ({
  startDate: new Date(apiVerificationTrip.dataPoczatkowa),
  endDate: new Date(apiVerificationTrip.dataKoncowa),
  location: apiVerificationTrip.lokalizacja,
  confirmations: apiVerificationTrip.wycieczka.odcinki
    ? apiVerificationTrip.wycieczka.odcinki.reduce(
        (acc: Confirmation[], route: any) => [
          ...acc,
          ...(route.potwierdzenia
            ? route.potwierdzenia.map((confirmation: any) =>
                parseConfirmation(
                  confirmation.potwierdzenieTerenowe,
                  route.segment.id
                )
              )
            : []),
        ],
        []
      )
    : [],
  ...parseTrip(apiVerificationTrip.wycieczka),
})
