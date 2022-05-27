/* eslint-disable import/prefer-default-export */
export const includes = (s1: string, s2: string): boolean =>
  s1.toLowerCase().includes(s2.toLowerCase()) ||
  s2.toLowerCase().includes(s1.toLowerCase())
