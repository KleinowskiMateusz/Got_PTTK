import React from 'react'
// import styled from 'styled-components'
// import tw from 'twin.macro'
import { Formik } from 'formik'

import { Form, Label, Input } from '../../components/shared/form'
import Button from '../../components/shared/button'

import { addConfirmation } from '../../requests/api'

import { Confirmation, Point, Route } from '../../types/gotpttk'
import { parseConfirmation } from '../../utils/parseApi'

type FormDataTypes = {
  file: File | null
}

type Props = {
  pointID: Point['id']
  routeID: Route['id']
  onAdd: (confirmation: Confirmation) => void
}

const ImageForm: React.FC<Props> = ({ routeID, pointID, onAdd }) => {
  const initialValues: FormDataTypes = {
    file: null,
  }

  return (
    <Formik
      initialValues={initialValues}
      onSubmit={async (values, { setSubmitting }) => {
        // eslint-disable-next-line no-console
        if (values.file) {
          const data = new FormData()
          data.append('Image', values.file)
          data.append('PunktId', pointID)
          data.append('OdcinekId', routeID)
          data.append('Url', '')
          try {
            const result = await addConfirmation(data)()
            onAdd(parseConfirmation(result.data, routeID))
          } catch (error: any) {
            console.error(error, error.response)
          }
        }

        setSubmitting(false)
      }}
    >
      {({ handleSubmit, setFieldValue }) => (
        <Form onSubmit={handleSubmit}>
          <Label>Nazwa</Label>
          <Input
            onChange={(e) => {
              if (e?.target?.files?.length && e?.target?.files[0])
                setFieldValue('file', e?.target?.files[0])
            }}
            name="file"
            type="file"
          />

          <Button primary type="submit">
            Zapisz
          </Button>
        </Form>
      )}
    </Formik>
  )
}

export default ImageForm
