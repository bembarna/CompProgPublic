import { ErrorResponse, InstructorsService } from '../../../../swagger';
import { RFFInput } from '../../../../component/forms';
import React from 'react';
import { Form } from 'react-final-form';
import { useToasts } from 'react-toast-notifications';
import { Modal, Divider, Form as SUIForm, Button } from 'semantic-ui-react';
import { useAsyncFn } from 'react-use';

type CreateStudentModal = {
  modalState: boolean;
  courseId: number;
  setModalState: (value: boolean) => void;
};

export const CreateStudentModal = (props: CreateStudentModal) => {
  const [errorList, setErrorList] = React.useState<
    ErrorResponse[] | undefined
  >();
  const { addToast } = useToasts();

  const [onSubmitState, onSubmit] = useAsyncFn(async (values: any) => {
    try {
      const response = await InstructorsService.addStudentToCourse({
        body: {
          courseId: props.courseId,
          ...values,
        },
      });

      if (response.errors.length > 0) {
        setErrorList(response.errors);
      } else {
        addToast('Successfully Added Student to Course!', {
          appearance: 'success',
        });
        props.setModalState(false);
      }
    } catch {
      addToast('Error! Something went wrong', { appearance: 'error' });
    }
  });

  return (
    <>
      <Modal
        open={props.modalState}
        onClose={() => {
          props.setModalState(false);
          setErrorList(undefined);
        }}
        size="tiny"
      >
        <Modal.Header>Create Student</Modal.Header>
        <Modal.Content>
          <Form
            onSubmit={onSubmit}
            render={({ handleSubmit }) => (
              <>
                <SUIForm onSubmit={handleSubmit}>
                  <StudentCreateRFFormFields errorList={errorList} />
                  <Divider />
                  <Modal.Actions>
                    <Button
                      type="submit"
                      primary
                      fluid
                      style={{ margin: 0 }}
                      loading={onSubmitState.loading}
                    >
                      Create Student
                    </Button>
                  </Modal.Actions>
                </SUIForm>
              </>
            )}
          />
        </Modal.Content>
      </Modal>
    </>
  );
};

type StudentCreate = {
  errorList: ErrorResponse[] | undefined;
};

const StudentCreateRFFormFields = (props: StudentCreate) => {
  return (
    <>
      <RFFInput
        fieldName="firstName"
        label="First Name"
        style={inputStyle}
        SIError={
          props.errorList?.find((x) => x.fieldName === 'FirstName')?.error ||
          undefined
        }
      />
      <RFFInput
        fieldName="lastName"
        label="Last Name"
        style={inputStyle}
        SIError={
          props.errorList?.find((x) => x.fieldName === 'LastName')?.error ||
          undefined
        }
      />
      <RFFInput
        fieldName="emailAddress"
        label="Email Address"
        style={inputStyle}
        SIError={
          props.errorList?.find((x) => x.fieldName === 'EmailAddress')?.error ||
          undefined
        }
      />
      <RFFInput
        fieldName="studentSchoolNumber"
        label="Student School Number"
        style={inputStyle}
        SIError={
          props.errorList?.find((x) => x.fieldName === 'StudentSchoolNumber')
            ?.error || undefined
        }
      />
    </>
  );
};

const inputStyle = {
  width: '100%',
  marginBottom: 10,
};
