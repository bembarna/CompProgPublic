import React, { useState } from 'react';
import { useDropzone } from 'react-dropzone';
import { useToasts } from 'react-toast-notifications';
import { useAsync, useAsyncFn } from 'react-use';
import { Modal, Popup, Icon, Message, Button } from 'semantic-ui-react';
import { DeleteButton } from '../../../../component/custom-delete-button';
import {
  AssignmentDetailDto,
  InstructorSubmissionsService,
} from '../../../../swagger';

type AssignmentSolutionUploadModal = {
  assignment: AssignmentDetailDto | undefined;
  modalState: boolean;
  setModalState: (value: boolean) => void;
};

export const AssignmentSolutionUploadModal = (
  props: AssignmentSolutionUploadModal
) => {
  const [updateLoading, setUpdateLoading] = useState<boolean>(false);
  const [file, setFile] = useState<any>();
  const { addToast } = useToasts();

  const { getRootProps, getInputProps } = useDropzone({
    onDrop: async (acceptedFiles: any[]) => {
      setFile(acceptedFiles[0]);
    },
  });

  useAsync(async () => {
    setFile(null);
  }, [props.modalState]);

  const createSolutionFile = async () => {
    setUpdateLoading(true);
    try {
      const response = await InstructorSubmissionsService.create({
        codeFile: file,
        assignmentId: props.assignment?.id,
      });
      if (response.errors.length > 0) {
        addToast(
          'Error uploading file. Please check to make sure it is a valid program.',
          { appearance: 'error' }
        );
        setUpdateLoading(false);
        props.setModalState(false);
      } else {
        addToast('Successfully added solution file!', {
          appearance: 'success',
        });
        setUpdateLoading(false);
        props.setModalState(false);
      }
    } catch {
      addToast(
        'Error uploading file. Please check to make sure it is a valid program.',
        { appearance: 'error' }
      );
      setUpdateLoading(false);
      props.setModalState(false);
    }
  };

  const [deleteSolutionState, deleteSolution] = useAsyncFn(async () => {
    await InstructorSubmissionsService.deleteByAssignmentId({
      assignmentId: props.assignment?.id,
    });
    props.setModalState(false);
    addToast('Successfully deleted assignment solution!', {
      appearance: 'success',
    });
  });

  return (
    <>
      <Modal
        open={props.modalState}
        onClose={() => {
          props.setModalState(false);
        }}
        size="tiny"
      >
        <Modal.Header>
          Assignment Solution File{' '}
          <Popup
            content="This file should be the expected program for this assignment. This file will be used to create test cases."
            position="top center"
            trigger={<Icon name="info circle" size="small" />}
          />
        </Modal.Header>
        <Modal.Content>
          <span {...getRootProps()}>
            <input {...getInputProps()} />
            {props.assignment?.assignmentSolutionFileName && (
              <Message
                header={`Current File Used: ${props.assignment?.assignmentSolutionFileName}`}
                color="blue"
                style={{ marginTop: 0 }}
              />
            )}
            {file ? (
              <>
                <Message icon="upload" header={file.name} color="green" />
              </>
            ) : (
              <>
                <Message
                  icon="upload"
                  header="Upload Solution File"
                  content="Click this box or drag a file here to upload your assignment solution program."
                />
              </>
            )}
          </span>
        </Modal.Content>
        <Modal.Actions>
          {props.assignment?.assignmentSolutionFileName && (
            <DeleteButton
              buttonText="Delete Solution File"
              event={deleteSolution}
              loading={deleteSolutionState.loading}
            />
          )}
          <Button
            disabled={!file}
            primary
            onClick={() => createSolutionFile()}
            loading={updateLoading}
          >
            Update Solution File
          </Button>
        </Modal.Actions>
      </Modal>
    </>
  );
};
