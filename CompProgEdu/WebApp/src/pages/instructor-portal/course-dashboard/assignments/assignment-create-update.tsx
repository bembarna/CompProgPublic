import moment from 'moment';
import React, { useEffect } from 'react';
import { useState } from 'react';
import { Form } from 'react-final-form';
import { useHistory, useParams } from 'react-router-dom';
import { useToasts } from 'react-toast-notifications';
import {
  Modal,
  Form as SUIForm,
  Divider,
  Button,
  Dropdown,
} from 'semantic-ui-react';
import { DeleteButton } from '../../../../component/custom-delete-button';
import {
  RFFDatePicker,
  RFFInput,
  RFFTextArea,
} from '../../../../component/forms';
import {
  AssignmentDetailDto,
  AssignmentsService,
  ErrorResponse,
} from '../../../../swagger';
import { languages } from './languages';

type CreateAssignmentModal = {
  courseId: number;
  modalState: boolean;
  setModalState: (value: boolean) => void;
};

type DropdownItem = {
  key: string;
  value: string;
  text: string;
};

export const AssignmentCreateModal = (props: CreateAssignmentModal) => {
  const [errorList, setErrorList] = useState<ErrorResponse[] | undefined>();
  const [allowedLanguages, setAllowedLanguages] = useState<string>();
  const { addToast } = useToasts();

  const updateAllowedLanguages = (data: any) => {
    const selectedLanguages = data.value as string[];
    setAllowedLanguages(
      languages
        .filter((x) => selectedLanguages.includes(x.value))
        .map((x) => x.text)
        .join(', ')
    );
  };

  const onSubmit = async (values: any) => {
    try {
      console.log(values);
      const response = await AssignmentsService.create({
        body: {
          courseId: props.courseId,
          allowedLanguages: allowedLanguages,
          ...values,
        },
      });

      if (response.errors.length > 0) {
        setErrorList(response.errors);
      } else {
        addToast('Successfully Created Assignment!', {
          appearance: 'success',
        });
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
        <Modal.Header>Create Assignment</Modal.Header>
        <Modal.Content>
          <Form
            onSubmit={onSubmit}
            render={({ handleSubmit, hasSubmitErrors }) => (
              <>
                <SUIForm onSubmit={handleSubmit} error={hasSubmitErrors}>
                  <AssignmentFields errorList={errorList} />
                  <label>Allowed Languages</label>
                  <Dropdown
                    options={languages}
                    multiple
                    selection
                    onChange={(e, d) => updateAllowedLanguages(d)}
                    style={{ marginBottom: 10, width: '100%' }}
                  />
                  <Divider />
                  <Modal.Actions>
                    <Button type="submit" primary fluid style={{ margin: 0 }}>
                      Create Assignment
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

type UpdateAssignmentModal = {
  assignment: AssignmentDetailDto | undefined;
  modalState: boolean;
  setModalState: (value: boolean) => void;
};

export const AssignmentUpdateModal = (props: UpdateAssignmentModal) => {
  const [allowedLanguages, setAllowedLanguages] = useState<string>();
  const [errorList, setErrorList] = useState<ErrorResponse[] | undefined>();
  const { courseId } = useParams() as any;
  const { addToast } = useToasts();
  const history = useHistory();

  const updateAllowedLanguages = (data: any) => {
    const selectedLanguages = data.value as string[];
    setAllowedLanguages(
      languages
        .filter((x) => selectedLanguages.includes(x.value))
        .map((x) => x.text)
        .join(', ')
    );
  };

  const allowedLanguagesArray = props.assignment?.allowedLanguages.split(', ');
  const languageOptions = languages.filter((x) =>
    allowedLanguagesArray?.includes(x.text)
  );

  useEffect(() => {
    setErrorList(undefined);
    setAllowedLanguages(props.assignment?.allowedLanguages as string);
  }, [props.assignment, props.modalState]);

  const onSubmit = async (values: any) => {
    try {
      const response = await AssignmentsService.update({
        body: {
          id: Number(props.assignment?.id),
          courseId: props.assignment?.courseId,
          allowedLanguages: allowedLanguages,
          ...values,
        },
      });

      if (response.errors.length > 0) {
        setErrorList(response.errors);
      } else {
        addToast('Successfully Updated Assignment!', {
          appearance: 'success',
        });
        props.setModalState(false);
      }
    } catch {
      addToast('Error! Something went wrong', { appearance: 'error' });
    }
  };

  const onDelete = async () => {
    try {
      const response = await AssignmentsService.delete({
        id: props.assignment?.id ?? 0,
      });

      if (response.errors.length > 0) {
        setErrorList(response.errors);
      } else {
        addToast('Successfully Deleted Assignment', { appearance: 'success' });
        history.push(`/instructor/${courseId}/course-dashboard/1`);
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
        <Modal.Header>Update Assignment</Modal.Header>
        <Modal.Content>
          <Form
            onSubmit={onSubmit}
            render={({ handleSubmit, hasSubmitErrors }) => (
              <>
                <SUIForm onSubmit={handleSubmit} error={hasSubmitErrors}>
                  <AssignmentFields
                    errorList={errorList}
                    assignment={props.assignment}
                  />
                  <label>Allowed Languages</label>
                  <Dropdown
                    options={languages}
                    multiple
                    selection
                    onChange={(e, d) => updateAllowedLanguages(d)}
                    style={{ marginBottom: 10, width: '100%' }}
                    //TODO: Fix this error stuff.
                    // error={
                    //   errorList?.find((x) => x.fieldName === 'AllowedLanguages')
                    //     ?.error || undefined
                    // }
                    defaultValue={languageOptions.map((x) => x.value)}
                  />
                  <Divider />
                  <Modal.Actions>
                    <div style={{ float: 'right', marginBottom: 14 }}>
                      <DeleteButton
                        buttonText="Delete Assignment"
                        event={onDelete}
                      />
                      <Button type="submit" color="green">
                        Update Assignment
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

type CourseFields = {
  errorList: ErrorResponse[] | undefined;
  assignment?: AssignmentDetailDto;
  allowedLanguages?: DropdownItem[] | undefined;
};

export const AssignmentFields = (props: CourseFields) => {
  return (
    <>
      <RFFInput
        fieldName="assignmentName"
        label="Assignment Name"
        style={inputStyle}
        SIError={
          props.errorList?.find((x) => x.fieldName === 'AssignmentName')
            ?.error || undefined
        }
        defaultValue={props.assignment?.assignmentName}
      />
      <RFFTextArea
        fieldName="assignmentInstructions"
        label="Instructions"
        style={inputStyle}
        SIError={
          props.errorList?.find((x) => x.fieldName === 'AssignmentInstructions')
            ?.error || undefined
        }
        defaultValue={props.assignment?.assignmentInstructions}
      />

      <RFFInput
        fieldName="exampleInput"
        label="Example Input"
        style={inputStyle}
        SIError={
          props.errorList?.find((x) => x.fieldName === 'ExampleInput')?.error ||
          undefined
        }
        defaultValue={props.assignment?.exampleInput}
      />
      <RFFInput
        fieldName="exampleOutput"
        label="Example Output"
        style={inputStyle}
        SIError={
          props.errorList?.find((x) => x.fieldName === 'ExampleOutput')
            ?.error || undefined
        }
        defaultValue={props.assignment?.exampleOutput}
      />
      <RFFInput
        fieldName="totalPointValue"
        label="Total Point Value"
        style={inputStyle}
        number
        SIError={
          props.errorList?.find((x) => x.fieldName === 'TotalPointValue')
            ?.error || undefined
        }
        defaultValue={props.assignment?.totalPointValue as any}
      />
      <RFFDatePicker
        fieldName="dueDate"
        label="Due Date"
        style={inputStyle}
        SIError={
          props.errorList?.find((x) => x.fieldName === 'DueDate')?.error ||
          undefined
        }
        defaultValue={
          props.assignment?.dueDate &&
          moment(props.assignment?.dueDate).subtract('hours', 6).toDate()
        }
      />
      <RFFDatePicker
        fieldName="visibilityDate"
        label="Visibility Date"
        style={inputStyle}
        SIError={
          props.errorList?.find((x) => x.fieldName === 'VisibilityDate')
            ?.error || undefined
        }
        defaultValue={
          props.assignment?.visibilityDate &&
          moment(props.assignment?.visibilityDate).subtract('hours', 6).toDate()
        }
      />
    </>
  );
};

const inputStyle = {
  width: '100%',
  marginBottom: 10,
};
