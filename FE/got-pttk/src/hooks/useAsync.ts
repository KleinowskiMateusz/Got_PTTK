import { useCallback, useEffect, useState } from 'react'

type Request<T> = () => Promise<T>

type Status = 'idle' | 'pending' | 'success' | 'error'

export default function useAsync<T>(request: Request<T>, immediate = true) {
  const [status, setStatus] = useState<Status>('idle')
  const [value, setValue] = useState<T | null>(null)
  const [error, setError] = useState<any>(null)

  const execute = useCallback(() => {
    setStatus('pending')
    setValue(null)
    setError(null)
    return request()
      .then((response) => {
        setValue(response)
        setStatus('success')
      })
      .catch((err) => {
        setError(err)
        setStatus('error')
      })
  }, [request])

  useEffect(() => {
    if (immediate) {
      execute()
    }
  }, [execute, immediate])

  return { execute, status, value, error }
}
