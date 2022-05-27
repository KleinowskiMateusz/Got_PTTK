export type MountainRange = {
  id: string
  name: string
}

export type Point = {
  id: string
  name: string
  lat: number
  lng: number
  mnpm: number
}

export type Route = {
  id: string
  name: string
  direction: string
  from: Point
  to: Point
  mountainRange: string
  points: {
    from: number
    to: number
  }
  owned: boolean
  return: boolean
}

export type Confirmation = {
  id: string
  type: 'qr' | 'image'
  src: string
  pointID: Point['id']
  routeID?: Route['id']
  date: Date
}

export type Trip = {
  id: string
  name: string
  status: string
  routes: Route[]
  owner: string | null
}

export type VerificationTrip = Trip & {
  startDate: Date
  endDate: Date
  location: string
  confirmations: Confirmation[]
}
