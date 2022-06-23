import React, { useState } from 'react';
import { useDropzone } from 'react-dropzone';
import { useParams } from 'react-router-dom';
import { useAsync } from 'react-use';
import {
  Table,
  Button,
  Popup,
  Header,
  Pagination,
  Dropdown,
} from 'semantic-ui-react';
import { LoadingWrapper } from '../../../../component/loading-wrapper';
import { pageSizeOptions } from '../../../../component/pagination-footer';
import {
  InstructorsService,
  StudentDetailDto,
  StudentResponseDto,
} from '../../../../swagger';
import { CreateStudentModal } from './student-create-modal';
import { UpdateStudentModal } from './student-update-modal';
import { StudentUploadModal } from './student-upload-modal';

type CourseStudentTable = {
  courseId: number;
  setStudentCount?: (value: number) => void;
};

export const CourseStudentTable = (props: CourseStudentTable) => {
  const [createStudentModal, setCreateStudentModal] = useState<boolean>(false);
  const [updateStudentModal, setUpdateStudentModal] = useState<boolean>(false);
  const [uploadStudentModal, setUploadStudentModal] = useState<boolean>(false);
  const [currentStudent, setCurrentStudent] = useState<StudentResponseDto>();
  const [loading, setLoading] = useState<boolean>(false);
  const [file, setFile] = useState<any>();
  const [studentList, setStudentList] = useState<
    StudentDetailDto[] | undefined
  >(undefined);
  const [page, setPage] = useState<number>(1);
  const [pageSize, setPageSize] = useState<number>(10);

  let { courseId } = useParams() as any;

  const { getRootProps, getInputProps } = useDropzone({
    onDrop: async (acceptedFiles: any[]) => {
      setFile(acceptedFiles[0]);
    },
  });

  const fetchStudents = useAsync(async () => {
    setLoading(true);
    const response = await InstructorsService.getCourseStudents({
      courseId: props.courseId,
      page: page - 1,
      pageSize: pageSize,
    });

    setLoading(false);
    if (props.setStudentCount) {
      props.setStudentCount(response.totalCount);
    }
    return response;
  }, [
    createStudentModal,
    updateStudentModal,
    uploadStudentModal,
    page,
    pageSize,
  ]);

  useAsync(async () => {
    if (!file) {
      return;
    }

    const response = await InstructorsService.extractFromFile({
      file: file,
    });

    setStudentList(response.result);
    setUploadStudentModal(true);

    setFile(null);
  }, [file]);

  const students = fetchStudents.value?.result;

  return (
    <>
      <Table
        style={{ marginLeft: 'auto', marginRight: 'auto' }}
        singleLine
        loading={loading}
        padded
      >
        <Table.Header>
          <Table.Row>
            <Table.HeaderCell>First Name</Table.HeaderCell>
            <Table.HeaderCell>Last Name</Table.HeaderCell>
            <Table.HeaderCell>E-mail address</Table.HeaderCell>
            <Table.HeaderCell>W Number</Table.HeaderCell>
            <Table.HeaderCell width="1">
              <span {...getRootProps()}>
                <>
                  <input {...getInputProps()} />
                  <Button color="orange">Add From Moodle CSV</Button>
                </>
              </span>
            </Table.HeaderCell>
            <Table.HeaderCell width="1">
              <Popup
                content="Add New Student"
                trigger={
                  <Button
                    primary
                    onClick={() => {
                      setCreateStudentModal(true);
                    }}
                  >
                    +
                  </Button>
                }
              />
            </Table.HeaderCell>
          </Table.Row>
        </Table.Header>

        <LoadingWrapper loading={fetchStudents.loading}>
          {students?.length ? (
            <>
              <Table.Body>
                {students?.map((x: any) => (
                  <Table.Row>
                    <Table.Cell>{x.firstName}</Table.Cell>
                    <Table.Cell>{x.lastName}</Table.Cell>
                    <Table.Cell>{x.emailAddress}</Table.Cell>
                    <Table.Cell>{x.studentSchoolNumber}</Table.Cell>
                    <Table.Cell />
                    <Table.Cell>
                      <Button
                        secondary
                        onClick={() => {
                          setUpdateStudentModal(true);
                          setCurrentStudent(x);
                        }}
                        size="small"
                      >
                        Edit
                      </Button>
                    </Table.Cell>
                  </Table.Row>
                ))}
              </Table.Body>
            </>
          ) : (
            <Table.Body>
              <Table.Row>
                <Table.Cell colSpan="4">
                  <Header>You Have No Students Registered</Header>
                </Table.Cell>
              </Table.Row>
            </Table.Body>
          )}
        </LoadingWrapper>
      </Table>
      {students && students?.length > 0 && (
        <div
          style={{
            width: '100%',
            display: 'flex',
            flexDirection: 'row',
          }}
        >
          <Pagination
            totalPages={
              fetchStudents.value
                ? Math.ceil(fetchStudents.value?.totalCount / pageSize)
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
      <CreateStudentModal
        courseId={props.courseId}
        modalState={createStudentModal}
        setModalState={(value: boolean) => setCreateStudentModal(value)}
      />
      <UpdateStudentModal
        currentStudent={currentStudent}
        modalState={updateStudentModal}
        setModalState={(value: boolean) => setUpdateStudentModal(value)}
      />
      <StudentUploadModal
        students={studentList}
        courseId={courseId}
        modalState={uploadStudentModal}
        setModalState={(value: boolean) => setUploadStudentModal(value)}
      />
    </>
  );
};
