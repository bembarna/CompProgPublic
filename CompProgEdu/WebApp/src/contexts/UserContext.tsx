/* eslint-disable no-redeclare */
import jwt from 'jsonwebtoken';
import { useAsync } from 'react-use';
import { AuthenticationService } from '../swagger';

type UserContext = {
  firstName: string | undefined;
  lastName: string | undefined;
  authenticated: boolean;
  role: string | undefined;
  instructorId: number | undefined;
  studentId: number | undefined;
};

export const UserContext = () => {
  const authToken = localStorage.jwtToken ?? sessionStorage.jwtToken;
  const decodedToken: any = jwt.decode(authToken);

  const me = useAsync(async () => {
    const { result } = await AuthenticationService.getMe();
    return result;
  });

  if (decodedToken) {
    const basicContext = me.value || undefined;

    var user: UserContext = {
      firstName: basicContext?.firstName ?? '',
      lastName: basicContext?.lastName ?? '',
      role:
        decodedToken[
        'http://schemas.microsoft.com/ws/2008/06/identity/claims/role'
        ],
      authenticated: true,
      instructorId: basicContext?.instructorId,
      studentId: basicContext?.studentId,
    };

    return user;
  } else {
    var user: UserContext = {
      firstName: undefined,
      lastName: undefined,
      role: undefined,
      authenticated: false,
      instructorId: undefined,
      studentId: undefined,
    };

    return user;
  }
};
