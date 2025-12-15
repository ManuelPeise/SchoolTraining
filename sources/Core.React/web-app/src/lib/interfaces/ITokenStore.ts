import { Moment } from 'moment';

export interface ITokenStore {
  jwt: string;
  refreshToken: string;
  expiresAt: Moment;
}
