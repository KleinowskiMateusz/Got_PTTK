import { useSelector } from 'react-redux'
import { StoreState } from '../store/reducer'

export default function useUser() {
  return useSelector<StoreState, StoreState['userType']>(
    (state) => state.userType
  )
}
