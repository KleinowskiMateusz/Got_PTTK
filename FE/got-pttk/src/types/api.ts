export type RoutePostData = {
  name: string
  points: number
  pointsBack: number
  fromId: number
  targetId: number
  mountainRangeId: number
}

export type TripPostData = {
  touristsBookId: string
  name: string
  segments: { order: number; segmentId: number; isBack: boolean }[]
}

export type VerificationPostData = {
  wycieczka: number
  przodownik: string
  zaakceptiowana: boolean
  powodOdrzucenia: string | null
}

export type ConfirmationPostData = {
  terrainPointId: number
  segmentId: number
  url?: ''
  image: string
}
