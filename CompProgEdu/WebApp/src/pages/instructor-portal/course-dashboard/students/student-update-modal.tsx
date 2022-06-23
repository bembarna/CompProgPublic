import {
  ErrorResponse,
  StudentResponseDto,
  StudentsService,
} from '../../../../swagger';
import { RFFInput } from '../../../../component/forms';
import React, { useState } from 'react';
import { useToasts } from 'react-toast-notifications';
import { Form } from 'react-final-form';
import { Modal, Divider, Form as SUIForm, Button } from 'semantic-ui-react';
import { DeleteButton } from '../../../../component/custom-delete-button';

type UpdateStudentModal = {
  modalState: boolean;
  setModalState: (value: boolean) => void;
  currentStudent: StudentResponseDto | undefined;
};

export const UpdateStudentModal = (props: UpdateStudentModal) => {
  const [errorList, setErrorList] = useState<ErrorResponse[] | undefined>();
  const { addToast } = useToasts();

  const onSubmit = async (values: any) => {
    try {
      const response = await StudentsService.update({
        id: props.currentStudent?.id ?? 0,
        body: {
          ...values,
        },
      });

      if (response.errors.length > 0) {
        setErrorList(response.errors);
      } else {
        addToast('Successfully Updated Student!', { appearance: 'success' });
        props.setModalState(false);
      }
    } catch {
      addToast('Error! Something went wrong', { appearance: 'error' });
    }
  };

  const onDelete = async () => {
    try {
      const response = await StudentsService.delete({
        id: props.currentStudent?.id ?? 0,
      });

      if (response.errors.length > 0) {
        setErrorList(response.errors);
      } else {
        addToast('Successfully Deleted Student', { appearance: 'success' });
        props.setModalState(false);
      }
    } catch {
      addToast('Error! Something went wrong', { appearance: 'error' });
    }
  };

  return (
    <>
      <Modal
        open={props.modalState}
        onClose={() => {
          props.setModalState(false);
          setErrorList(undefined);
        }}
        size="tiny"
        style={{ height: '375px' }}
      >
        <Modal.Header>Update Student</Modal.Header>
        <Modal.Content>
          <Form
            initialValues={props.currentStudent}
            onSubmit={onSubmit}
            render={({ handleSubmit }) => (
              <>
                <SUIForm onSubmit={handleSubmit}>
                  <StudentUpdateRFFormFields
                    errorList={errorList}
                    initialValues={props.currentStudent}
                  />
                  <Divider />
                  <Modal.Actions>
                    <div style={{ float: 'right' }}>
                      <DeleteButton
                        buttonText="Delete Student"
                        event={onDelete}
                      />
                      <Button type="submit" color="blue">
                        Update Student
                      </Button>
                    </div>
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

type StudentUpdate = {
  errorList: ErrorResponse[] | undefined;
  initialValues?: StudentResponseDto;
};

const StudentUpdateRFFormFields = (props: StudentUpdate) => {
  return (
    <>
      <RFFInput
        fieldName="firstName"
        label="First Name"
        style={inputStyle}
        defaultValue={props.initialValues?.firstName}
        SIError={
          props.errorList?.find((x) => x.fieldName === 'FirstName')?.error ||
          undefined
        }
      />
      <RFFInput
        fieldName="lastName"
        label="Last Name"
        style={inputStyle}
        defaultValue={props.initialValues?.lastName}
        SIError={
          props.errorList?.find((x) => x.fieldName === 'LastName')?.error ||
          undefined
        }
      />
      <RFFInput
        fieldName="studentSchoolNumber"
        label="Student School Number"
        defaultValue={props.initialValues?.studentSchoolNumber}
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
