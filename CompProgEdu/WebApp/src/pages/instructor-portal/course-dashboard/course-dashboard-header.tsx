/* eslint-disable jsx-a11y/anchor-is-valid */
import React, { useState } from 'react';
import { useHistory } from 'react-router-dom';
import { Button, Icon, Header } from 'semantic-ui-react';
import { paths } from '../../../routing/paths';
import { CourseDetailDto } from '../../../swagger';
import { UpdateCourseModal } from '../courses/course-update-modal';
import './course-dashboard-header.css';

type DashboardHeader = {
  course: CourseDetailDto | undefined;
};

export const CourseDashboardHeader = ({ course }: DashboardHeader) => {
  const [updateCourseModal, setUpdateCourseModal] = useState<boolean>(false);
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
          onClick={() => history.push(paths.instructor.selectCourse)}
          color="blue"
          compact
        >
          <Icon name="arrow left" />
          Courses
        </Button>
        <Header
          id="pageHeader"
          as="h1"
          style={{ fontSize: '40px', marginRight: 120 }}
        >
          <a
            onClick={() => setUpdateCourseModal(true)}
            style={{ cursor: 'pointer' }}
          >
            {course?.title || ''} - {course?.section || ''}
          </a>
        </Header>
      </div>
      <UpdateCourseModal
        course={course}
        modalState={updateCourseModal}
        setModalState={(value: boolean) => setUpdateCourseModal(value)}
      />
    </>
  );
};
