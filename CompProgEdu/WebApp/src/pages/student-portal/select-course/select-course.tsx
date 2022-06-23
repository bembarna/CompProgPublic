import React from 'react';
import { useHistory } from 'react-router-dom';
import { useAsync } from 'react-use';
import { Card, Container, Grid, Header } from 'semantic-ui-react';
import { StudentCustomCard } from '../../../component/custom-cards/student-card';
import { LoadingWrapper } from '../../../component/loading-wrapper';
import { UserContext } from '../../../contexts/UserContext';
import { CourseService } from '../../../swagger';

export const SelectCourseStudent = () => {
  const { studentId } = UserContext();

  const fetchCourses = useAsync(async () => {
    const response = await CourseService.getAllByStudentId({
      studentId: studentId as number,
    });
    return response.result;
  }, [studentId]);

  const courses = fetchCourses.value;

  const history = useHistory();

  return (
    <>
      <div id="pageHeader">
        <Header as="h1" style={{ paddingTop: 8, color: 'white' }}>
          Course Menu
        </Header>
      </div>
      <Grid centered>
        <Container id="courseContainer">
          <LoadingWrapper loading={fetchCourses.loading}>
            <Card.Group centered stackable itemsPerRow={4}>
              {courses?.map((x) => (
                <>
                  <StudentCustomCard
                    onClick={() =>
                      history.push(`/student/${x.id}/course-dashboard/0`)
                    }
                    link
                    header={`${x.title} - ${x.section}`}
                    header2={x.instructorName}
                  />
                </>
              ))}
            </Card.Group>
          </LoadingWrapper>
        </Container>
      </Grid>
    </>
  );
};
