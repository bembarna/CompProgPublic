import React, { useState } from 'react';
import { Header, Grid, Container, Card } from 'semantic-ui-react';
import { CourseService } from '../../../swagger';
import { useAsync } from 'react-use';
import './select-course.scoped.css';
import { InstructorCustomCard } from '../../../component/custom-cards/instructor-card';
import { UserContext } from '../../../contexts/UserContext';
import { useHistory } from 'react-router-dom';
import { CreateCourseModal } from './course-create-modal';
import { LoadingWrapper } from '../../../component/loading-wrapper';

export const SelectCourse = () => {
  const [openModal, setOpenModal] = useState<boolean>(false);
  const instructor = UserContext();

  const fetchCourses = useAsync(async () => {
    const response = await CourseService.getAllByInstructorId({
      instructorId: instructor.instructorId as number,
    });
    return response.result;
  }, [instructor.instructorId, openModal]);

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
                  <InstructorCustomCard
                    onClick={() =>
                      history.push(`/instructor/${x.id}/course-dashboard/0`)
                    }
                    link
                    instructorCardHeader={`${x.title} - ${x.section}`}
                    instructorCardHeader2={`Student Count: ${x.studentCount}`}
                  />
                </>
              ))}
              <InstructorCustomCard
                instructorCardHeader="Add New Course"
                instructorCardHeader2="+"
                style={{ backgroundColor: 'ggreyray' }}
                onClick={() => setOpenModal(true)}
              />
            </Card.Group>
          </LoadingWrapper>
        </Container>
      </Grid>
      <CreateCourseModal
        modalState={openModal}
        setModalState={(value: boolean) => setOpenModal(value)}
      />
    </>
  );
};
