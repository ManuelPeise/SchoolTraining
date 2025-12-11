import i18n from 'i18next';
import { initReactI18next } from 'react-i18next';
import common_en from './lib/localization/common_en.json';
import common_de from './lib/localization/common_de.json';

const resources = {
  common: {
    en: { common: common_en },
    de: { common: common_de },
  },
};

i18n.use(initReactI18next).init({
  resources,
  lng: 'en',
  fallbackLng: 'en',
  interpolation: {
    escapeValue: false,
  },
});

export default i18n;
