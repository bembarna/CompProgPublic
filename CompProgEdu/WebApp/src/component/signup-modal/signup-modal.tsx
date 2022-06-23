/* eslint-disable no-redeclare */
/* eslint-disable jsx-a11y/anchor-is-valid */
import React from 'react';
import {
  Button,
  Modal,
  Form as SUIForm,
  Container,
  Segment,
  Grid,
  Message,
} from 'semantic-ui-react';
import { LoginFormInput } from '../custom-forms/custom-forms';
import { AuthenticationService, ErrorResponse } from '../../swagger';
import { Form as RFForm } from 'react-final-form';
import { useToasts } from 'react-toast-notifications';

function SignupModal() {
  const { addToast } = useToasts();
  const [open, setOpen] = React.useState(false);
  const [errorList, setErrorList] = React.useState<
    ErrorResponse[] | undefined
  >();

  const getErrorList = () => {
    return errorList;
  };

  // need to wait for registration, then fill in props
  const signup = async (values: any) => {
    try {
      var result = await AuthenticationService.registerInstructor({
        body: values,
      });

      if (result.errors.length > 0) {
        setErrorList(result.errors);
      } else {
        addToast('Successfully registered!', { appearance: 'success' });
        setOpen(false);
      }
    } catch {
      addToast('Error! Something went wrong', { appearance: 'error' });
    }
  };

  return (
    <Modal
      size="tiny"
      closeIcon
      open={open}
      trigger={
        <Button type="submit" color="blue" fluid size="large">
          Sign Up
        </Button>
      }
      onClose={() => setOpen(false)}
      onOpen={() => {
        setOpen(true);
        setErrorList(undefined);
      }}
    >
      <Modal.Header>Instructor Sign Up</Modal.Header>
      <Modal.Content width="90">
        <Container textAlign="left" fluid responsive>
          <InstructorSignUpTab
            getErrorList={() => {
              return getErrorList();
            }}
            signUp={signup}
          />
        </Container>
      </Modal.Content>
    </Modal>
  );
}

type RegisterUser = {
  signUp: (values: any) => void;
  getErrorList: () => ErrorResponse[] | undefined;
};

const InstructorSignUpTab = (props: RegisterUser) => {
  return (
    <Segment vertical>
      <Grid relaxed container stackable verticalAlign="middle">
        <Grid.Column textAlign="center">
          <Message
            attached
            header="Glad you are interested in signing up!"
            content="Please fill out the fields below to begin creating your courses."
          />
          <RFForm
            onSubmit={props.signUp}
            render={({ handleSubmit }) => (
              <SUIForm onSubmit={handleSubmit} size="large">
                <Segment stacked>
                  <InstructorRFFormFields binglist={props.getErrorList()} />
                  <Button type="submit" color="blue" fluid size="large">
                    Complete Signup
                  </Button>
                </Segment>
              </SUIForm>
            )}
          />
        </Grid.Column>
      </Grid>
    </Segment>
  );
};

type bingeelist = {
  binglist: ErrorResponse[] | undefined;
};

const InstructorRFFormFields = (bingeelist: bingeelist) => {
  return (
    <>
      <LoginFormInput
        formName="Title"
        formField="title"
        placeHolder="Title"
        icon="info circle"
      />
      <LoginFormInput
        formName="First Name"
        formField="firstName"
        placeHolder="First Name"
        icon="user"
        error={
          bingeelist?.binglist?.find((x) => x.fieldName === 'FirstName')
            ?.error || undefined
        }
      />
      <LoginFormInput
        formName="Last Name"
        formField="lastName"
        placeHolder="Last Name"
        icon="user"
        error={
          bingeelist?.binglist?.find((x) => x.fieldName === 'LastName')
            ?.error || undefined
        }
      />
      <LoginFormInput
        formName="E-mail address"
        formField="emailAddress"
        placeHolder="E-mail address"
        icon="mail"
        error={
          bingeelist?.binglist?.find((x) => x.fieldName === 'EmailAddress')
            ?.error || undefined
        }
      />
      <LoginFormInput
        formName="Password"
        type="password"
        formField="password"
        placeHolder="Password"
        icon="unlock alternate"
        error={
          bingeelist?.binglist?.find((x) => x.fieldName === 'Password')
            ?.error || undefined
        }
      />
      <LoginFormInput
        formName="Password"
        type="password"
        formField="confirmPassword"
        placeHolder="Confirm password"
        icon="lock"
        error={
          bingeelist?.binglist?.find((x) => x.fieldName === 'ConfirmPassword')
            ?.error || undefined
        }
      />
    </>
  );
};

export default SignupModal;
