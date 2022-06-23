import React from 'react';
import { Route, Switch, useParams } from 'react-router-dom';
import { useAsync } from 'react-use';
import { paths } from '../../../routing/paths';
import { CourseService } from '../../../swagger';
import { StudentAssignmentDetails } from './assignments/assignment-details';
import { StudentDashboardTabs } from './dashboard-tabs/dashboard-tabs';
import { StudentCourseDashboardHeader } from './header';
import './routes.css';

export const StudentCourseDashboardRoutes = () => {
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
      <StudentCourseDashboardHeader course={course} />
      <div className="background">
        <Switch>
          <Route path={paths.student.courseDashboard}>
            <StudentDashboardTabs />
          </Route>
          <Route path={paths.student.assignmentDetails}>
            <StudentAssignmentDetails />
          </Route>
        </Switch>
      </div>
    </>
  );
};
