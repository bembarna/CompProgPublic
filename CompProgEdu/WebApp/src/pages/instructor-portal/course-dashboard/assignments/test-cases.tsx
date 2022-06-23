import React, { useState } from 'react';
import { Form } from 'react-final-form';
import TreeMenu from 'react-simple-tree-menu';
import { useToasts } from 'react-toast-notifications';
import { useAsync } from 'react-use';
import {
  Header,
  Button,
  Icon,
  Form as SUIForm,
  Modal,
  Divider,
  Table,
} from 'semantic-ui-react';
import { DeleteButton } from '../../../../component/custom-delete-button';
import { RFFInput, RFFTextArea } from '../../../../component/forms';
import {
  AssignmentDetailDto,
  ErrorResponse,
  InstructorSubmissionsService,
  MethodTestCaseGetDto,
  MethodTestCaseService,
} from '../../../../swagger';

type TestCases = {
  assignment: AssignmentDetailDto | undefined;
  refetchAssignment: any;
};

export const TestCases = (props: TestCases) => {
  const [nodeKey, setNodeKey] = useState<string>();
  const [createModal, setCreateModal] = useState<boolean>(false);
  const [updateModal, setUpdateModal] = useState<boolean>(false);
  const [selectedTest, setSelectedTest] = useState<number>();
  const [testCaseModal, setTestCaseModal] = useState<boolean>(false);

  const fetchTestCaseTree = useAsync(async () => {
    const response = await InstructorSubmissionsService.getTreeNodes({
      assignmentId: props.assignment?.id,
    });
    return response.result;
  }, [props.assignment?.id]);

  const fetchTestCases = useAsync(async () => {
    const response = await MethodTestCaseService.getByAssignmentId({
      assignmentId: props.assignment?.id || 0,
    });
    return response;
  }, [props.assignment, createModal, updateModal]);

  const testCases = fetchTestCases.value?.result;

  const testCaseTree = fetchTestCaseTree.value;

  return (
    <>
      <div style={{ marginTop: 20 }}>
        <div style={{ display: 'flex', flexDirection: 'row' }}>
          <Header as="h1">Test Cases</Header>
          <Button
            style={{ marginLeft: 'auto', height: 35 }}
            color="black"
            disabled={!props.assignment?.assignmentSolutionFileName}
            onClick={() => setTestCaseModal(true)}
          >
            <Icon name="plus" />
            Add Test Cases
          </Button>
        </div>
        <Table>
          <Table.Header>
            <Table.Row>
              <Table.HeaderCell>Test Signature</Table.HeaderCell>
              <Table.HeaderCell>Parm Inputs</Table.HeaderCell>
              <Table.HeaderCell>Output</Table.HeaderCell>
              <Table.HeaderCell>Points</Table.HeaderCell>
              <Table.HeaderCell width="1" />
            </Table.Row>
          </Table.Header>

          {testCases?.length !== 0 ? (
            <>
              {testCases?.map((x) => (
                <>
                  <Table.Body>
                    <Table.Row key={x.id}>
                      <Table.Cell>{x.methodTestInjectable}</Table.Cell>
                      <Table.Cell>{x.paramInputs}</Table.Cell>
                      <Table.Cell>{x.output}</Table.Cell>
                      <Table.Cell>{x.pointValue}</Table.Cell>
                      <Table.Cell>
                        <Button
                          color="black"
                          onClick={() => {
                            setSelectedTest(x.id);
                            setUpdateModal(true);
                          }}
                        >
                          Edit
                        </Button>
                      </Table.Cell>
                    </Table.Row>
                  </Table.Body>
                </>
              ))}
            </>
          ) : (
              <Table.Body>
                <Table.Row>
                  <Table.Cell colSpan="4">
                    <Header>You Currently Have No Test Cases</Header>
                  </Table.Cell>
                </Table.Row>
              </Table.Body>
            )}
        </Table>
      </div>
      <Modal open={testCaseModal} onClose={() => setTestCaseModal(false)}>
        <Modal.Header>Add Test Case</Modal.Header>
        <Modal.Content>
          <TreeMenu
            data={testCaseTree}
            onClickItem={({ key, label }) => {
              var lastNodeType = key.split('/');
              var lastNodeType2 = lastNodeType[lastNodeType.length - 1].trim().split('-')[0].trim();
              if (lastNodeType2 === "method") {
                setNodeKey(key);
                setCreateModal(true)
              }
            }}
          />
        </Modal.Content>
      </Modal>
      <TestMethodCaseCreateModal
        refetchAssignment={props.refetchAssignment}
        assignmentId={props.assignment?.id || 0}
        modalState={createModal}
        methodNodeKey={nodeKey || ''}
        setTestModalState={(value: boolean) => setTestCaseModal(value)}
        setModalState={(value: boolean) => setCreateModal(value)}
      />
      <TestMethodCaseUpdateModal
        methodTestId={selectedTest}
        refetchAssignment={props.refetchAssignment}
        modalState={updateModal}
        setModalState={(value: boolean) => setUpdateModal(value)}
      />
    </>
  );
};

type CreateMethodTestCaseModal = {
  assignmentId: number;
  methodNodeKey: string;
  modalState: boolean;
  setModalState: (value: boolean) => void;
  setTestModalState: (value: boolean) => void;
  refetchAssignment: any;
};

export const TestMethodCaseCreateModal = (props: CreateMethodTestCaseModal) => {
  const [errorList, setErrorList] = useState<ErrorResponse[] | undefined>();
  const { addToast } = useToasts();

  const onSubmit = async (values: any) => {
    try {
      values.methodTestCaseDto.assignmentId = props.assignmentId;
      values.methodNodeKey = props.methodNodeKey;
      console.log(values);
      const response = await MethodTestCaseService.create({
        body: {
          ...values,
        },
      });

      if (response.errors.length > 0) {
        setErrorList(response.errors);
      } else {
        addToast('Successfully Created Method Test Case!', {
          appearance: 'success',
        });
        props.refetchAssignment();
        props.setModalState(false);
        props.setTestModalState(false);
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
        <Modal.Header>Create Method Test Case</Modal.Header>
        <Modal.Content>
          <Form
            onSubmit={onSubmit}
            render={({ handleSubmit, hasSubmitErrors }) => (
              <>
                <SUIForm onSubmit={handleSubmit} error={hasSubmitErrors}>
                  <MethodTestCaseFields errorList={errorList} />
                  <Divider />
                  <Modal.Actions>
                    <Button type="submit" primary fluid style={{ margin: 0 }}>
                      Create Method Test Case
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

type TestMethodCaseUpdateModal = {
  methodTestId: number | undefined;
  modalState: boolean;
  setModalState: (value: boolean) => void;
  refetchAssignment: any;
};

export const TestMethodCaseUpdateModal = (props: TestMethodCaseUpdateModal) => {
  const [errorList, setErrorList] = React.useState<
    ErrorResponse[] | undefined
  >();
  const { addToast } = useToasts();

  const fetchMethodTest = useAsync(async () => {
    if (props.methodTestId) {
      const response = await MethodTestCaseService.getById({
        id: Number(props.methodTestId),
      });
      return response;
    }
  }, [props.modalState, props.methodTestId]);

  const methodTestCase = fetchMethodTest.value;

  const onSubmit = async (values: any) => {
    try {
      values.id = Number(props.methodTestId);
      const response = await MethodTestCaseService.update({
        body: {
          ...values,
        },
      });

      if (response.errors.length > 0) {
        setErrorList(response.errors);
      } else {
        addToast('Successfully Updated Method Test Case!', {
          appearance: 'success',
        });
        props.refetchAssignment();
        props.setModalState(false);
      }
    } catch {
      addToast('Error! Something went wrong', { appearance: 'error' });
      props.setModalState(false);
    }
  };

  const onDelete = async () => {
    try {
      const response = await MethodTestCaseService.delete({
        id: props.methodTestId,
      });

      if (response.errors.length > 0) {
        setErrorList(response.errors);
      } else {
        addToast('Successfully Deleted Method Test Case!', {
          appearance: 'success',
        });
        props.refetchAssignment();
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
        <Modal.Header>Update Method Test Case</Modal.Header>
        <Modal.Content>
          <Form
            onSubmit={onSubmit}
            render={({ handleSubmit }) => (
              <>
                <SUIForm onSubmit={handleSubmit}>
                  <MethodTestCaseUpdateFields
                    errorList={errorList}
                    methodTestCase={methodTestCase?.result}
                  />
                  <Divider />
                  <Modal.Actions>
                    <div style={{ float: 'right' }}>
                      <DeleteButton
                        buttonText="Delete Method Test Case"
                        event={onDelete}
                      />
                      <Button type="submit" color="blue">
                        Update Method Test Case
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

type MethodTestCaseFields = {
  errorList: ErrorResponse[] | undefined;
  methodTestCase?: MethodTestCaseGetDto | undefined;
};

export const MethodTestCaseFields = (props: MethodTestCaseFields) => {
  return (
    <>
      <RFFInput
        fieldName="methodTestCaseDto.paramInputs"
        label="Method Parameter Inputs"
        style={inputStyle}
        SIError={
          props.errorList?.find(
            (x) => x.fieldName === 'MethodTestCaseDto.ParamInputs'
          )?.error || undefined
        }
        defaultValue={props.methodTestCase?.paramInputs}
      />

      <RFFInput
        fieldName="methodTestCaseDto.output"
        label="Method Test Case Output"
        style={inputStyle}
        SIError={
          props.errorList?.find(
            (x) => x.fieldName === 'MethodTestCaseDto.Output'
          )?.error || undefined
        }
        defaultValue={props.methodTestCase?.output}
      />
      <RFFTextArea
        fieldName="methodTestCaseDto.hint"
        label="Hint"
        style={inputStyle}
        SIError={
          props.errorList?.find((x) => x.fieldName === 'MethodTestCaseDto.Hint')
            ?.error || undefined
        }
        defaultValue={props.methodTestCase?.hint}
      />
      <RFFInput
        fieldName="methodTestCaseDto.pointValue"
        label="Point Value"
        style={inputStyle}
        number
        SIError={
          props.errorList?.find(
            (x) => x.fieldName === 'MethodTestCaseDto.PointValue'
          )?.error || undefined
        }
        defaultValue={0 as any}
      />
    </>
  );
};

type MethodTestCaseUpdateFields = {
  errorList: ErrorResponse[] | undefined;
  methodTestCase?: MethodTestCaseGetDto | undefined;
};

export const MethodTestCaseUpdateFields = (
  props: MethodTestCaseUpdateFields
) => {
  return (
    <>
      <RFFInput
        fieldName="paramInputs"
        label="Method Parameter Inputs"
        style={inputStyle}
        SIError={
          props.errorList?.find((x) => x.fieldName === 'ParamInputs')?.error ||
          undefined
        }
        defaultValue={props.methodTestCase?.paramInputs}
      />

      <RFFInput
        fieldName="output"
        label="Method Test Case Output"
        style={inputStyle}
        SIError={
          props.errorList?.find((x) => x.fieldName === 'Output')?.error ||
          undefined
        }
        defaultValue={props.methodTestCase?.output}
      />
      <RFFTextArea
        fieldName="hint"
        label="Hint"
        style={inputStyle}
        SIError={
          props.errorList?.find((x) => x.fieldName === 'Hint')?.error ||
          undefined
        }
        defaultValue={props.methodTestCase?.hint}
      />
      <RFFInput
        fieldName="pointValue"
        label="Point Value"
        style={inputStyle}
        number
        SIError={
          props.errorList?.find((x) => x.fieldName === 'PointValue')?.error ||
          undefined
        }
        defaultValue={props.methodTestCase?.pointValue as any}
      />
    </>
  );
};

const inputStyle = {
  width: '100%',
  marginBottom: 10,
};
