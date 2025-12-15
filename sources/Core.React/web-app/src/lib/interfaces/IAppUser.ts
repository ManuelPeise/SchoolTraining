import { Moment } from 'moment';
import { UserRoleEnum } from '../enums/UserRoleEnum';

export interface IAppUser {
  id: number;
  familyId: number;
  firstName?: string;
  lastName?: string;
  userName: string;
  dateOfBirth?: Moment;
  userRole: UserRoleEnum;
  email?: string;
}
