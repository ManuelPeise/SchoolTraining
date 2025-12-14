export interface INotificationBadgeState {
  show: boolean;
  message: string;
  color: 'success' | 'error' | 'info' | 'warning';
  duration: number;
}
