import { StudentNavBar } from './student-nav';
import React from 'react';
import { Switch, Route, Redirect } from 'react-router-dom';
import { paths } from '../../routing/paths';
import { SelectCourseStudent } from './select-course/select-course';
import { UserContext } from '../../contexts/UserContext';
import { useAsync } from 'react-use';
import { StudentsService } from '../../swagger';
import { StudentChangePassword } from './student-change-password';
import { StudentCourseDashboardRoutes } from './course-dashboard/routes';
import './student-routes.css';

export const StudentPortalRoutes = () => {
  const userContext = UserContext();

  const fetchStudent = useAsync(async () => {
    const response = await StudentsService.getById({
      id: Number(userContext.studentId),
    });
    return response.result;
  }, [userContext.studentId]);

  const student = fetchStudent.value;

  return (
    <>
      <div className="student-background">
        <StudentNavBar />
        {student && (
          <Switch>
            {student.changedPassword ? (
              <>
                <Route path={paths.student.selectCourse}>
                  <SelectCourseStudent />
                </Route>
                <Route path={paths.student.dashboardRoot}>
                  <StudentCourseDashboardRoutes />
                </Route>
                {/* <Route>
                  <Redirect to={paths.student.selectCourse} />
                </Route> */}
              </>
            ) : (
              <>
                <Redirect to={paths.student.changePassword} />
                <Route path={paths.student.changePassword}>
                  <StudentChangePassword />
                </Route>
              </>
            )}
          </Switch>
        )}
      </div>
    </>
  );
};
