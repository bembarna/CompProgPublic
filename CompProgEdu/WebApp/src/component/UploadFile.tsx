import React, { useCallback, useState } from 'react';
import { useDropzone } from 'react-dropzone';
import { Message, Modal, Button } from 'semantic-ui-react';

const fileTypes = ['txt', 'js', 'cs', 'java', 'cpp'];

const handleFile = (acceptedFiles: any) => {
  acceptedFiles.forEach((file: any) => {
    const reader = new FileReader();
    reader.onabort = () => console.log('file reading was aborted');
    reader.onerror = () => console.log('file reading has failed');
    reader.onload = () => {
      // Do whatever you want with the file contents
      const binaryStr = reader.result;
      console.log(binaryStr);
    };
    reader.readAsText(file);
  });
};

const checkExtension = (file: any) => {
  var extension = file[0].name.split('.').pop().toLowerCase(),
    isSuccess = fileTypes.indexOf(extension) > -1;
  return isSuccess;
};

const UploadFile = () => {
  const [error, setError] = useState(false);
  const [success, setSuccess] = useState(false);
  const [open, setOpen] = useState(false);

  const onDrop = useCallback((acceptedFiles) => {
    if (acceptedFiles && acceptedFiles[0]) {
      let isSuccess = checkExtension(acceptedFiles);
      if (isSuccess) {
        handleFile(acceptedFiles);
        setError(false);
        setSuccess(true);
        setOpen(false);
      } else {
        console.warn('Not Accepted');
        setSuccess(false);
        setError(true);
        setOpen(false);
      }
    }
  }, []);
  const { getRootProps, getInputProps, isDragActive } = useDropzone({ onDrop });

  return (
    <div>
      {error ? (
        <Message negative floating compact>
          <Message.Header>File Not Accepted</Message.Header>
          <p>Please try a different file type</p>
        </Message>
      ) : null}

      {success ? (
        <Message positive floating compact>
          <Message.Header>File Accepted</Message.Header>
        </Message>
      ) : null}
      <div>
        <Modal
          onClose={() => setOpen(false)}
          onOpen={() => setOpen(true)}
          open={open}
          trigger={<Button>Upload File</Button>}
          size={'tiny'}
          dimmer={'blurring'}
        >
          <div {...getRootProps()}>
            <div style={{ height: '100px' }}>
              <input {...getInputProps()} />
              {isDragActive ? (
                <p>Drop the files here ...</p>
              ) : (
                <p>Drag 'n' drop some files here, or click to select files</p>
              )}
            </div>
          </div>
        </Modal>
      </div>
    </div>
  );
};

export default UploadFile;
