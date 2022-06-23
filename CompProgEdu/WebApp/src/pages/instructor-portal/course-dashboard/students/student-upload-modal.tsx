import _ from 'lodash';
import React, { useEffect, useState } from 'react';
import { useToasts } from 'react-toast-notifications';
import { Button, Icon, Modal, Table } from 'semantic-ui-react';
import { InstructorsService, StudentDetailDto } from '../../../../swagger';

type UploadTypes = {
  students: StudentDetailDto[] | undefined;
  courseId: number;
  modalState: boolean;
  setModalState: (value: boolean) => void;
};

export const StudentUploadModal = (props: UploadTypes) => {
  const [studentList, setStudentList] = useState<
    StudentDetailDto[] | undefined
  >();
  const [loading, setLoading] = useState<boolean>(false);
  const { addToast } = useToasts();

  useEffect(() => {
    setStudentList(props.students);
  }, [props.students]);

  const removeStudent = (student: StudentDetailDto) => {
    let list = studentList as StudentDetailDto[];

    list = _.remove(studentList as StudentDetailDto[], (x) => {
      return x !== student;
    });

    setStudentList(list);
  };

  const upload = async () => {
    setLoading(true);
    const response = await InstructorsService.addStudentsFromFile({
      studentList: JSON.stringify(studentList),
      courseId: props.courseId,
    });

    if (response.errors.length > 0) {
      //TODO:Put an error message here if this fails.
      setLoading(false);
      return null;
    }

    addToast('Successfully added students from file!', {
      appearance: 'success',
    });
    setLoading(false);
    props.setModalState(false);
  };

  return (
    <>
      <Modal open={props.modalState} onClose={() => props.setModalState(false)}>
        <Modal.Header>Add From Moodle CSV</Modal.Header>
        <Modal.Content>
          <Table style={{ marginLeft: 'auto', marginRight: 'auto' }} singleLine>
            <Table.Header>
              <Table.Row>
                <Table.HeaderCell>First Name</Table.HeaderCell>
                <Table.HeaderCell>Last Name</Table.HeaderCell>
                <Table.HeaderCell>E-mail address</Table.HeaderCell>
                <Table.HeaderCell>W Number</Table.HeaderCell>
                <Table.HeaderCell width="1"></Table.HeaderCell>
              </Table.Row>
              {studentList?.map((x) => (
                <>
                  <Table.Row>
                    <Table.Cell>{x.firstName}</Table.Cell>
                    <Table.Cell>{x.lastName}</Table.Cell>
                    <Table.Cell>{x.emailAddress}</Table.Cell>
                    <Table.Cell>{x.studentSchoolNumber}</Table.Cell>
                    <Table.Cell width="1">
                      <Button
                        icon
                        circular
                        color="red"
                        size="tiny"
                        onClick={() => removeStudent(x)}
                      >
                        <Icon name="remove" style={{ paddingTop: 2 }} />
                      </Button>
                    </Table.Cell>
                  </Table.Row>
                </>
              ))}
            </Table.Header>
          </Table>
          <Modal.Actions>
            <Button onClick={() => props.setModalState(false)}>Cancel</Button>
            <Button primary onClick={() => upload()} loading={loading}>
              Add Students
            </Button>
          </Modal.Actions>
        </Modal.Content>
      </Modal>
    </>
  );
};
