import React from 'react';
import { Route, Switch } from 'react-router-dom';
import { SelectCourse } from './courses/select-course';
import { InstructorNavBar } from './instructor-nav';
import './routes.css';
import { paths } from '../../routing/paths';
import { CourseDashboardRoutes } from './course-dashboard/routes';

export const InstructorPortalRoutes = () => {
  return (
    <>
      <div>
        <InstructorNavBar />
        <Switch>
          <Route path={paths.instructor.selectCourse}>
            <SelectCourse />
          </Route>
          <Route path="/instructor/:courseId/*">
            <CourseDashboardRoutes />
          </Route>
        </Switch>
      </div>
    </>
  );
};
