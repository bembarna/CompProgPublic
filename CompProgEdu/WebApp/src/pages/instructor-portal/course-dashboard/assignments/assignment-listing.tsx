import moment from 'moment-timezone';
import React, { useState } from 'react';
import { Link } from 'react-router-dom';
import { useAsync } from 'react-use';
import {
  Button,
  Dropdown,
  Header,
  Pagination,
  Popup,
  Table,
} from 'semantic-ui-react';
import { LoadingWrapper } from '../../../../component/loading-wrapper';
import { pageSizeOptions } from '../../../../component/pagination-footer';
import { AssignmentsService } from '../../../../swagger';
import { AssignmentCreateModal } from './assignment-create-update';

type AssignmentListing = {
  courseId: number;
};

export const InstructorAssignmentListing = (props: AssignmentListing) => {
  const [createModal, setCreateModal] = useState<boolean>(false);
  const [page, setPage] = useState<number>(1);
  const [pageSize, setPageSize] = useState<number>(10);

  const fetchAssignments = useAsync(async () => {
    const response = await AssignmentsService.getAllAssignmentsByCourseId({
      courseId: props.courseId,
      page: page - 1,
      pageSize: pageSize,
    });
    return response;
  }, [props.courseId, createModal, page, pageSize]);

  const assignments = fetchAssignments.value?.result;

  return (
    <>
      <Table>
        <Table.Header>
          <Table.HeaderCell>Name</Table.HeaderCell>
          <Table.HeaderCell>Due Date</Table.HeaderCell>
          <Table.HeaderCell>Total Point Value</Table.HeaderCell>
          <Table.HeaderCell>Visible Date</Table.HeaderCell>
          <Table.HeaderCell width="1">
            <Popup
              content="Add New Assignment"
              trigger={
                <Button primary onClick={() => setCreateModal(true)}>
                  +
                </Button>
              }
            />
          </Table.HeaderCell>
        </Table.Header>

        <LoadingWrapper loading={fetchAssignments.loading}>
          {assignments?.length ? (
            <>
              {assignments?.map((x) => (
                <>
                  <Table.Body>
                    <Table.Row key={x.id}>
                      <Table.Cell>{x.assignmentName}</Table.Cell>
                      <Table.Cell>
                        {x.dueDate
                          ? moment(x.dueDate)
                              .tz('Pacific/Majuro')
                              .add(-1, 'day')
                              .format('MM/DD/YYYY h:mm A')
                          : 'N/A'}
                      </Table.Cell>
                      <Table.Cell>{x.totalPointValue}</Table.Cell>
                      <Table.Cell>
                        {x.visibilityDate
                          ? moment(x.visibilityDate)
                              .tz('Pacific/Majuro')
                              .add(-1, 'day')
                              .format('MM/DD/YYYY h:mm A')
                          : 'Not Visible'}
                      </Table.Cell>
                      <Table.Cell>
                        <Button
                          secondary
                          as={Link}
                          to={`/instructor/${props.courseId}/assignment-details/${x.id}`}
                          size="small"
                        >
                          Details
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
                <Table.Cell>
                  <Header>You Have No Assignments</Header>
                </Table.Cell>
              </Table.Row>
            </Table.Body>
          )}
        </LoadingWrapper>
      </Table>
      {assignments && assignments?.length > 0 && (
        <div
          style={{
            width: '100%',
            display: 'flex',
            flexDirection: 'row',
          }}
        >
          <Pagination
            totalPages={
              fetchAssignments.value
                ? Math.ceil(fetchAssignments.value?.totalCount / pageSize)
                : 1
            }
            onPageChange={(e, d) =>
              setPage(Number(d.activePage ? Number(d.activePage) : 0))
            }
          />
          <div style={{ marginLeft: 'auto' }}>
            <span style={{ fontSize: 16 }}>Page Size: </span>
            <Dropdown
              selection
              options={pageSizeOptions}
              defaultValue={10}
              compact
              onChange={(e, d) => setPageSize(Number(d.value))}
            />
          </div>
        </div>
      )}
      <AssignmentCreateModal
        courseId={props.courseId}
        modalState={createModal}
        setModalState={(value: boolean) => setCreateModal(value)}
      />
    </>
  );
};
