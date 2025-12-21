// please keep in sync with ~\sources\Shared.Models\Administration\UserRight.cs
export interface IUserRight {
  rightGuid: string;
  name: string;
  nameResourceKey: string;
  descriptionResourceKey: string;
  deny: boolean;
  view: boolean;
  edit: boolean;
  create: boolean;
  delete: boolean;
}
