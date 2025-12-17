import { ModuleTypeEnum } from 'src/lib/enums/ModuleTypeEnum';
import { ModuleConfigurationTypeEnum } from './enums/ModuleConfigurationTypeEnum';

export type ModuleConfigurationModel = {
  key: ModuleConfigurationTypeEnum;
  moduleType: ModuleTypeEnum;
  isModuleIdRequired: boolean;
  hasModuleSelection: boolean;
  hasTitle: boolean;
  hasDescription: boolean;
  hasVocabularyDirection: boolean;
};

export const moduleFormConfiguration: ModuleConfigurationModel[] = [
  {
    key: ModuleConfigurationTypeEnum.ModuleConfiguration,
    moduleType: ModuleTypeEnum.Vocabulary,
    isModuleIdRequired: false,
    hasModuleSelection: false,
    hasTitle: true,
    hasDescription: true,
    hasVocabularyDirection: false,
  },
  {
    key: ModuleConfigurationTypeEnum.SubModuleConfiguration,
    moduleType: ModuleTypeEnum.Vocabulary,
    isModuleIdRequired: true,
    hasModuleSelection: true,
    hasTitle: true,
    hasDescription: true,
    hasVocabularyDirection: true,
  },
];

export const getModuleConfiguration = (
  key: ModuleConfigurationTypeEnum
): ModuleConfigurationModel => {
  const config = moduleFormConfiguration.find((c) => c.key === key);
  return config!;
};
