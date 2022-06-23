import React from 'react';
import { Link, useParams } from 'react-router-dom';
import { useAsyncRetry } from 'react-use';
import { Container, Header, Icon, Tab } from 'semantic-ui-react';
import { AssignmentsService } from '../../../../swagger';
import { InstructorAssignmentDetails } from './assignment-details';
import { DesiredOutputs } from './desired-outputs';
import { TestCases } from './test-cases';

export const AssignmentTabs = () => {
  const { assignmentId, courseId } = useParams() as any;

  const fetchAssignment = useAsyncRetry(async () => {
    const response = await AssignmentsService.getById({ id: assignmentId });
    return response.result;
  }, [assignmentId]);

  const assignment = fetchAssignment.value;

  return (
    <>
      <Container style={{ width: '45%', minWidth: 500 }}>
        <div style={{ display: 'flex', flexDirection: 'row' }}>
          <Link
            to={`/instructor/${courseId}/course-dashboard/1`}
            style={{
              fontWeight: 'bold',
              fontSize: 16,
              marginBottom: 0,
              float: 'left',
            }}
          >
            <Icon name="caret left" />
            Back to Assignments
          </Link>
          {assignment?.totalPointValue && assignment.totalPointsAssigned ? (
            <Header
              style={
                assignment?.totalPointsAssigned >= assignment?.totalPointValue
                  ? styles.positivePoints
                  : styles.negativePoints
              }
            >
              Total Points Assigned: {assignment?.totalPointsAssigned}/
              {assignment?.totalPointValue}
            </Header>
          ) : (
              <Header style={styles.negativePoints}>
                Total Points Assigned: 0/
                {assignment?.totalPointValue}
              </Header>
            )}
        </div>
        <Tab
          style={{
            backgroundColor: 'white',
            padding: '10px',
            borderRadius: '5px',
            marginTop: 10,
          }}
          panes={[
            {
              menuItem: 'Details',
              render: () => (
                <InstructorAssignmentDetails
                  refetchAssignment={() => fetchAssignment.retry()}
                />
              ),
            },
            {
              menuItem: 'Desired Outputs',
              render: () => (
                <DesiredOutputs
                  refetchAssignment={() => fetchAssignment.retry()}
                />
              ),
            },
            {
              menuItem: 'Test Cases',
              render: () => <TestCases refetchAssignment={() => fetchAssignment.retry()} assignment={assignment} />,
            },
          ]}
        />
      </Container>
    </>
  );
};

const styles = {
  positivePoints: {
    color: '#00e275',
    margin: '0 0 0 auto',
  },
  negativePoints: {
    color: '#ff4b54',
    margin: '0 0 0 auto',
  },
};
