import { IFamilyModel } from './interfaces/IFamilyModel';

export const dummyFamilies: IFamilyModel[] = Array.from({ length: 50 }, (_, i) => ({
  familyId: i + 1,
  name: `Family ${i + 1}`,
  contactMailAddress: `family${i + 1}@example.com`,
  isActive: i % 2 === 0,
  lastUpdateBy: i % 3 === 0 ? 'admin' : `user${(i % 5) + 1}`,
  lastUpdateAt: `2025-01-${((i % 28) + 1).toString().padStart(2, '0')}`,
}));
