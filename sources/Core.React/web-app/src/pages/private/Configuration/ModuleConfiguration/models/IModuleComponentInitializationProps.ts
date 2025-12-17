import { DropdownItem } from 'src/components/input/Dropdown';
import { ModuleServerModel, SubModuleServermodel } from './Module';

export interface IModuleComponentInitializationProps {
  isReadonly: boolean;
  modulesDropdownItems: DropdownItem[];
  subModulesDropdownItems: DropdownItem[];
  modules: ModuleServerModel[];
  subModules: SubModuleServermodel[];
  getResource: (key: string) => string;
  setIsLoading: (isLoading: boolean) => void;
}

export type ModuleInitializationModel = {
  modules: ModuleServerModel[];
  subModules: SubModuleServermodel[];
};
