import React from 'react';
import { useHistory } from 'react-router-dom';
import { Button, Icon, Header } from 'semantic-ui-react';
import { paths } from '../../../routing/paths';
import { CourseDetailDto } from '../../../swagger';
import './header.css';

type DashboardHeader = {
  course: CourseDetailDto | undefined;
};

export const StudentCourseDashboardHeader = ({ course }: DashboardHeader) => {
  const history = useHistory();
  return (
    <>
      <div id="headerContainer">
        <Button
          style={{
            width: 120,
            height: 40,
            margin: '5px 0 0 5px',
          }}
          onClick={() => history.push(paths.student.selectCourse)}
          color="blue"
          compact
        >
          <Icon name="arrow left" />
          Courses
        </Button>
        <Header id="pageHeader" as="h1" style={{ fontSize: '40px' }}>
          {course?.title || ''} - {course?.section || ''}
        </Header>
      </div>
    </>
  );
};
