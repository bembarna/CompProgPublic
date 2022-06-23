import React from 'react';
import { useHistory, useParams } from 'react-router-dom';
import { useAsync } from 'react-use';
import { Header, Table } from 'semantic-ui-react';
import { AssignmentsService } from '../../../../swagger';
import moment from 'moment-timezone';
import { LoadingWrapper } from '../../../../component/loading-wrapper';

export const StudentAssignmentListing = () => {
  const { courseId } = useParams() as any;
  const history = useHistory();

  const fetchAssignments = useAsync(async () => {
    const response = await AssignmentsService.getAllStudentAssignmentsByCourseId(
      { courseId }
    );
    return response.result;
  }, [courseId]);

  const assignments = fetchAssignments.value;

  return (
    <>
      <Table selectable>
        <Table.Header>
          <Table.Row>
            <Table.HeaderCell>Name</Table.HeaderCell>
            <Table.HeaderCell>Due Date</Table.HeaderCell>
            <Table.HeaderCell>Points</Table.HeaderCell>
          </Table.Row>
        </Table.Header>
        <Table.Body>
          <LoadingWrapper loading={fetchAssignments.loading}>
            {assignments?.map((x) => (
              <Table.Row
                style={{ cursor: 'pointer' }}
                onClick={() =>
                  history.push(
                    `/student/${courseId}/assignment-details/${x.id}`
                  )
                }
              >
                <Table.Cell>{x.assignmentName}</Table.Cell>
                <Table.Cell>
                  {moment(x.dueDate)
                    .tz('Pacific/Majuro')
                    .add(-1, 'day')
                    .format('MM/DD/YYYY hh:mm a')}
                </Table.Cell>
                <Table.Cell>{x.totalPointValue}</Table.Cell>
              </Table.Row>
            ))}
            {assignments?.length === 0 && (
              <Table.Row>
                <Table.Cell colSpan="4">
                  <Header>You Currently Have No Assignments</Header>
                </Table.Cell>
              </Table.Row>
            )}
          </LoadingWrapper>
        </Table.Body>
      </Table>
    </>
  );
};
