import { Moment } from 'moment';

export interface ITokenStore {
  userId: number;
  jwt: string;
  refreshToken: string;
  expiresAt: Moment;
}
