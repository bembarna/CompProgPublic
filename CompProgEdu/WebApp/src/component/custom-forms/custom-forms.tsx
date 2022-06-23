import React from 'react';
import { Field as RFField } from 'react-final-form';
import { Form as SUIForm } from 'semantic-ui-react';

type CustomFormInput = {
  formField: string;
  formName: string;
  placeHolder?: string;
  type?: string;
  icon?: string;
  error?: string;
};

type CustomFormCheckbox = {
  formField: string;
  formName: string;
  type?: string;
  error?: string;
};

export const LoginFormInput = (formProps: CustomFormInput) => {
  return (
    <>
      <RFField name={formProps.formField}>
        {(props) => (
          <SUIForm.Input
            name={props.input.name}
            value={props.input.value}
            onChange={props.input.onChange}
            fluid
            icon={formProps.icon}
            iconPosition="left"
            placeholder={formProps.placeHolder}
            type={formProps.type}
            error={formProps.error}
          />
        )}
      </RFField>
    </>
  );
};

export const LoginFormCheckBox = (formProps: CustomFormCheckbox) => {
  return (
    <>
      <RFField name={formProps.formField} type="checkbox">
        {(props) => (
          <div style={{ display: 'flex', flexDirection: 'row' }}>
            <SUIForm.Input
              style={{ marginTop: '3.5px' }}
              name={props.input.name}
              value={props.input.value}
              onChange={props.input.onChange}
              error={formProps.error}
              type="checkbox"
            />
            <label style={{ marginLeft: 10 }}>{formProps.formName}</label>
          </div>
        )}
      </RFField>
    </>
  );
};
