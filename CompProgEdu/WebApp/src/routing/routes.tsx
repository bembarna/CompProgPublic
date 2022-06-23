import React from 'react';
import { Switch, Route, Redirect } from 'react-router-dom';
import { HomeRoutes } from '../pages/home-pages/routes';
import { paths, portalRoutes } from './paths';
import { InstructorPortalRoutes } from '../pages/instructor-portal/routes';
import { AuthorizedRoute } from '../component/auth-wrappers';
import { roles } from '../enums/roles';
import { StudentPortalRoutes } from '../pages/student-portal/student-routes';

export const RouteConfig = () => {
  return (
    <>
      <Switch>
        <Route path={paths.home}>
          <AuthorizedRoute noAuth>
            <HomeRoutes />
          </AuthorizedRoute>
        </Route>
        <Route path={portalRoutes.instructor}>
          <AuthorizedRoute role={roles.instructor}>
            <InstructorPortalRoutes />
          </AuthorizedRoute>
        </Route>
        <Route path={portalRoutes.student}>
          <AuthorizedRoute role={roles.student}>
            <StudentPortalRoutes />
          </AuthorizedRoute>
        </Route>
        <Route>
          <AuthorizedRoute role={roles.student}>
            <Redirect to={paths.student.selectCourse} />
          </AuthorizedRoute>
          <AuthorizedRoute noAuth>
            <Redirect to={paths.home} />
          </AuthorizedRoute>
        </Route>
      </Switch>
    </>
  );
};
