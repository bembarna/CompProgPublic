/* eslint-disable jsx-a11y/anchor-is-valid */
import React, { useState } from 'react';
import { Form as RFForm } from 'react-final-form';
import {
  Form as SUIForm,
  Message,
  Segment,
  Button,
  Header,
} from 'semantic-ui-react';
import './login-layout.css';
import {
  LoginFormCheckBox,
  LoginFormInput,
} from '../../component/custom-forms/custom-forms';
import { loginUser } from '../../actions/userAction';
import { useHistory } from 'react-router-dom';

const RFFormFields = () => {
  return (
    <>
      <LoginFormInput
        formName="E-mail address"
        formField="email"
        placeHolder="E-mail address"
        icon="mail"
      />
      <LoginFormInput
        formName="Password"
        formField="password"
        placeHolder="Password"
        type="password"
        icon="lock"
      />
      <LoginFormCheckBox
        formName="Remember Me"
        formField="stayLoggedIn"
        type="checkbox"
      />
    </>
  );
};

export const LoginForm = () => {
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string>();
  const [authenticated, setAuthenticated] = useState<boolean>(false);

  const history = useHistory();

  const loginSubmit = async (values: any) => {
    setLoading(true);
    var result = await loginUser(values);

    if (result.errors.length > 0) {
      setError(result.errors[0].errorMessage);
      setLoading(false);
      return;
    }

    setError('');
    setAuthenticated(true);
    setLoading(false);
  };

  return (
    <>
      <RFForm
        onSubmit={loginSubmit}
        render={({ handleSubmit }) => (
          <SUIForm onSubmit={handleSubmit} loading={loading} size="large">
            <Segment stacked>
              <RFFormFields />
              <Button type="submit" color="blue" fluid size="large">
                Login
              </Button>
              <Header as="h5" style={{ paddingTop: '10px', margin: 6 }}>
                Forgot <a href="#">Username</a> / <a href="#">Password</a> ?
              </Header>
              {!error ? null : (
                <>
                  <Message>{error}</Message>
                </>
              )}
              {!error && authenticated && history.go(0)}
            </Segment>
          </SUIForm>
        )}
      />
    </>
  );
};
