import React, { useState } from 'react';
import { Form } from 'react-final-form';
import { useHistory } from 'react-router-dom';
import { useToasts } from 'react-toast-notifications';
import {
  Container,
  Divider,
  Header,
  Segment,
  Form as SUIForm,
  Button,
} from 'semantic-ui-react';
import { RFFInput } from '../../component/forms';
import { UserContext } from '../../contexts/UserContext';
import { paths } from '../../routing/paths';
import { ErrorResponse, StudentsService } from '../../swagger';

export const StudentChangePassword = () => {
  const userContext = UserContext();
  const [errorList, setErrorList] = useState<ErrorResponse[] | undefined>();
  const history = useHistory();
  const { addToast } = useToasts();

  const onSubmit = async (values: any) => {
    try {
      const response = await StudentsService.updatePassword({
        id: Number(userContext.studentId),
        body: {
          ...values,
        },
      });

      if (response.errors.length > 0) {
        setErrorList(response.errors);
      } else {
        history.replace(paths.student.selectCourse);
        history.go(0);
      }
    } catch {
      addToast('Error! Something went wrong', { appearance: 'error' });
    }
  };

  return (
    <>
      <Container style={styles.container}>
        <Segment>
          <Header as="h1">Change Password</Header>
          <Divider />
          <div style={styles.welcomeMessage}>
            Welcome to CompProgEdu! Please change your password in order to
            start.
          </div>
          <Form
            onSubmit={onSubmit}
            render={({ handleSubmit, hasSubmitErrors }) => (
              <>
                <SUIForm onSubmit={handleSubmit} error={hasSubmitErrors}>
                  <RFFInput
                    fieldName="newPassword"
                    label="New Password"
                    labelStyle={styles.label}
                    style={styles.newPassword}
                    SIError={
                      errorList?.find((x) => x.fieldName === 'NewPassword')
                        ?.error || undefined
                    }
                    defaultValue=""
                    type="password"
                  />
                  <RFFInput
                    fieldName="confirmPassword"
                    label="Confirm New Password"
                    labelStyle={styles.label}
                    style={styles.confirmPassword}
                    SIError={
                      errorList?.find((x) => x.fieldName === 'ConfirmPassword')
                        ?.error || undefined
                    }
                    defaultValue=""
                    type="password"
                  />
                  <div style={{ width: '100%', textAlign: 'right' }}>
                    <Button color="black" type="submit" style={styles.submit}>
                      Change Password
                    </Button>
                  </div>
                </SUIForm>
              </>
            )}
          />
        </Segment>
      </Container>
    </>
  );
};

const styles = {
  container: {
    marginTop: 40,
    width: 500,
  },
  welcomeMessage: {
    marginBottom: 30,
  },
  newPassword: {
    width: '100%',
  },
  label: {
    marginTop: 20,
  },
  confirmPassword: {
    width: '100%',
  },
  submit: {
    marginTop: 20,
  },
};
