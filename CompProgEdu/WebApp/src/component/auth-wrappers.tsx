import React from 'react';
import { UserContext } from '../contexts/UserContext';
import { Redirect } from 'react-router-dom';
import { roles } from '../enums/roles';
import { paths } from '../routing/paths';

type ComponentTypes = {
  notAuthenticated?: boolean;
};

type RouteTypes = {
  role?: string | string[];
  noAuth?: boolean;
};

export const AuthorizedComponent: React.FC<ComponentTypes> = ({
  children,
  notAuthenticated,
}) => {
  const user = UserContext();

  console.log(user);

  if (!notAuthenticated) {
    if (user.authenticated) {
      return <>{children}</>;
    } else {
      return <></>;
    }
  } else {
    if (!user.authenticated) {
      return <>{children}</>;
    } else {
      return <></>;
    }
  }
};

export const AuthorizedRoute: React.FC<RouteTypes> = ({
  children,
  role,
  noAuth,
}) => {
  const user = UserContext();
  const userRole = user.role;

  const RedirectUser = () => {
    return (
      <>
        {/*TODO: We will have to make a global admin portal eventually.*/}
        {(userRole === roles.instructor && (
          <Redirect to={paths.instructor.selectCourse} />
        )) ||
          (userRole === roles.student && (
            <Redirect to={paths.student.selectCourse} />
          )) ||
          (!userRole && <Redirect to={paths.home} />)}
      </>
    );
  };

  if (userRole === roles.globalAdmin) {
    return <>{children}</>;
  }

  if (!user.authenticated) {
    if (noAuth) {
      return <>{children}</>;
    } else {
      return <RedirectUser />;
    }
  }

  if (role && role.includes(user.role ?? '')) {
    return <>{children}</>;
  } else {
    return <RedirectUser />;
  }
};
