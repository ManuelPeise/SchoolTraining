import moment from 'moment';
import { UserRoleEnum } from '../enums/UserRoleEnum';

type JwtPayload = {
  familyId: number;
  id: number;
  name: string;
  userRole: UserRoleEnum;
  expiretime: moment.Moment;
};

export const parseJwtToken = (token: string | null): JwtPayload | null => {
  try {
    if (!token) {
      return null;
    }

    const base64Url = token.split('.')[1];
    if (!base64Url) {
      return null;
    }

    const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    const jsonPayload = decodeURIComponent(
      atob(base64)
        .split('')
        .map((c) => '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2))
        .join('')
    );

    return JSON.parse(jsonPayload) as JwtPayload;
  } catch (error) {
    console.error('Error parsing JWT:', error);
    return null;
  }
};

export const isTokenExpired = (token: string): boolean => {
  const payload: JwtPayload | null = parseJwtToken(token);

  if (!payload || moment() > moment(payload.expiretime)) {
    return true;
  }

  return moment().isAfter(payload.expiretime);
};
