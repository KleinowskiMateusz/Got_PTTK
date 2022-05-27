import { Actions } from './actions'
import { USER_LOGIN, USER_LOGOUT } from './types'

export interface StoreState {
  userType: 'tourist' | 'leader' | 'worker' | null
}

const initialState: StoreState = {
  userType: null,
}

const reducer = (state = initialState, action: Actions): StoreState => {
  switch (action.type) {
    case USER_LOGIN:
      return {
        ...state,
        userType: action.payload,
      }

    case USER_LOGOUT:
      return { ...state, userType: null }

    default:
      return state
  }
}

export default reducer
