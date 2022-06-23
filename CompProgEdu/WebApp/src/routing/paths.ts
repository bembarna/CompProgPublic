export const portalRoutes = {
  instructor: '/instructor',
  student: '/student',
};

export const paths = {
  home: '/home',
  testAccount: '/test-accounts',
  simpleLogin: '/login-page',
  aboutUs: '/about-us',
  ourProcess: '/our-process',
  ReactAceTest: '/code-playground',
  instructor: {
    selectCourse: `${portalRoutes.instructor}/select-course`,
    courseDashboard: `${portalRoutes.instructor}/:courseId/course-dashboard/:defaultTab`,
    assignmentDetails: `${portalRoutes.instructor}/:courseId/assignment-details/:assignmentId`,
  },
  student: {
    dashboardRoot: `${portalRoutes.student}/:courseId/*`,
    courseDashboard: `${portalRoutes.student}/:courseId/course-dashboard/:defaultTab`,
    assignmentDetails: `${portalRoutes.student}/:courseId/assignment-details/:assignmentId`,
    changePassword: `${portalRoutes.student}/change-password`,
    selectCourse: `${portalRoutes.student}/select-course`,
  },
};
