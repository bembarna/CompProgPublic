import moment from 'moment';
import React, { useState } from 'react';
import { useParams } from 'react-router-dom';
import { useAsync } from 'react-use';
import { AssignmentUpdateModal } from './assignment-create-update';
import { AssignmentSolutionUploadModal } from './assignment-solution-upload-modal';
import { LoadingWrapper } from '../../../../component/loading-wrapper';
import { Button, Header, Icon, Table } from 'semantic-ui-react';
import { AssignmentsService } from '../../../../swagger';

type InstructorAssignmentDetails = {
  refetchAssignment: any;
};

export const InstructorAssignmentDetails = (
  props: InstructorAssignmentDetails
) => {
  const { assignmentId } = useParams() as any;
  const [updateModal, setUpdateModal] = useState<boolean>(false);
  const [uploadModal, setUploadModal] = useState<boolean>(false);

  const fetchAssignment = useAsync(async () => {
    const response = await AssignmentsService.getById({ id: assignmentId });
    if (props.refetchAssignment) {
      props.refetchAssignment();
    }
    return response.result;
  }, [assignmentId, updateModal, uploadModal]);

  const assignment = fetchAssignment.value;

  return (
    <>
      <div style={{ marginTop: 20 }}>
        <LoadingWrapper loading={fetchAssignment.loading}>
          <div style={{ display: 'flex', flexDirection: 'row' }}>
            <Header as="h1">{assignment?.assignmentName}</Header>
            <Button
              style={{ marginLeft: 'auto', height: 35 }}
              primary
              onClick={() => {
                setUpdateModal(true);
              }}
            >
              <Icon name="edit" />
              Edit
            </Button>
          </div>
          <Table basic="very">
            <Table.Row>
              <Table.Cell width="5">Due Date:</Table.Cell>
              <Table.Cell>
                {assignment?.dueDate
                  ? moment(assignment.dueDate)
                      .tz('Pacific/Majuro')
                      .add(-1, 'day')
                      .format('MM/DD/YYYY h:mm A')
                  : 'N/A'}
              </Table.Cell>
            </Table.Row>
            <Table.Row>
              <Table.Cell>Visibility Date:</Table.Cell>
              <Table.Cell>
                {assignment?.visibilityDate
                  ? moment(assignment.visibilityDate)
                      .tz('Pacific/Majuro')
                      .add(-1, 'day')
                      .format('MM/DD/YYYY h:mm A')
                  : 'N/A'}
              </Table.Cell>
            </Table.Row>
            <Table.Row>
              <Table.Cell>Total Point Value:</Table.Cell>
              <Table.Cell>{`${assignment?.totalPointValue} points`}</Table.Cell>
            </Table.Row>
            <Table.Row>
              <Table.Cell>Allowed Languages:</Table.Cell>
              <Table.Cell>{assignment?.allowedLanguages}</Table.Cell>
            </Table.Row>
            <Table.Row>
              <Table.Cell>Instructions:</Table.Cell>
              <Table.Cell>
                {assignment?.assignmentInstructions ?? 'N/A'}
              </Table.Cell>
            </Table.Row>
            <Table.Row>
              <Table.Cell>Example Input:</Table.Cell>
              <Table.Cell>{assignment?.exampleInput}</Table.Cell>
            </Table.Row>
            <Table.Row>
              <Table.Cell>Example Output:</Table.Cell>
              <Table.Cell>{assignment?.exampleOutput}</Table.Cell>
            </Table.Row>
            <Table.Row>
              <Table.Cell>Solution File:</Table.Cell>
              <Table.Cell>
                {assignment?.assignmentSolutionFileName ? (
                  <>
                    <>
                      <Button
                        size="small"
                        primary
                        onClick={() => setUploadModal(true)}
                      >
                        <Icon name="edit" />
                        {assignment.assignmentSolutionFileName}
                      </Button>
                    </>
                  </>
                ) : (
                  <>
                    <Button
                      size="small"
                      primary
                      onClick={() => setUploadModal(true)}
                    >
                      <Icon name="plus" />
                      Add Solution File
                    </Button>
                  </>
                )}
              </Table.Cell>
            </Table.Row>
          </Table>
        </LoadingWrapper>
      </div>
      <AssignmentUpdateModal
        assignment={assignment}
        modalState={updateModal}
        setModalState={(value: boolean) => setUpdateModal(value)}
      />
      <AssignmentSolutionUploadModal
        assignment={assignment}
        modalState={uploadModal}
        setModalState={(value: boolean) => setUploadModal(value)}
      />
    </>
  );
};
