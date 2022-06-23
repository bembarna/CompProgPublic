/* eslint-disable jsx-a11y/anchor-is-valid */
import React, { useState } from 'react';
import { useHistory, useParams } from 'react-router-dom';
import { InstructorAssignmentListing } from '../course-dashboard/assignments/assignment-listing';
import { CourseStudentTable } from '../course-dashboard/students/student-listing';
import { Tab, Header, Container } from 'semantic-ui-react';

export const CourseListings = () => {
  let { courseId, defaultTab } = useParams() as any;
  const [studentCount, setStudentCount] = useState<number>(0);
  const history = useHistory();

  const handleChange = (e: any, { activeIndex }: any) => {
    history.push(`/instructor/${courseId}/course-dashboard/${activeIndex}`);
  };

  return (
    <div>
      <div style={{ width: '90%', marginLeft: 'auto', marginRight: 'auto' }}>
        <Container style={{ width: '80%', marginBottom: 50 }}>
          <Header
            style={{
              color: 'white',
              fontSize: '25px',
              float: 'right',
              marginTop: '17px',
            }}
          >
            Enrolled Students: {studentCount}
          </Header>
          <Header style={{ color: 'white', fontSize: '40px' }}>
            Class Info
          </Header>
          <Tab
            defaultActiveIndex={defaultTab}
            onTabChange={handleChange}
            style={{
              backgroundColor: 'white',
              padding: '10px',
              borderRadius: '5px',
            }}
            panes={[
              {
                menuItem: 'Students',
                render: () => (
                  <CourseStudentTable
                    courseId={parseInt(courseId)}
                    setStudentCount={(value: number) => setStudentCount(value)}
                  />
                ),
              },

              {
                menuItem: 'Assignments',
                render: () => (
                  <InstructorAssignmentListing courseId={parseInt(courseId)} />
                ),
              },
            ]}
          />
        </Container>
      </div>
    </div>
  );
};
