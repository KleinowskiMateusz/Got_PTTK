import styled from 'styled-components'
import tw from 'twin.macro'
import { Text } from 'components/shared/typography'

export const Form = styled.form`
  ${tw`flex flex-col m-0`}
`

export const Label = styled(Text)``

export const Input = styled.input`
  ${tw` mb-3 p-1 bg-gray-200 rounded-sm text-xs w-full`}
`

export const Select = styled(Input).attrs({ as: 'select' })``
