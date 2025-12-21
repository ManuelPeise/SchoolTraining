import { VocabularyDirectionEnum } from 'src/lib/enums/VocabularyDirectionEnum';
import { IModule } from './module';
import { DropdownItem } from 'src/components/input/Dropdown';

// keep in sync with Shared.Models.Learning.SubModuleBase
export interface ISubModuleBase {
  subModuleId: number;
  moduleId: number;
  module: IModule;
  idExternal: string;
  title: string;
  description: string;
  direction: VocabularyDirectionEnum;
  lastUpdateBy: string | null;
  lastUpdateAt: string | null;
}

// keep in sync with Shared.Models.Learning.SubModuleDataCollection
export interface ISubModuleDataCollection {
  moduleId: number;
  subModules: ISubModuleBase[];
  subModuleDropdownItems: DropdownItem[];
}
