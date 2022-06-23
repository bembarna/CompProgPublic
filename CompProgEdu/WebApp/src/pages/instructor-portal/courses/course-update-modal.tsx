import {
  ErrorResponse,
  CourseDetailDto,
  CourseService,
} from '../../../swagger';
import React from 'react';
import { useToasts } from 'react-toast-notifications';
import { Form } from 'react-final-form';
import { Modal, Divider, Form as SUIForm, Button } from 'semantic-ui-react';
import { UserContext } from '../../../contexts/UserContext';
import { CourseRFFormFields } from './course-create-modal';
import { DeleteButton } from '../../../component/custom-delete-button';
import { useHistory } from 'react-router-dom';
import { paths } from '../../../routing/paths';

type UpdateCourse = {
  setModalState: (value: boolean) => void;
  modalState: boolean;
  course: CourseDetailDto | undefined;
};

export const UpdateCourseModal = (props: UpdateCourse) => {
  const [errorList, setErrorList] = React.useState<
    ErrorResponse[] | undefined
  >();
  const { addToast } = useToasts();
  const history = useHistory();

  const user = UserContext();

  const onSubmit = async (values: any) => {
    try {
      values.instructorId = user.instructorId;
      const response = await CourseService.update({
        id: props.course?.id ?? 0,
        body: values,
      });

      if (response.errors.length > 0) {
        setErrorList(response.errors);
      } else {
        addToast('Successfully Updated Course!', { appearance: 'success' });
        props.setModalState(false);
      }
    } catch {
      addToast('Error! Something went wrong', { appearance: 'error' });
    }
  };

  const onDelete = async () => {
    try {
      await CourseService.delete({
        id: Number(props.course?.id),
      });

      addToast('Successfully Deleted Course', { appearance: 'success' });
      history.push(paths.instructor.selectCourse);
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
        <Modal.Header>Update Course</Modal.Header>
        <Modal.Content>
          <Form
            onSubmit={onSubmit}
            render={({ handleSubmit, hasSubmitErrors, submitErrors }) => (
              <>
                <SUIForm onSubmit={handleSubmit} error={hasSubmitErrors}>
                  <CourseRFFormFields
                    errorList={errorList}
                    course={props.course}
                  />
                  <Divider />
                  <Modal.Actions>
                    <div style={{ float: 'right', marginBottom: 14 }}>
                      <DeleteButton
                        buttonText="Delete Course"
                        event={onDelete}
                      />
                      <Button type="submit" color="green">
                        Update Course
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
