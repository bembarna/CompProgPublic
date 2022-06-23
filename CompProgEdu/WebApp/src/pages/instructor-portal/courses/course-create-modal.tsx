import React from 'react';
import { Modal, Button, Divider, Form as SUIForm } from 'semantic-ui-react';
import { Form } from 'react-final-form';
import { RFFInput } from '../../../component/forms';
import {
  CourseService,
  ErrorResponse,
  CourseDetailDto,
} from '../../../swagger';
import { UserContext } from '../../../contexts/UserContext';
import { useToasts } from 'react-toast-notifications';

type CreateCourseModal = {
  modalState: boolean;
  setModalState: (value: boolean) => void;
};

export const CreateCourseModal = (props: CreateCourseModal) => {
  const [errorList, setErrorList] = React.useState<
    ErrorResponse[] | undefined
  >();
  const { addToast } = useToasts();

  const instructor = UserContext();

  const onSubmit = async (values: any) => {
    try {
      const response = await CourseService.create({
        body: {
          instructorId: instructor.instructorId,
          ...values,
        },
      });

      if (response.errors.length > 0) {
        setErrorList(response.errors);
      } else {
        addToast('Successfully Created Course!', { appearance: 'success' });
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
      >
        <Modal.Header>Create Course</Modal.Header>
        <Modal.Content>
          <Form
            onSubmit={onSubmit}
            render={({ handleSubmit, hasSubmitErrors }) => (
              <>
                <SUIForm onSubmit={handleSubmit} error={hasSubmitErrors}>
                  <CourseRFFormFields errorList={errorList} />
                  <Divider />
                  <Modal.Actions>
                    <Button type="submit" primary fluid style={{ margin: 0 }}>
                      Create Course
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

type CourseFields = {
  errorList: ErrorResponse[] | undefined;
  course?: CourseDetailDto;
};

export const CourseRFFormFields = (props: CourseFields) => {
  return (
    <>
      <RFFInput
        fieldName="title"
        label="Course Name"
        style={inputStyle}
        SIError={
          props.errorList?.find((x) => x.fieldName === 'Title')?.error ||
          undefined
        }
        defaultValue={props.course?.title}
      />
      <RFFInput
        fieldName="section"
        label="Course Section"
        style={inputStyle}
        SIError={
          props.errorList?.find((x) => x.fieldName === 'Section')?.error ||
          undefined
        }
        defaultValue={props.course?.section}
      />
    </>
  );
};

const inputStyle = {
  width: '100%',
  marginBottom: 10,
};
