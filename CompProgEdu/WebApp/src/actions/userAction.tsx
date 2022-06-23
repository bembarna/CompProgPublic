//in useActions.ts file
import { AuthenticationService } from '../swagger';
import { instance } from '../swagger/config';

type LoginResponse = {
  email: string;
  password: string;
  stayLoggedIn: boolean;
};

type UserResponsee = {
  emailAddress: string | undefined;
  firstName: string | undefined;
  lastName: string | undefined;
  errors: { errorMessage: string }[];
};

export const loginUser = async (LoginResponse: LoginResponse) => {
  var result = await AuthenticationService.login({
    body: {
      email: LoginResponse.email,
      password: LoginResponse.password,
      stayLoggedIn: LoginResponse.stayLoggedIn,
    },
  });

  var errors =
    result.errors?.map((x) => ({ errorMessage: x.error })) || undefined;

  if (errors.length === 0) {
    const token = `${result.result.token}`;
    if (LoginResponse.stayLoggedIn) {
      localStorage.setItem('jwtToken', token);
      localStorage.setItem('firstName', result.result.user.firstName);
      localStorage.setItem('lastName', result.result.user.lastName);
    }
    else {
      sessionStorage.setItem('jwtToken', token);
      sessionStorage.setItem('firstName', result.result.user.firstName);
      sessionStorage.setItem('lastName', result.result.user.lastName);
      sessionStorage.setItem('lastName', result.result.user.lastName);
    }
  }

  const userReturn: UserResponsee = {
    emailAddress: result.result?.user.emailAddress || undefined,
    firstName: result.result?.user.firstName || undefined,
    lastName: result.result?.user.lastName || undefined,
    errors: errors,
  };

  return userReturn;
};

export const logoutUser = async () => {
  if (localStorage.getItem('jwtToken')) {
    localStorage.removeItem('jwtToken');
    localStorage.removeItem('firstName');
    localStorage.removeItem('lastName');
  }
  else if (sessionStorage.getItem('jwtToken')) {
    sessionStorage.removeItem('jwtToken');
    sessionStorage.removeItem('firstName');
    sessionStorage.removeItem('lastName');
  }
};

instance.interceptors.request.use(
  function (config) {
    const token = localStorage.getItem('jwtToken') ?? sessionStorage.getItem('jwtToken');
    if (token) {
      config.headers['Authorization'] = 'Bearer ' + token;
    }
    return config;
  },
  function (error) {
    return Promise.reject(error);
  }
);
