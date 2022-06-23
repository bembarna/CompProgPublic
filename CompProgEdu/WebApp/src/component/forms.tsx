import moment from 'moment';
import React, { useState } from 'react';
import DatePicker from 'react-datepicker';
import 'react-datepicker/dist/react-datepicker.css';
import { Field } from 'react-final-form';
import './forms.css';
import {
  DropdownItemProps,
  DropdownProps,
  Form as SUIForm,
  StrictInputProps,
} from 'semantic-ui-react';

type inputType = StrictInputProps & {
  placeholder?: string;
  fieldName: string;
  style?: {};
  labelStyle?: {};
  number?: boolean;
  SIError?: string;
  defaultValue?: string;
};

type textAreaType = StrictInputProps & {
  placeholder?: string;
  fieldName: string;
  style?: {};
  labelStyle?: {};
  number?: boolean;
  SIError?: string;
  defaultValue?: string;
};

type checkBoxType = {
  label?: string;
  fieldName: string;
  style?: {};
  labelStyle?: {};
  toggle?: boolean;
};

type datePickerType = {
  label?: string;
  fieldName: string;
  labelStyle?: {};
  style?: {};
  SIError?: string;
  defaultValue?: any;
};

type dropdownType = DropdownProps & {
  label?: string;
  placeholder?: string;
  fieldName: string;
  selection?: boolean;
  SIError?: string;
  style?: {};
  labelStyle?: {};
  options: DropdownItemProps[];
  defaultValue?: string;
};

export const RFFInput = (config: inputType) => {
  return (
    <>
      <div>
        <div style={config.labelStyle}>
          <label>{config.label}</label>
        </div>
        <Field
          name={config.fieldName}
          defaultValue={config.defaultValue}
          parse={(val) => (config.number ? Number(val) : val)}
        >
          {(props) => (
            <SUIForm.Input
              {...config}
              name={props.input.name}
              value={props.input.value}
              onChange={props.input.onChange}
              style={config.style}
              placeholder={config.placeholder}
              label=""
              error={config.SIError}
            />
          )}
        </Field>
      </div>
    </>
  );
};

export const RFFTextArea = (config: textAreaType) => {
  return (
    <>
      <div>
        <label style={config.labelStyle}>{config.label}</label>
        <Field
          defaultValue={config.defaultValue}
          name={config.fieldName}
          parse={(val) => (config.number ? Number(val) : val)}
        >
          {(props) => (
            <div>
              <SUIForm.TextArea
                {...config}
                input={props.input.name}
                value={props.input.value}
                onChange={props.input.onChange}
                style={config.style}
                placeholder={config.placeholder}
                label=""
                error={config.SIError}
              />
            </div>
          )}
        </Field>
      </div>
    </>
  );
};

export const RFFCheckbox = (config: checkBoxType) => {
  return (
    <>
      <div>
        <label style={config.labelStyle}>{config.label}</label>
        <Field name={config.fieldName}>
          {(props) => (
            <div>
              <SUIForm.Checkbox
                input={props.input.name}
                onChange={props.input.onChange}
                style={config.style}
                toggle={config.toggle}
              />
            </div>
          )}
        </Field>
      </div>
    </>
  );
};

export const RFFDropdown = (config: dropdownType) => {
  return (
    <>
      <div>
        <label style={config.labelStyle}>{config.label}</label>
        <Field name={config.fieldName} defaultValue={config.defaultValue}>
          {(props) => (
            <div>
              <SUIForm.Dropdown
                {...config}
                label=""
                input={props.input.name}
                onChange={props.input.onChange}
                style={config.style}
                selection={config.selection}
                placeholder={config.placeholder}
                options={config.options}
                error={config.SIError}
              />
            </div>
          )}
        </Field>
      </div>
    </>
  );
};

export const RFFDatePicker = (config: datePickerType) => {
  const [date, setDate] = useState<any>(config.defaultValue);

  return (
    <>
      <div style={config.style}>
        <label style={config.labelStyle}>{config.label}</label>
        <Field name={config.fieldName} defaultValue={date} value={date}>
          {(props) => (
            <div>
              <DatePicker
                className="date-picker"
                name={props.input.name}
                value={date}
                selected={date}
                onChange={(x) => setDate(x)}
                showTimeSelect
                injectTimes={[moment().hours(23).minutes(59).toDate()]}
                dateFormat="MMMM d, yyyy h:mm aa"
                autoComplete="off"
              />
            </div>
          )}
        </Field>
      </div>
    </>
  );
};
