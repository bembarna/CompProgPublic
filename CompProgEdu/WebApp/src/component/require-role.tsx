import React from 'react';
import { UserContext } from '../contexts/UserContext';
import { roles } from '../enums/roles';

type ConfigTypes = {
  role: string | string[];
};

export const RequireRole: React.FC<ConfigTypes> = ({ children, role }) => {
  const user = UserContext();
  if (!user.authenticated) {
    return <></>;
  }

  const userRole = user.role;
  const isAuthorized = userRole === role || roles.globalAdmin;

  return <>{isAuthorized && children}</>;
};
