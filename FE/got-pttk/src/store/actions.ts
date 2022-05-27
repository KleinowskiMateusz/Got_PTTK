import { StoreState } from './reducer'
import { USER_LOGIN, USER_LOGOUT } from './types'

type Action<Type, Payload = undefined> = {
  type: Type
  payload: Payload
}

type UserLoginAction = Action<
  typeof USER_LOGIN,
  Exclude<StoreState['userType'], null>
>
type UserLogoutAction = Action<typeof USER_LOGOUT>

export type Actions = UserLoginAction | UserLogoutAction

export const loginUser = (
  userType: UserLoginAction['payload']
): UserLoginAction => ({
  type: USER_LOGIN,
  payload: userType,
})

export const logoutUser = (): UserLogoutAction => ({
  type: USER_LOGOUT,
  payload: undefined,
})
