/** Generate by swagger-axios-codegen */
// tslint:disable
/* eslint-disable */
import axiosStatic, { AxiosInstance } from 'axios';

export interface IRequestOptions {
  headers?: any;
  baseURL?: string;
  responseType?: string;
}

export interface IRequestConfig {
  method?: any;
  headers?: any;
  url?: any;
  data?: any;
  params?: any;
}

// Add options interface
export interface ServiceOptions {
  axios?: AxiosInstance;
}

// Add default options
export const serviceOptions: ServiceOptions = {};

// Instance selector
export function axios(configs: IRequestConfig, resolve: (p: any) => void, reject: (p: any) => void): Promise<any> {
  if (serviceOptions.axios) {
    return serviceOptions.axios
      .request(configs)
      .then(res => {
        resolve(res.data);
      })
      .catch(err => {
        reject(err);
      });
  } else {
    throw new Error('please inject yourself instance like axios  ');
  }
}

export function getConfigs(method: string, contentType: string, url: string, options: any): IRequestConfig {
  const configs: IRequestConfig = { ...options, method, url };
  configs.headers = {
    ...options.headers,
    'Content-Type': contentType
  };
  return configs;
}

export interface IList<T> extends Array<T> {}
export interface List<T> extends Array<T> {}
export interface IDictionary<TValue> {
  [key: string]: TValue;
}
export interface Dictionary<TValue> extends IDictionary<TValue> {}

export interface IListResult<T> {
  items?: T[];
}

export class ListResultDto<T> implements IListResult<T> {
  items?: T[];
}

export interface IPagedResult<T> extends IListResult<T> {
  totalCount: number;
}

export class PagedResultDto<T> implements IPagedResult<T> {
  totalCount!: number;
}

// customer definition
// empty
const basePath = '';

export class AssignmentsService {
  /**
   *
   */
  static getById(
    params: {
      /**  */
      id: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<AssignmentDetailDtoValidateableResponse> {
    return new Promise((resolve, reject) => {
      let url = basePath + '/api/assignments/by-id/{id}';
      url = url.replace('{id}', params['id'] + '');

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);

      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getAllAssignmentsByCourseId(
    params: {
      /**  */
      courseId: number;
      /**  */
      page?: number;
      /**  */
      pageSize?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<AssignmentSummaryDtoListPaginatedResponse> {
    return new Promise((resolve, reject) => {
      let url = basePath + '/api/assignments/by-course-id/{courseId}';
      url = url.replace('{courseId}', params['courseId'] + '');

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { page: params['page'], pageSize: params['pageSize'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getAllStudentAssignmentsByCourseId(
    params: {
      /**  */
      courseId: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<AssignmentSummaryDtoListValidateableResponse> {
    return new Promise((resolve, reject) => {
      let url = basePath + '/api/assignments/student/{courseId}';
      url = url.replace('{courseId}', params['courseId'] + '');

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);

      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static create(
    params: {
      /**  */
      body?: CreateAssignmentRequest;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<AssignmentGetDtoValidateableResponse> {
    return new Promise((resolve, reject) => {
      let url = basePath + '/api/assignments';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params['body'];

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static update(
    params: {
      /**  */
      body?: UpdateAssignmentByIdRequest;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<AssignmentDetailDtoValidateableResponse> {
    return new Promise((resolve, reject) => {
      let url = basePath + '/api/assignments';

      const configs: IRequestConfig = getConfigs('put', 'application/json', url, options);

      let data = params['body'];

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static delete(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<AssignmentGetDtoValidateableResponse> {
    return new Promise((resolve, reject) => {
      let url = basePath + '/api/assignments';

      const configs: IRequestConfig = getConfigs('delete', 'application/json', url, options);
      configs.params = { id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static submitAssignment(
    params: {
      /**  */
      assignmentId?: number;
      /**  */
      code?: string;
      /**  */
      language?: string;
      /**  */
      submission?: string;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<SubmissionResultValidateableResponse> {
    return new Promise((resolve, reject) => {
      let url = basePath + '/api/assignments/assignment-submission';

      const configs: IRequestConfig = getConfigs('post', 'multipart/form-data', url, options);
      configs.params = { assignmentId: params['assignmentId'], code: params['code'], language: params['language'] };
      let data = null;
      data = new FormData();
      if (params['submission']) {
        data.append('submission', params['submission'] as any);
      }

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
}

export class AuthenticationService {
  /**
   *
   */
  static login(
    params: {
      /**  */
      body?: LoginRequest;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<LoginResponseValidateableResponse> {
    return new Promise((resolve, reject) => {
      let url = basePath + '/api/authenticate/login';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params['body'];

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static register(
    params: {
      /**  */
      body?: RegisterRequest;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<UserGetDtoValidateableResponse> {
    return new Promise((resolve, reject) => {
      let url = basePath + '/api/authenticate/register';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params['body'];

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static registerStudent(
    params: {
      /**  */
      body?: RegisterStudentRequest;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<UserGetDtoValidateableResponse> {
    return new Promise((resolve, reject) => {
      let url = basePath + '/api/authenticate/register-student';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params['body'];

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static registerInstructor(
    params: {
      /**  */
      body?: RegisterInstructorRequest;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<UserGetDtoValidateableResponse> {
    return new Promise((resolve, reject) => {
      let url = basePath + '/api/authenticate/register-instructor';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params['body'];

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static logout(options: IRequestOptions = {}): Promise<any> {
    return new Promise((resolve, reject) => {
      let url = basePath + '/api/authenticate/logout';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getMe(options: IRequestOptions = {}): Promise<UserGetMeDtoValidateableResponse> {
    return new Promise((resolve, reject) => {
      let url = basePath + '/api/authenticate/get-me';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
}

export class CourseService {
  /**
   *
   */
  static getById(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<CourseDetailDtoValidateableResponse> {
    return new Promise((resolve, reject) => {
      let url = basePath + '/api/courses';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static create(
    params: {
      /**  */
      body?: CreateCourseRequest;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<CourseGetDtoValidateableResponse> {
    return new Promise((resolve, reject) => {
      let url = basePath + '/api/courses';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params['body'];

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static delete(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<CourseGetDtoValidateableResponse> {
    return new Promise((resolve, reject) => {
      let url = basePath + '/api/courses';

      const configs: IRequestConfig = getConfigs('delete', 'application/json', url, options);
      configs.params = { id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getAllByInstructorId(
    params: {
      /**  */
      instructorId: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<CourseSummaryDtoListValidateableResponse> {
    return new Promise((resolve, reject) => {
      let url = basePath + '/api/courses/instructor/{instructorId}';
      url = url.replace('{instructorId}', params['instructorId'] + '');

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);

      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getAllByStudentId(
    params: {
      /**  */
      studentId: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<CourseSummaryDtoListValidateableResponse> {
    return new Promise((resolve, reject) => {
      let url = basePath + '/api/courses/student/{studentId}';
      url = url.replace('{studentId}', params['studentId'] + '');

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);

      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static update(
    params: {
      /**  */
      id: number;
      /**  */
      body?: CourseDto;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<CourseGetDtoValidateableResponse> {
    return new Promise((resolve, reject) => {
      let url = basePath + '/api/courses/{id}';
      url = url.replace('{id}', params['id'] + '');

      const configs: IRequestConfig = getConfigs('put', 'application/json', url, options);

      let data = params['body'];

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
}

export class DesiredOutputsService {
  /**
   *
   */
  static getById(
    params: {
      /**  */
      id: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<DesiredOutputGetDtoValidateableResponse> {
    return new Promise((resolve, reject) => {
      let url = basePath + '/api/desired-outputs/{id}';
      url = url.replace('{id}', params['id'] + '');

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);

      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getByAssignmentId(
    params: {
      /**  */
      assignmentId: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<DesiredOutputGetDtoListValidateableResponse> {
    return new Promise((resolve, reject) => {
      let url = basePath + '/api/desired-outputs/by-assignment/{assignmentId}';
      url = url.replace('{assignmentId}', params['assignmentId'] + '');

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);

      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static create(
    params: {
      /**  */
      body?: CreateDesiredOutputRequest;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<DesiredOutputGetDtoValidateableResponse> {
    return new Promise((resolve, reject) => {
      let url = basePath + '/api/desired-outputs';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params['body'];

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static update(
    params: {
      /**  */
      body?: UpdateDesiredOutputRequest;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<DesiredOutputGetDtoValidateableResponse> {
    return new Promise((resolve, reject) => {
      let url = basePath + '/api/desired-outputs';

      const configs: IRequestConfig = getConfigs('put', 'application/json', url, options);

      let data = params['body'];

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static delete(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<DesiredOutputGetDtoValidateableResponse> {
    return new Promise((resolve, reject) => {
      let url = basePath + '/api/desired-outputs';

      const configs: IRequestConfig = getConfigs('delete', 'application/json', url, options);
      configs.params = { id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static updateOrder(
    params: {
      /**  */
      body?: DesiredOutputGetDto[];
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<DesiredOutputGetDtoListValidateableResponse> {
    return new Promise((resolve, reject) => {
      let url = basePath + '/api/desired-outputs/order';

      const configs: IRequestConfig = getConfigs('put', 'application/json', url, options);

      let data = params['body'];

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
}

export class InstructorsService {
  /**
   *
   */
  static create(
    params: {
      /**  */
      body?: CreateInstructorRequest;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<InstructorResponseDtoValidateableResponse> {
    return new Promise((resolve, reject) => {
      let url = basePath + '/api/instructors';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params['body'];

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getAll(options: IRequestOptions = {}): Promise<InstructorGetAllResponseDtoListValidateableResponse> {
    return new Promise((resolve, reject) => {
      let url = basePath + '/api/instructors';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);

      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static delete(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<InstructorResponseDtoValidateableResponse> {
    return new Promise((resolve, reject) => {
      let url = basePath + '/api/instructors';

      const configs: IRequestConfig = getConfigs('delete', 'application/json', url, options);
      configs.params = { id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static addStudentToCourse(
    params: {
      /**  */
      body?: AddStudentToCourseRequest;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<StudentResponseDtoValidateableResponse> {
    return new Promise((resolve, reject) => {
      let url = basePath + '/api/instructors/add-student-to-course';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params['body'];

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static extractFromFile(
    params: {
      /**  */
      file?: string;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<StudentDetailDtoListValidateableResponse> {
    return new Promise((resolve, reject) => {
      let url = basePath + '/api/instructors/extract';

      const configs: IRequestConfig = getConfigs('post', 'multipart/form-data', url, options);

      let data = null;
      data = new FormData();
      if (params['file']) {
        data.append('file', params['file'] as any);
      }

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static addStudentsFromFile(
    params: {
      /**  */
      studentList?: string;
      /**  */
      courseId?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<StudentDetailDtoListValidateableResponse> {
    return new Promise((resolve, reject) => {
      let url = basePath + '/api/instructors/upload';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);
      configs.params = { StudentList: params['studentList'], CourseId: params['courseId'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getById(
    params: {
      /**  */
      id: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<InstructorResponseDtoValidateableResponse> {
    return new Promise((resolve, reject) => {
      let url = basePath + '/api/instructors/{id}';
      url = url.replace('{id}', params['id'] + '');

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);

      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static update(
    params: {
      /**  */
      id: number;
      /**  */
      body?: InstructorDetailDto;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<InstructorResponseDtoValidateableResponse> {
    return new Promise((resolve, reject) => {
      let url = basePath + '/api/instructors/{id}';
      url = url.replace('{id}', params['id'] + '');

      const configs: IRequestConfig = getConfigs('put', 'application/json', url, options);

      let data = params['body'];

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getCourseStudents(
    params: {
      /**  */
      courseId: number;
      /**  */
      page?: number;
      /**  */
      pageSize?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<StudentResponseDtoListPaginatedResponse> {
    return new Promise((resolve, reject) => {
      let url = basePath + '/api/instructors/all-course-students/{courseId}';
      url = url.replace('{courseId}', params['courseId'] + '');

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { page: params['page'], pageSize: params['pageSize'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
}

export class InstructorSubmissionsService {
  /**
   *
   */
  static create(
    params: {
      /**  */
      assignmentId?: number;
      /**  */
      codeFile?: string;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<InstructorSubmissionDetailDtoValidateableResponse> {
    return new Promise((resolve, reject) => {
      let url = basePath + '/api/instructor-submissions';

      const configs: IRequestConfig = getConfigs('post', 'multipart/form-data', url, options);
      configs.params = { assignmentId: params['assignmentId'] };
      let data = null;
      data = new FormData();
      if (params['codeFile']) {
        data.append('codeFile', params['codeFile'] as any);
      }

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static deleteByAssignmentId(
    params: {
      /**  */
      assignmentId?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<InstructorSubmissionDetailDtoValidateableResponse> {
    return new Promise((resolve, reject) => {
      let url = basePath + '/api/instructor-submissions';

      const configs: IRequestConfig = getConfigs('delete', 'application/json', url, options);
      configs.params = { assignmentId: params['assignmentId'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getById(
    params: {
      /**  */
      id: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<InstructorSubmissionDetailDtoValidateableResponse> {
    return new Promise((resolve, reject) => {
      let url = basePath + '/api/instructor-submissions/{id}';
      url = url.replace('{id}', params['id'] + '');

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);

      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getChildById(
    params: {
      /**  */
      id: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<CurlySetDetailDtoValidateableResponse> {
    return new Promise((resolve, reject) => {
      let url = basePath + '/api/instructor-submissions/child-{id}';
      url = url.replace('{id}', params['id'] + '');

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);

      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getTreeNodes(
    params: {
      /**  */
      assignmentId?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<TreeNodeDtoListValidateableResponse> {
    return new Promise((resolve, reject) => {
      let url = basePath + '/api/instructor-submissions/tree-node';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { assignmentId: params['assignmentId'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
}

export class JDoodleApiTestService {
  /**
   *
   */
  static createNew(
    params: {
      /**  */
      body?: SendToJDoodleApiRequest;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<JDoodleRequestValidateableResponse> {
    return new Promise((resolve, reject) => {
      let url = basePath + '/api/jdoodle-api/test-code-exe';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params['body'];

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
}

export class MethodTestCaseService {
  /**
   *
   */
  static getById(
    params: {
      /**  */
      id: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<MethodTestCaseGetDtoValidateableResponse> {
    return new Promise((resolve, reject) => {
      let url = basePath + '/api/method-test-case/{id}';
      url = url.replace('{id}', params['id'] + '');

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);

      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getByAssignmentId(
    params: {
      /**  */
      assignmentId: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<MethodTestCaseGetDtoListValidateableResponse> {
    return new Promise((resolve, reject) => {
      let url = basePath + '/api/method-test-case/by-assignment/{assignmentId}';
      url = url.replace('{assignmentId}', params['assignmentId'] + '');

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);

      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static create(
    params: {
      /**  */
      body?: CreateMethodTestCaseRequest;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<MethodTestCaseGetDtoValidateableResponse> {
    return new Promise((resolve, reject) => {
      let url = basePath + '/api/method-test-case';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params['body'];

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static update(
    params: {
      /**  */
      body?: UpdateMethodTestCaseRequest;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<MethodTestCaseGetDtoValidateableResponse> {
    return new Promise((resolve, reject) => {
      let url = basePath + '/api/method-test-case';

      const configs: IRequestConfig = getConfigs('put', 'application/json', url, options);

      let data = params['body'];

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static delete(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<MethodTestCaseGetDtoValidateableResponse> {
    return new Promise((resolve, reject) => {
      let url = basePath + '/api/method-test-case';

      const configs: IRequestConfig = getConfigs('delete', 'application/json', url, options);
      configs.params = { id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
}

export class SendGridApiTestService {
  /**
   *
   */
  static createNew(
    params: {
      /**  */
      body?: SendToSendGridRequest;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<any> {
    return new Promise((resolve, reject) => {
      let url = basePath + '/api/sendgrid-api/send-email';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params['body'];

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
}

export class StudentsService {
  /**
   *
   */
  static create(
    params: {
      /**  */
      body?: CreateStudentRequest;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<StudentResponseDtoValidateableResponse> {
    return new Promise((resolve, reject) => {
      let url = basePath + '/api/students';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params['body'];

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getAll(options: IRequestOptions = {}): Promise<StudentGetAllResponseDtoListValidateableResponse> {
    return new Promise((resolve, reject) => {
      let url = basePath + '/api/students';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);

      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static delete(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<StudentResponseDtoValidateableResponse> {
    return new Promise((resolve, reject) => {
      let url = basePath + '/api/students';

      const configs: IRequestConfig = getConfigs('delete', 'application/json', url, options);
      configs.params = { id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getById(
    params: {
      /**  */
      id: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<StudentResponseDtoValidateableResponse> {
    return new Promise((resolve, reject) => {
      let url = basePath + '/api/students/{id}';
      url = url.replace('{id}', params['id'] + '');

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);

      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static update(
    params: {
      /**  */
      id: number;
      /**  */
      body?: StudentUpdateDto;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<StudentResponseDtoValidateableResponse> {
    return new Promise((resolve, reject) => {
      let url = basePath + '/api/students/{id}';
      url = url.replace('{id}', params['id'] + '');

      const configs: IRequestConfig = getConfigs('put', 'application/json', url, options);

      let data = params['body'];

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static updatePassword(
    params: {
      /**  */
      id: number;
      /**  */
      body?: UpdateStudentPasswordRequest;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<StudentResponseDtoValidateableResponse> {
    return new Promise((resolve, reject) => {
      let url = basePath + '/api/students/{id}/update-password';
      url = url.replace('{id}', params['id'] + '');

      const configs: IRequestConfig = getConfigs('put', 'application/json', url, options);

      let data = params['body'];

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
}

export class TestAccountService {
  /**
   *
   */
  static create(
    params: {
      /**  */
      body?: CreateTestAccountRequest;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<TestAccountGetDtoValidateableResponse> {
    return new Promise((resolve, reject) => {
      let url = basePath + '/api/test-account';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params['body'];

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getAll(options: IRequestOptions = {}): Promise<TestAccountGetDtoListValidateableResponse> {
    return new Promise((resolve, reject) => {
      let url = basePath + '/api/test-account';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);

      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static delete(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<TestAccountGetDtoValidateableResponse> {
    return new Promise((resolve, reject) => {
      let url = basePath + '/api/test-account';

      const configs: IRequestConfig = getConfigs('delete', 'application/json', url, options);
      configs.params = { id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getById(
    params: {
      /**  */
      id: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<TestAccountGetDtoValidateableResponse> {
    return new Promise((resolve, reject) => {
      let url = basePath + '/api/test-account/{id}';
      url = url.replace('{id}', params['id'] + '');

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);

      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static update(
    params: {
      /**  */
      id: number;
      /**  */
      body?: TestAccountDto;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<TestAccountGetDtoValidateableResponse> {
    return new Promise((resolve, reject) => {
      let url = basePath + '/api/test-account/{id}';
      url = url.replace('{id}', params['id'] + '');

      const configs: IRequestConfig = getConfigs('put', 'application/json', url, options);

      let data = params['body'];

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
}

export class UsersService {
  /**
   *
   */
  static create(
    params: {
      /**  */
      body?: CreateUserRequest;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<UserGetDtoValidateableResponse> {
    return new Promise((resolve, reject) => {
      let url = basePath + '/api/users';

      const configs: IRequestConfig = getConfigs('post', 'application/json', url, options);

      let data = params['body'];

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static getById(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<UserGetDtoValidateableResponse> {
    return new Promise((resolve, reject) => {
      let url = basePath + '/api/users';

      const configs: IRequestConfig = getConfigs('get', 'application/json', url, options);
      configs.params = { id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static delete(
    params: {
      /**  */
      id?: number;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<UserGetDtoValidateableResponse> {
    return new Promise((resolve, reject) => {
      let url = basePath + '/api/users';

      const configs: IRequestConfig = getConfigs('delete', 'application/json', url, options);
      configs.params = { id: params['id'] };
      let data = null;

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
  /**
   *
   */
  static update(
    params: {
      /**  */
      id: number;
      /**  */
      body?: UserDto;
    } = {} as any,
    options: IRequestOptions = {}
  ): Promise<UserGetDtoValidateableResponse> {
    return new Promise((resolve, reject) => {
      let url = basePath + '/api/users/{id}';
      url = url.replace('{id}', params['id'] + '');

      const configs: IRequestConfig = getConfigs('put', 'application/json', url, options);

      let data = params['body'];

      configs.data = data;
      axios(configs, resolve, reject);
    });
  }
}

export interface CourseGetDto {
  /**  */
  id: number;

  /**  */
  title: string;

  /**  */
  section: string;

  /**  */
  instructorId: number;
}

export interface UserClaim {
  /**  */
  user: User;

  /**  */
  id: number;

  /**  */
  userId: number;

  /**  */
  claimType: string;

  /**  */
  claimValue: string;
}

export interface UserLogin {
  /**  */
  user: User;

  /**  */
  loginProvider: string;

  /**  */
  providerKey: string;

  /**  */
  providerDisplayName: string;

  /**  */
  userId: number;
}

export interface UserToken {
  /**  */
  user: User;

  /**  */
  userId: number;

  /**  */
  loginProvider: string;

  /**  */
  name: string;

  /**  */
  value: string;
}

export interface RoleClaim {
  /**  */
  role: Role;

  /**  */
  id: number;

  /**  */
  roleId: number;

  /**  */
  claimType: string;

  /**  */
  claimValue: string;
}

export interface Role {
  /**  */
  users: UserRole[];

  /**  */
  roleClaims: RoleClaim[];

  /**  */
  id: number;

  /**  */
  name: string;

  /**  */
  normalizedName: string;

  /**  */
  concurrencyStamp: string;
}

export interface UserRole {
  /**  */
  user: User;

  /**  */
  role: Role;

  /**  */
  userId: number;

  /**  */
  roleId: number;
}

export interface User {
  /**  */
  firstName: string;

  /**  */
  lastName: string;

  /**  */
  userClaims: UserClaim[];

  /**  */
  userLogins: UserLogin[];

  /**  */
  userTokens: UserToken[];

  /**  */
  userRoles: UserRole[];

  /**  */
  id: number;

  /**  */
  userName: string;

  /**  */
  normalizedUserName: string;

  /**  */
  email: string;

  /**  */
  normalizedEmail: string;

  /**  */
  emailConfirmed: boolean;

  /**  */
  passwordHash: string;

  /**  */
  securityStamp: string;

  /**  */
  concurrencyStamp: string;

  /**  */
  phoneNumber: string;

  /**  */
  phoneNumberConfirmed: boolean;

  /**  */
  twoFactorEnabled: boolean;

  /**  */
  lockoutEnd: Date;

  /**  */
  lockoutEnabled: boolean;

  /**  */
  accessFailedCount: number;
}

export interface Instructor {
  /**  */
  user: User;

  /**  */
  id: number;

  /**  */
  userId: number;

  /**  */
  title: string;
}

export interface Student {
  /**  */
  user: User;

  /**  */
  studentCourses: StudentCourse[];

  /**  */
  id: number;

  /**  */
  userId: number;

  /**  */
  grade: number;

  /**  */
  studentSchoolNumber: string;

  /**  */
  changedPassword: boolean;
}

export interface StudentCourse {
  /**  */
  studentId: number;

  /**  */
  student: Student;

  /**  */
  courseId: number;

  /**  */
  course: Course;
}

export interface Course {
  /**  */
  instructor: Instructor;

  /**  */
  studentCourses: StudentCourse[];

  /**  */
  id: number;

  /**  */
  title: string;

  /**  */
  section: string;

  /**  */
  instructorId: number;
}

export interface MethodTestCase {
  /**  */
  assignment: Assignment;

  /**  */
  input: string;

  /**  */
  id: number;

  /**  */
  returnType: string;

  /**  */
  methodTestInjectable: string;

  /**  */
  assignmentId: number;

  /**  */
  paramInputs: string;

  /**  */
  output: string;

  /**  */
  hint: string;

  /**  */
  pointValue: number;
}

export interface Assignment {
  /**  */
  course: Course;

  /**  */
  desiredOutputs: DesiredOutput[];

  /**  */
  methodTestCases: MethodTestCase[];

  /**  */
  id: number;

  /**  */
  assignmentName: string;

  /**  */
  allowedLanguages: string;

  /**  */
  assignmentInstructions: string;

  /**  */
  exampleInput: string;

  /**  */
  exampleOutput: string;

  /**  */
  totalPointValue: number;

  /**  */
  dueDate: Date;

  /**  */
  visibilityDate: Date;

  /**  */
  assignmentSolutionFileName: string;

  /**  */
  courseId: number;
}

export interface DesiredOutput {
  /**  */
  assignment: Assignment;

  /**  */
  id: number;

  /**  */
  assignmentId: number;

  /**  */
  input: string;

  /**  */
  output: string;

  /**  */
  pointValue: number;

  /**  */
  order: number;
}

export interface AssignmentDetailDto {
  /**  */
  courseDto: CourseGetDto;

  /**  */
  desiredOutputs: DesiredOutput[];

  /**  */
  methodTestCases: MethodTestCase[];

  /**  */
  totalPointsAssigned: number;

  /**  */
  id: number;

  /**  */
  assignmentName: string;

  /**  */
  allowedLanguages: string;

  /**  */
  assignmentInstructions: string;

  /**  */
  exampleInput: string;

  /**  */
  exampleOutput: string;

  /**  */
  totalPointValue: number;

  /**  */
  dueDate: Date;

  /**  */
  visibilityDate: Date;

  /**  */
  assignmentSolutionFileName: string;

  /**  */
  courseId: number;
}

export interface ErrorResponse {
  /**  */
  fieldName: string;

  /**  */
  error: string;
}

export interface AssignmentDetailDtoValidateableResponse {
  /**  */
  result: AssignmentDetailDto;

  /**  */
  isValidResponse: boolean;

  /**  */
  errors: ErrorResponse[];
}

export interface AssignmentSummaryDto {
  /**  */
  id: number;

  /**  */
  courseId: number;

  /**  */
  assignmentName: string;

  /**  */
  assignmentInstructions: string;

  /**  */
  totalPointValue: number;

  /**  */
  assignmentSolution: string;

  /**  */
  dueDate: string;

  /**  */
  visibilityDate: string;
}

export interface AssignmentSummaryDtoListPaginatedResponse {
  /**  */
  totalCount: number;

  /**  */
  result: AssignmentSummaryDto[];

  /**  */
  isValidResponse: boolean;

  /**  */
  errors: ErrorResponse[];
}

export interface AssignmentSummaryDtoListValidateableResponse {
  /**  */
  result: AssignmentSummaryDto[];

  /**  */
  isValidResponse: boolean;

  /**  */
  errors: ErrorResponse[];
}

export interface CreateAssignmentRequest {
  /**  */
  assignmentName: string;

  /**  */
  allowedLanguages: string;

  /**  */
  assignmentInstructions: string;

  /**  */
  exampleInput: string;

  /**  */
  exampleOutput: string;

  /**  */
  totalPointValue: number;

  /**  */
  dueDate: Date;

  /**  */
  visibilityDate: Date;

  /**  */
  assignmentSolutionFileName: string;

  /**  */
  courseId: number;
}

export interface AssignmentGetDto {
  /**  */
  id: number;

  /**  */
  assignmentName: string;

  /**  */
  allowedLanguages: string;

  /**  */
  assignmentInstructions: string;

  /**  */
  exampleInput: string;

  /**  */
  exampleOutput: string;

  /**  */
  totalPointValue: number;

  /**  */
  dueDate: Date;

  /**  */
  visibilityDate: Date;

  /**  */
  assignmentSolutionFileName: string;

  /**  */
  courseId: number;
}

export interface AssignmentGetDtoValidateableResponse {
  /**  */
  result: AssignmentGetDto;

  /**  */
  isValidResponse: boolean;

  /**  */
  errors: ErrorResponse[];
}

export interface UpdateAssignmentByIdRequest {
  /**  */
  id: number;

  /**  */
  assignmentName: string;

  /**  */
  allowedLanguages: string;

  /**  */
  assignmentInstructions: string;

  /**  */
  exampleInput: string;

  /**  */
  exampleOutput: string;

  /**  */
  totalPointValue: number;

  /**  */
  dueDate: Date;

  /**  */
  visibilityDate: Date;

  /**  */
  assignmentSolutionFileName: string;

  /**  */
  courseId: number;
}

export interface MethodTestCheckDto {
  /**  */
  passed: boolean;

  /**  */
  hint: string;
}

export interface SubmissionResult {
  /**  */
  totalScore: number;

  /**  */
  outputChecks: boolean[];

  /**  */
  methodTestChecks: MethodTestCheckDto[];
}

export interface SubmissionResultValidateableResponse {
  /**  */
  result: SubmissionResult;

  /**  */
  isValidResponse: boolean;

  /**  */
  errors: ErrorResponse[];
}

export interface LoginRequest {
  /**  */
  email: string;

  /**  */
  password: string;

  /**  */
  stayLoggedIn: boolean;
}

export interface UserGetDto {
  /**  */
  id: number;

  /**  */
  emailAddress: string;

  /**  */
  firstName: string;

  /**  */
  lastName: string;

  /**  */
  role: string;
}

export interface LoginResponse {
  /**  */
  user: UserGetDto;

  /**  */
  token: string;
}

export interface LoginResponseValidateableResponse {
  /**  */
  result: LoginResponse;

  /**  */
  isValidResponse: boolean;

  /**  */
  errors: ErrorResponse[];
}

export interface RegisterRequest {
  /**  */
  password: string;

  /**  */
  confirmPassword: string;

  /**  */
  emailAddress: string;

  /**  */
  firstName: string;

  /**  */
  lastName: string;

  /**  */
  role: string;
}

export interface UserGetDtoValidateableResponse {
  /**  */
  result: UserGetDto;

  /**  */
  isValidResponse: boolean;

  /**  */
  errors: ErrorResponse[];
}

export interface RegisterStudentRequest {
  /**  */
  password: string;

  /**  */
  confirmPassword: string;

  /**  */
  emailAddress: string;

  /**  */
  firstName: string;

  /**  */
  lastName: string;

  /**  */
  studentSchoolNumber: string;

  /**  */
  changedPassword: boolean;
}

export interface RegisterInstructorRequest {
  /**  */
  password: string;

  /**  */
  confirmPassword: string;

  /**  */
  firstName: string;

  /**  */
  lastName: string;

  /**  */
  emailAddress: string;

  /**  */
  title: string;
}

export interface UserGetMeDto {
  /**  */
  instructorId: number;

  /**  */
  studentId: number;

  /**  */
  id: number;

  /**  */
  emailAddress: string;

  /**  */
  firstName: string;

  /**  */
  lastName: string;

  /**  */
  role: string;
}

export interface UserGetMeDtoValidateableResponse {
  /**  */
  result: UserGetMeDto;

  /**  */
  isValidResponse: boolean;

  /**  */
  errors: ErrorResponse[];
}

export interface CourseDetailDto {
  /**  */
  id: number;

  /**  */
  title: string;

  /**  */
  section: string;

  /**  */
  instructorName: string;
}

export interface CourseDetailDtoValidateableResponse {
  /**  */
  result: CourseDetailDto;

  /**  */
  isValidResponse: boolean;

  /**  */
  errors: ErrorResponse[];
}

export interface CreateCourseRequest {
  /**  */
  title: string;

  /**  */
  section: string;

  /**  */
  instructorId: number;
}

export interface CourseGetDtoValidateableResponse {
  /**  */
  result: CourseGetDto;

  /**  */
  isValidResponse: boolean;

  /**  */
  errors: ErrorResponse[];
}

export interface CourseSummaryDto {
  /**  */
  id: number;

  /**  */
  title: string;

  /**  */
  section: string;

  /**  */
  studentCount: number;

  /**  */
  instructorName: string;
}

export interface CourseSummaryDtoListValidateableResponse {
  /**  */
  result: CourseSummaryDto[];

  /**  */
  isValidResponse: boolean;

  /**  */
  errors: ErrorResponse[];
}

export interface CourseDto {
  /**  */
  title: string;

  /**  */
  section: string;

  /**  */
  instructorId: number;
}

export interface DesiredOutputGetDto {
  /**  */
  id: number;

  /**  */
  assignmentId: number;

  /**  */
  input: string;

  /**  */
  output: string;

  /**  */
  pointValue: number;

  /**  */
  order: number;
}

export interface DesiredOutputGetDtoValidateableResponse {
  /**  */
  result: DesiredOutputGetDto;

  /**  */
  isValidResponse: boolean;

  /**  */
  errors: ErrorResponse[];
}

export interface DesiredOutputGetDtoListValidateableResponse {
  /**  */
  result: DesiredOutputGetDto[];

  /**  */
  isValidResponse: boolean;

  /**  */
  errors: ErrorResponse[];
}

export interface CreateDesiredOutputRequest {
  /**  */
  assignmentId: number;

  /**  */
  input: string;

  /**  */
  output: string;

  /**  */
  pointValue: number;

  /**  */
  order: number;
}

export interface UpdateDesiredOutputRequest {
  /**  */
  id: number;

  /**  */
  assignmentId: number;

  /**  */
  input: string;

  /**  */
  output: string;

  /**  */
  pointValue: number;

  /**  */
  order: number;
}

export interface CreateInstructorRequest {
  /**  */
  firstName: string;

  /**  */
  lastName: string;

  /**  */
  emailAddress: string;

  /**  */
  title: string;
}

export interface InstructorResponseDto {
  /**  */
  id: number;

  /**  */
  userId: number;

  /**  */
  firstName: string;

  /**  */
  lastName: string;

  /**  */
  emailAddress: string;

  /**  */
  title: string;
}

export interface InstructorResponseDtoValidateableResponse {
  /**  */
  result: InstructorResponseDto;

  /**  */
  isValidResponse: boolean;

  /**  */
  errors: ErrorResponse[];
}

export interface InstructorGetAllResponseDto {
  /**  */
  id: number;

  /**  */
  userId: number;

  /**  */
  firstName: string;

  /**  */
  lastName: string;

  /**  */
  emailAddress: string;

  /**  */
  title: string;
}

export interface InstructorGetAllResponseDtoListValidateableResponse {
  /**  */
  result: InstructorGetAllResponseDto[];

  /**  */
  isValidResponse: boolean;

  /**  */
  errors: ErrorResponse[];
}

export interface AddStudentToCourseRequest {
  /**  */
  courseId: number;

  /**  */
  emailAddress: string;

  /**  */
  firstName: string;

  /**  */
  lastName: string;

  /**  */
  studentSchoolNumber: string;

  /**  */
  changedPassword: boolean;
}

export interface StudentResponseDto {
  /**  */
  id: number;

  /**  */
  userId: number;

  /**  */
  emailAddress: string;

  /**  */
  firstName: string;

  /**  */
  lastName: string;

  /**  */
  studentSchoolNumber: string;

  /**  */
  changedPassword: boolean;
}

export interface StudentResponseDtoValidateableResponse {
  /**  */
  result: StudentResponseDto;

  /**  */
  isValidResponse: boolean;

  /**  */
  errors: ErrorResponse[];
}

export interface StudentDetailDto {
  /**  */
  emailAddress: string;

  /**  */
  firstName: string;

  /**  */
  lastName: string;

  /**  */
  studentSchoolNumber: string;

  /**  */
  changedPassword: boolean;
}

export interface StudentDetailDtoListValidateableResponse {
  /**  */
  result: StudentDetailDto[];

  /**  */
  isValidResponse: boolean;

  /**  */
  errors: ErrorResponse[];
}

export interface InstructorDetailDto {
  /**  */
  firstName: string;

  /**  */
  lastName: string;

  /**  */
  emailAddress: string;

  /**  */
  title: string;
}

export interface StudentResponseDtoListPaginatedResponse {
  /**  */
  totalCount: number;

  /**  */
  result: StudentResponseDto[];

  /**  */
  isValidResponse: boolean;

  /**  */
  errors: ErrorResponse[];
}

export interface PrimitiveVariableDto {
  /**  */
  id: number;

  /**  */
  assignmentId: number;

  /**  */
  variableType: string;

  /**  */
  variableName: string;

  /**  */
  variableValue: string;

  /**  */
  variableSignature: string;

  /**  */
  curlySetId: number;
}

export interface PropertySignatureDto {
  /**  */
  id: number;

  /**  */
  propertyHead: string;

  /**  */
  assignmentId: number;

  /**  */
  propertyType: string;

  /**  */
  propertyName: string;

  /**  */
  accessModifier: string;

  /**  */
  isStatic: boolean;

  /**  */
  propertyFunction: string;

  /**  */
  curlySetId: number;
}

export interface ClassSignature {
  /**  */
  id: number;

  /**  */
  assignmentId: number;

  /**  */
  fullClassSignature: string;

  /**  */
  accessModifier: string;

  /**  */
  isStatic: boolean;

  /**  */
  isAbstract: boolean;

  /**  */
  className: string;
}

export interface MethodParameter {
  /**  */
  parameterType: string;

  /**  */
  parameterName: string;
}

export interface MethodSignature {
  /**  */
  methodTestCase: MethodTestCase;

  /**  */
  id: number;

  /**  */
  fullMethodSignature: string;

  /**  */
  methodName: string;

  /**  */
  accessModifier: string;

  /**  */
  returnType: string;

  /**  */
  isReference: boolean;

  /**  */
  isAsync: boolean;

  /**  */
  isVoid: boolean;

  /**  */
  isStatic: boolean;

  /**  */
  isReadOnly: boolean;

  /**  */
  assignmentId: number;

  /**  */
  methodParameters: MethodParameter[];

  /**  */
  methodTestCaseId: number;
}

export interface PrimitiveStatement {
  /**  */
  id: number;

  /**  */
  assignmentId: number;

  /**  */
  statement: string;

  /**  */
  statementSignature: string;
}

export interface ReturnStatement {
  /**  */
  id: number;

  /**  */
  returnStartIndex: number;

  /**  */
  returnEndIndex: number;

  /**  */
  returnSignature: string;
}

export interface CurlySetDetailDto {
  /**  */
  primitiveVariables: PrimitiveVariableDto[];

  /**  */
  propertySignatures: PropertySignatureDto[];

  /**  */
  curlySets: CurlySetDetailDto[];

  /**  */
  id: number;

  /**  */
  assignmentId: number;

  /**  */
  parentId: number;

  /**  */
  isMethod: boolean;

  /**  */
  isMain: boolean;

  /**  */
  isClass: boolean;

  /**  */
  propertySignatureId: number;

  /**  */
  classSignatureId: number;

  /**  */
  classSignature: ClassSignature;

  /**  */
  methodSignatureId: number;

  /**  */
  methodSignature: MethodSignature;

  /**  */
  statementSignatureId: number;

  /**  */
  statementSignature: PrimitiveStatement;

  /**  */
  isPrimitiveStatement: boolean;

  /**  */
  returnStatementId: number;

  /**  */
  returnStatement: ReturnStatement;

  /**  */
  hasReturn: boolean;
}

export interface InstructorSubmissionDetailDto {
  /**  */
  curlySet: CurlySetDetailDto;

  /**  */
  id: number;

  /**  */
  curlySetId: number;

  /**  */
  assignmentId: number;
}

export interface InstructorSubmissionDetailDtoValidateableResponse {
  /**  */
  result: InstructorSubmissionDetailDto;

  /**  */
  isValidResponse: boolean;

  /**  */
  errors: ErrorResponse[];
}

export interface CurlySetDetailDtoValidateableResponse {
  /**  */
  result: CurlySetDetailDto;

  /**  */
  isValidResponse: boolean;

  /**  */
  errors: ErrorResponse[];
}

export interface TreeNodeDto {
  /**  */
  key: string;

  /**  */
  label: string;

  /**  */
  nodes: TreeNodeDto[];
}

export interface TreeNodeDtoListValidateableResponse {
  /**  */
  result: TreeNodeDto[];

  /**  */
  isValidResponse: boolean;

  /**  */
  errors: ErrorResponse[];
}

export interface SendToJDoodleApiRequest {
  /**  */
  inputs: string[];

  /**  */
  script: string;

  /**  */
  language: string;

  /**  */
  versionIndex: string;
}

export interface JDoodleRequest {
  /**  */
  output: string;

  /**  */
  statusCode: string;

  /**  */
  memory: string;

  /**  */
  cpuTime: string;
}

export interface JDoodleRequestValidateableResponse {
  /**  */
  result: JDoodleRequest;

  /**  */
  isValidResponse: boolean;

  /**  */
  errors: ErrorResponse[];
}

export interface MethodTestCaseGetDto {
  /**  */
  id: number;

  /**  */
  returnType: string;

  /**  */
  methodTestInjectable: string;

  /**  */
  assignmentId: number;

  /**  */
  paramInputs: string;

  /**  */
  output: string;

  /**  */
  hint: string;

  /**  */
  pointValue: number;
}

export interface MethodTestCaseGetDtoValidateableResponse {
  /**  */
  result: MethodTestCaseGetDto;

  /**  */
  isValidResponse: boolean;

  /**  */
  errors: ErrorResponse[];
}

export interface MethodTestCaseGetDtoListValidateableResponse {
  /**  */
  result: MethodTestCaseGetDto[];

  /**  */
  isValidResponse: boolean;

  /**  */
  errors: ErrorResponse[];
}

export interface MethodTestCaseDto {
  /**  */
  assignmentId: number;

  /**  */
  paramInputs: string;

  /**  */
  output: string;

  /**  */
  hint: string;

  /**  */
  pointValue: number;
}

export interface CreateMethodTestCaseRequest {
  /**  */
  methodTestCaseDto: MethodTestCaseDto;

  /**  */
  methodNodeKey: string;
}

export interface UpdateMethodTestCaseRequest {
  /**  */
  id: number;

  /**  */
  returnType: string;

  /**  */
  methodTestInjectable: string;

  /**  */
  assignmentId: number;

  /**  */
  paramInputs: string;

  /**  */
  output: string;

  /**  */
  hint: string;

  /**  */
  pointValue: number;
}

export interface SendToSendGridRequest {
  /**  */
  emailTo: string;

  /**  */
  personTo: string;

  /**  */
  subject: string;

  /**  */
  message: string;
}

export interface CreateStudentRequest {
  /**  */
  emailAddress: string;

  /**  */
  firstName: string;

  /**  */
  lastName: string;

  /**  */
  studentSchoolNumber: string;

  /**  */
  changedPassword: boolean;
}

export interface StudentGetAllResponseDto {
  /**  */
  id: number;

  /**  */
  userId: number;

  /**  */
  firstName: string;

  /**  */
  lastName: string;

  /**  */
  emailAddress: string;

  /**  */
  grade: string;

  /**  */
  studentSchoolNumber: string;
}

export interface StudentGetAllResponseDtoListValidateableResponse {
  /**  */
  result: StudentGetAllResponseDto[];

  /**  */
  isValidResponse: boolean;

  /**  */
  errors: ErrorResponse[];
}

export interface StudentUpdateDto {
  /**  */
  firstName: string;

  /**  */
  lastName: string;

  /**  */
  studentSchoolNumber: string;

  /**  */
  changedPassword: boolean;
}

export interface UpdateStudentPasswordRequest {
  /**  */
  id: number;

  /**  */
  newPassword: string;

  /**  */
  confirmPassword: string;
}

export interface CreateTestAccountRequest {
  /**  */
  accountNumber: string;

  /**  */
  accountName: string;

  /**  */
  emailAddress: string;

  /**  */
  lastVisit: Date;

  /**  */
  isPremium: boolean;

  /**  */
  numberOfPeople: number;
}

export interface TestAccountGetDto {
  /**  */
  id: number;

  /**  */
  accountNumber: string;

  /**  */
  accountName: string;

  /**  */
  emailAddress: string;

  /**  */
  lastVisit: Date;

  /**  */
  isPremium: boolean;

  /**  */
  numberOfPeople: number;
}

export interface TestAccountGetDtoValidateableResponse {
  /**  */
  result: TestAccountGetDto;

  /**  */
  isValidResponse: boolean;

  /**  */
  errors: ErrorResponse[];
}

export interface TestAccountGetDtoListValidateableResponse {
  /**  */
  result: TestAccountGetDto[];

  /**  */
  isValidResponse: boolean;

  /**  */
  errors: ErrorResponse[];
}

export interface TestAccountDto {
  /**  */
  accountNumber: string;

  /**  */
  accountName: string;

  /**  */
  emailAddress: string;

  /**  */
  lastVisit: Date;

  /**  */
  isPremium: boolean;

  /**  */
  numberOfPeople: number;
}

export interface CreateUserRequest {
  /**  */
  emailAddress: string;

  /**  */
  firstName: string;

  /**  */
  lastName: string;

  /**  */
  role: string;
}

export interface UserDto {
  /**  */
  emailAddress: string;

  /**  */
  firstName: string;

  /**  */
  lastName: string;

  /**  */
  role: string;
}
