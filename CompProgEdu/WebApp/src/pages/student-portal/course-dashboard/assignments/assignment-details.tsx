import moment from 'moment';
import React, { useState } from 'react';
import { useDropzone } from 'react-dropzone';
import { Link, useParams } from 'react-router-dom';
import { useToasts } from 'react-toast-notifications';
import { useAsync } from 'react-use';
import {
  Container,
  Icon,
  Segment,
  Header,
  Table,
  Message,
  Button,
  Dropdown,
  Popup,
} from 'semantic-ui-react';
import { LoadingWrapper } from '../../../../component/loading-wrapper';
import { AssignmentsService } from '../../../../swagger';
import AceEditor from 'react-ace';
import { languages } from '../../../instructor-portal/course-dashboard/assignments/languages';

const submitMethods = {
  file: 'file',
  editor: 'editor',
};

type MethodTestCaseResult = {
  passed: boolean;
  hint: string;
};

export const StudentAssignmentDetails = () => {
  const { assignmentId, courseId } = useParams() as any;
  const [outputResults, setOutputResults] = useState<boolean[]>();
  const [testCaseResults, setTestCaseResults] = useState<
    MethodTestCaseResult[]
  >();
  const [points, setPoints] = useState<number>();
  const [submitting, setSubmitting] = useState<boolean>(false);
  const [file, setFile] = useState<any>();
  const [code, setCode] = useState<string>();
  const [language, setLanguage] = useState<string>();
  const [submitMethod, setSubmitMethod] = useState<string>(submitMethods.file);

  const { addToast } = useToasts();

  const { getRootProps, getInputProps } = useDropzone({
    onDrop: async (acceptedFiles: any[]) => {
      setFile(acceptedFiles[0]);
    },
  });

  const submitAssignment = async () => {
    setSubmitting(true);
    try {
      var payload = {};
      if (submitMethod === submitMethods.editor) {
        payload = {
          code: code,
          language: language,
          assignmentId: assignmentId,
        };
      } else {
        payload = { submission: file, assignmentId: assignmentId };
      }

      console.log(language);
      const response = await AssignmentsService.submitAssignment(payload);

      if (response.errors.length > 0) {
        addToast(response.errors[0].error, { appearance: 'error' });
        setSubmitting(false);
        return;
      }

      setOutputResults(response.result.outputChecks);
      setTestCaseResults(response.result.methodTestChecks);
      setPoints(response.result.totalScore);
      setSubmitting(false);
    } catch {
      addToast('Error! Something went wrong', { appearance: 'error' });
      setSubmitting(false);
    }
  };

  const fetchAssignment = useAsync(async () => {
    const response = await AssignmentsService.getById({ id: assignmentId });

    const allowedLanguages = response?.result.allowedLanguages.split(', ');
    const languageOptions = languages.filter((x) =>
      allowedLanguages?.includes(x.text)
    );
    setLanguage(languageOptions[0]?.value);

    return response.result;
  }, [assignmentId]);

  const assignment = fetchAssignment.value;
  const allowedLanguages = assignment?.allowedLanguages.split(', ');
  const languageOptions = languages.filter((x) =>
    allowedLanguages?.includes(x.text)
  );

  return (
    <>
      <Container style={{ width: '45%', minWidth: 900 }}>
        <Link
          to={`/student/${courseId}/course-dashboard/0`}
          style={{ fontWeight: 'bold', fontSize: 16 }}
        >
          <Icon name="caret left" />
          Back to Assignments
        </Link>
        <Segment>
          <LoadingWrapper loading={fetchAssignment.loading}>
            <div style={{ display: 'flex', flexDirection: 'row' }}>
              <Header as="h1">{assignment?.assignmentName}</Header>
            </div>
            <Table basic="very">
              <Table.Row>
                <Table.Cell width="3">Due Date:</Table.Cell>
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
              {assignment?.desiredOutputs.length !== 0 && (
                <Table.Row>
                  <Table.Cell>Desired Outputs:</Table.Cell>
                  <Table.Cell>
                    {outputResults
                      ? outputResults.map((x) =>
                          x ? (
                            <Icon name="check" color="green" size="large" />
                          ) : (
                            <Icon name="times" color="red" size="large" />
                          )
                        )
                      : assignment?.desiredOutputs.map(() => (
                          <Icon name="square" color="grey" size="large" />
                        ))}
                  </Table.Cell>
                </Table.Row>
              )}
              {assignment?.methodTestCases.length !== 0 && (
                <Table.Row>
                  <Table.Cell>Test Cases:</Table.Cell>
                  <Table.Cell>
                    {testCaseResults
                      ? testCaseResults.map((x) =>
                          x.passed ? (
                            <Popup
                              content={x.hint}
                              position="top center"
                              trigger={
                                <Icon name="check" color="green" size="large" />
                              }
                            />
                          ) : (
                            <Popup
                              content={x.hint}
                              position="top center"
                              trigger={
                                <Icon name="times" color="red" size="large" />
                              }
                            />
                          )
                        )
                      : assignment?.methodTestCases.map((x) => (
                          <Popup
                            content={x.hint}
                            position="top center"
                            trigger={
                              <Icon name="square" color="grey" size="large" />
                            }
                          />
                        ))}
                  </Table.Cell>
                </Table.Row>
              )}
              <Table.Row>
                <Table.Cell>Total Points:</Table.Cell>
                <Table.Cell>{points ? points : '-'}</Table.Cell>
              </Table.Row>
              <Table.Row>
                <Table.Cell>Submission</Table.Cell>
                <Table.Cell>
                  <Button.Group fluid widths="2" style={{ marginBottom: 10 }}>
                    <Button
                      color={
                        submitMethod === submitMethods.file ? 'black' : 'grey'
                      }
                      onClick={() => setSubmitMethod(submitMethods.file)}
                    >
                      File
                    </Button>
                    <Button
                      color={
                        submitMethod === submitMethods.editor ? 'black' : 'grey'
                      }
                      onClick={() => setSubmitMethod(submitMethods.editor)}
                    >
                      Code Editor
                    </Button>
                  </Button.Group>
                  {submitMethod === submitMethods.file && (
                    <>
                      <span {...getRootProps()}>
                        <input {...getInputProps()} />
                        {!file ? (
                          <Message
                            style={{
                              height: 100,
                              cursor: 'pointer',
                            }}
                            icon="plus"
                            content={
                              <Header>
                                Drag program file here or click to add your
                                assignment submission.
                              </Header>
                            }
                          />
                        ) : (
                          <Message color="green">
                            <Header>
                              <Icon name="file outline" />
                              {file.name}
                            </Header>
                          </Message>
                        )}
                      </span>
                      {file && (
                        <Button
                          primary
                          fluid
                          style={{ marginTop: 10 }}
                          onClick={() => submitAssignment()}
                          loading={submitting}
                        >
                          Submit Assignment
                        </Button>
                      )}
                    </>
                  )}
                  {submitMethod === submitMethods.editor && (
                    <>
                      <div style={{ textAlign: 'right', marginBottom: 10 }}>
                        <span style={{ margin: 'auto', fontSize: 14 }}>
                          Language:{' '}
                        </span>
                        <Dropdown
                          options={languageOptions}
                          defaultValue={languageOptions[0]?.value}
                          selection
                          onChange={(e, d) => setLanguage(String(d.value))}
                          style={{ width: 50 }}
                        />
                      </div>
                      <div style={{ overflowY: 'scroll', height: 350 }}>
                        <AceEditor
                          mode="csharp"
                          theme="twilight"
                          name="blah2"
                          showPrintMargin={true}
                          defaultValue={code}
                          value={code}
                          showGutter={true}
                          highlightActiveLine={true}
                          width="100%"
                          minLines={25}
                          maxLines={45}
                          debounceChangePeriod={100}
                          editorProps={{ $blockScrolling: true }}
                          onChange={(value) => {
                            setCode(value);
                          }}
                          setOptions={{
                            enableBasicAutocompletion: true,
                            enableLiveAutocompletion: true,
                            enableSnippets: false,
                            showLineNumbers: true,
                            tabSize: 2,
                            autoScrollEditorIntoView: true,
                          }}
                        />
                      </div>
                      <Button
                        primary
                        fluid
                        style={{ marginTop: 10 }}
                        disabled={!code}
                        onClick={() => submitAssignment()}
                        loading={submitting}
                      >
                        Submit Assignment
                      </Button>
                    </>
                  )}
                </Table.Cell>
              </Table.Row>
            </Table>
          </LoadingWrapper>
        </Segment>
      </Container>
    </>
  );
};
