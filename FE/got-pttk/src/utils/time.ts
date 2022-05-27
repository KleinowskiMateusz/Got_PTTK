/* eslint-disable import/prefer-default-export */
export const timeToString = (minutes: number) => {
  const h = Math.floor(minutes / 60)
  const min = Math.floor(minutes % 60)

  return `${h > 0 ? `${h}h` : ''} ${min}min`
}
