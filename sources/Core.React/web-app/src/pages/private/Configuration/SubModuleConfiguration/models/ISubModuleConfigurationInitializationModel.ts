import { DropdownItem } from 'src/components/input/Dropdown';
import { ISubModuleDataCollection } from '../../models/subModule';
import { IModule } from '../../models/module';

// keep in sync with Shared.Models.Learning.SubModuleConfigurationInitializationModel
export interface ISubModuleConfigurationInitializationModel {
  parentModuleDropdownItems: DropdownItem[];
  modules: IModule[];
  subModuleDataCollection: ISubModuleDataCollection[];
}
