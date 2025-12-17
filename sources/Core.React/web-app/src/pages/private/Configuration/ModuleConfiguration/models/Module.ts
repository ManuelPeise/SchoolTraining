import { VocabularyDirectionEnum } from 'src/lib/enums/VocabularyDirectionEnum';

export type Module = {
  moduleId: number;
  idExternal?: string | null;
  title: string;
  description: string;
  direction: VocabularyDirectionEnum | null;
};

export type ModuleServerModel = Omit<Module, 'direction'>;

export type SubModuleServermodel = Module;
