import { DropdownItem } from 'src/components/input/Dropdown';
import { VocabularyDirectionEnum } from 'src/lib/enums/VocabularyDirectionEnum';

export const vocabularyDirectionTypeDropdownItems: DropdownItem[] = [
  { id: VocabularyDirectionEnum.GermanEnglish, label: 'common.labelGermanToEnglish' },
  { id: VocabularyDirectionEnum.EnglishGerman, label: 'common.labelEnglishToGerman' },
  { id: VocabularyDirectionEnum.EnglishDanish, label: 'common.labelEnglishToDanish' },
  { id: VocabularyDirectionEnum.DanishEnglish, label: 'common.labelDanishToEnglish' },
  { id: VocabularyDirectionEnum.EnglishFrench, label: 'common.labelEnglishToFrench' },
  { id: VocabularyDirectionEnum.FrenchEnglish, label: 'common.labelFrenchToEnglish' },
  { id: VocabularyDirectionEnum.GermanDanish, label: 'common.labelGermanToDanish' },
  { id: VocabularyDirectionEnum.DanishGerman, label: 'common.labelDanishToGerman' },
  { id: VocabularyDirectionEnum.GermanFrench, label: 'common.labelGermanToFrench' },
  { id: VocabularyDirectionEnum.FrenchGerman, label: 'common.labelFrenchToGerman' },
  { id: VocabularyDirectionEnum.DanishFrench, label: 'common.labelDanishToFrench' },
  { id: VocabularyDirectionEnum.FrenchDanish, label: 'common.labelFrenchToDanish' },
].sort((a, b) => a.id - b.id);
