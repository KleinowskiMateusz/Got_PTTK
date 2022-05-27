import React from 'react'
// import styled from 'styled-components'
// import tw from 'twin.macro'
import { Formik } from 'formik'

import { Form, Label, Input } from '../../components/shared/form'
import Button from '../../components/shared/button'
import { Point, Route } from '../../types/gotpttk'

type FormDataTypes = {
  url: string
}

type Props = {
  pointID: Point['id']
  routeID: Route['id']
}

const QRForm: React.FC<Props> = ({ routeID, pointID }) => {
  const initialValues: FormDataTypes = {
    url: '',
  }

  const parseDataToAPI = (form: FormDataTypes) => ({
    terrainPointId: pointID,
    segmentId: routeID,
    url: form.url,
  })

  return (
    <Formik
      initialValues={initialValues}
      onSubmit={(values, { setSubmitting }) => {
        setTimeout(() => {
          // eslint-disable-next-line no-console
          console.log(parseDataToAPI(values))
          setSubmitting(false)
        }, 400)
      }}
    >
      {({ values, handleChange, handleSubmit }) => (
        <Form onSubmit={handleSubmit}>
          <Label>Url</Label>
          <Input onChange={handleChange} value={values.url} name="url" />

          <Button primary>Zapisz</Button>
        </Form>
      )}
    </Formik>
  )
}

export default QRForm
