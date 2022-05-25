import styled, { css } from 'styled-components'
import tw from 'twin.macro'

import loadingIcon from 'assets/icons/loading.svg'

type ButtonProps = {
  className?: string
  primary?: boolean
  accept?: boolean
  danger?: boolean
  loading?: boolean
}

const Button = styled.button<ButtonProps>`
  ${tw`px-4 py-2 rounded-sm text-sm cursor-pointer ease-in-out duration-150 text-center`}

  ${({ primary, accept, danger }) => {
    if (primary) return tw`bg-blue-500 text-white hover:bg-blue-600`
    if (accept) return tw`bg-green-500 text-white hover:bg-green-600`
    if (danger) return tw`bg-red-500 text-white hover:bg-red-600`
    return tw`bg-gray-500 text-white hover:bg-gray-600`
  }}

  ${({ loading }) =>
    loading &&
    css`
      opacity: 0.7;
      cursor: wait;

      &::before {
        content: '';
        position: absolute;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;

        background: inherit;
      }

      &::after {
        content: '';
        position: absolute;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;

        background-image: url(${loadingIcon});
        background-size: auto 75%;
        background-position: center;
        background-repeat: no-repeat;
      }
    `}

  &[disabled] {
    opacity: 0.6;
    cursor: not-allowed;
  }
`

export default Button
