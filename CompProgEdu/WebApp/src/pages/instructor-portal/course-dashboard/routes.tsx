import React from 'react';
import { Route, Switch, useParams } from 'react-router-dom';
import { useAsync } from 'react-use';
import { paths } from '../../../routing/paths';
import { CourseService } from '../../../swagger';
import { CourseListings } from '../courses/course-listings';
import { CourseDashboardHeader } from './course-dashboard-header';
import '../routes.css';
import { AssignmentTabs } from './assignments/assignment-tabs';

export const CourseDashboardRoutes = () => {
  let { courseId } = useParams() as any;

  const fetchCourse = useAsync(async () => {
    const response = await CourseService.getById({
      id: courseId,
    });
    const result = response.result;
    return result;
  }, [courseId]);

  const course = fetchCourse.value;

  return (
    <>
      <CourseDashboardHeader course={course} />
      <div
        style={{
          height: '100%',
          paddingTop: 20,
        }}
      >
        <Switch>
          <Route path={paths.instructor.courseDashboard}>
            <CourseListings />
          </Route>
          <Route path={paths.instructor.assignmentDetails}>
            <AssignmentTabs />
          </Route>
        </Switch>
      </div>
    </>
  );
};
