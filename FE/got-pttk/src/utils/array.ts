/* eslint-disable import/prefer-default-export */
export const getLast = <T>(array: T[]): T | null =>
  array.length === 0 ? null : array[array.length - 1]
