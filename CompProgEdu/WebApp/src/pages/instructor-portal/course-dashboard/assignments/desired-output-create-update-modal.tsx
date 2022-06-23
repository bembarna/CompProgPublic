import React from 'react';
import { Form } from 'react-final-form';
import { useToasts } from 'react-toast-notifications';
import { useAsync } from 'react-use';
import { Modal, Form as SUIForm, Divider, Button } from 'semantic-ui-react';
import { DeleteButton } from '../../../../component/custom-delete-button';
import { RFFInput } from '../../../../component/forms';
import { DesiredOutputsService, ErrorResponse } from '../../../../swagger';

type DesiredOutputCreateModal = {
  assignmentId: number;
  count: number;
  modalState: boolean;
  setModalState: (value: boolean) => void;
};

export const DesiredOutputCreateModal = (props: DesiredOutputCreateModal) => {
  const [errorList, setErrorList] = React.useState<
    ErrorResponse[] | undefined
  >();
  const { addToast } = useToasts();

  const onSubmit = async (values: any) => {
    try {
      const response = await DesiredOutputsService.create({
        body: {
          ...values,
          assignmentId: Number(props.assignmentId),
          order: props.count,
        },
      });

      if (response.errors.length > 0) {
        setErrorList(response.errors);
      } else {
        addToast('Successfully Created Desired Output!', {
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
        onClose={() => props.setModalState(false)}
        size="tiny"
      >
        <Modal.Header>Create Desired Output</Modal.Header>
        <Modal.Content>
          <Form
            onSubmit={onSubmit}
            render={({ handleSubmit }) => (
              <>
                <SUIForm onSubmit={handleSubmit}>
                  <DesiredOutputFormFields errorList={errorList} />
                  <Divider />
                  <Modal.Actions>
                    <Button
                      type="submit"
                      primary
                      fluid
                      style={{ margin: 0 }}
                      //loading={onSubmitState.loading}
                    >
                      Create Desired Output
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

type DesiredOutputUpdateModal = {
  outputId: number | undefined;
  modalState: boolean;
  setModalState: (value: boolean) => void;
};

export const DesiredOutputUpdateModal = (props: DesiredOutputUpdateModal) => {
  const [errorList, setErrorList] = React.useState<
    ErrorResponse[] | undefined
  >();
  const { addToast } = useToasts();

  const fetchDesiredOutput = useAsync(async () => {
    if (props.outputId) {
      const response = await DesiredOutputsService.getById({
        id: Number(props.outputId),
      });
      return response;
    }
  }, [props.modalState, props.outputId]);

  const desiredOutput = fetchDesiredOutput.value;

  const onSubmit = async (values: any) => {
    try {
      const response = await DesiredOutputsService.update({
        body: {
          ...values,
        },
      });

      if (response.errors.length > 0) {
        setErrorList(response.errors);
      } else {
        addToast('Successfully Updated Desired Output!', {
          appearance: 'success',
        });
        props.setModalState(false);
      }
    } catch {
      addToast('Error! Something went wrong', { appearance: 'error' });
      props.setModalState(false);
    }
  };

  const onDelete = async () => {
    try {
      const response = await DesiredOutputsService.delete({
        id: props.outputId,
      });

      if (response.errors.length > 0) {
        setErrorList(response.errors);
      } else {
        addToast('Successfully Deleted Desired Output!', {
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
        onClose={() => props.setModalState(false)}
        size="tiny"
        style={{ paddingBottom: 20 }}
      >
        <Modal.Header>Update Desired Output</Modal.Header>
        <Modal.Content>
          <Form
            onSubmit={onSubmit}
            initialValues={desiredOutput?.result}
            render={({ handleSubmit }) => (
              <>
                <SUIForm onSubmit={handleSubmit}>
                  <DesiredOutputFormFields errorList={errorList} />
                  <Divider />
                  <Modal.Actions>
                    <div style={{ float: 'right' }}>
                      <DeleteButton
                        buttonText="Delete Desired Output"
                        event={onDelete}
                      />
                      <Button type="submit" color="blue">
                        Update Desired Output
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

type DesiredOutputFormFields = {
  errorList: ErrorResponse[] | undefined;
};

const DesiredOutputFormFields = (props: DesiredOutputFormFields) => {
  return (
    <>
      <RFFInput
        fieldName="input"
        label="Input"
        style={inputStyle}
        SIError={
          props.errorList?.find((x) => x.fieldName === 'Input')?.error ||
          undefined
        }
      />
      <RFFInput
        fieldName="output"
        label="Output"
        style={inputStyle}
        SIError={
          props.errorList?.find((x) => x.fieldName === 'Output')?.error ||
          undefined
        }
      />

      <RFFInput
        fieldName="pointValue"
        label="Point Value"
        number
        style={inputStyle}
        SIError={
          props.errorList?.find((x) => x.fieldName === 'PointValue')?.error ||
          undefined
        }
      />
    </>
  );
};

const inputStyle = {
  marginBottom: 10,
};
