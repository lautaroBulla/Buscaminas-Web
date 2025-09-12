export default defineI18nConfig(() => {
  return {
    legacy: false,
    globalInjection: true,
    locale: 'es',
    fallbackLocale: 'es',
    availableLocales: ['en', 'es'],
  }
})