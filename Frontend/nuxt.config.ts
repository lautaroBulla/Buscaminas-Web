import tailwindcss from "@tailwindcss/vite";

// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
  ssr: false,
  compatibilityDate: '2025-05-15',
  devtools: { enabled: true },
  css: ['~/assets/css/main.css', '~/assets/css/themes/classicTheme.css', '~/assets/css/themes/darkTheme.css'],
  modules: ['@nuxt/eslint', '@nuxt/ui', '@nuxtjs/i18n', '@nuxtjs/color-mode'],

  nitro: {
    preset: 'static'
  },

  app: {
    head: {
      title: 'Buscaminas',
      meta: [
        { name: 'viewport', content: 'width=device-width, initial-scale=1' },
        { charset: 'utf-8' },
        { name: 'description', content: 'Buscaminas game built with Nuxt 3 and Tailwind CSS' },
        { name: 'keywords', content: 'buscaminas, minesweeper, game, juego' },
        { name: 'author', content: 'Lautaro Portillo Bulla' },
      ],
      link: [
        { rel: 'icon', type: 'image/x-icon', href: '/favicon.ico' }
      ]
    }
  },

  colorMode: {
    classSuffix: '',
    preference: 'system', 
    fallback: 'dark', 
    storageKey: 'nuxt-color-mode', 
    storage: 'cookie',
  },

  i18n: {
    lazy: true,
    langDir: 'locales',
    strategy: 'no_prefix',
    locales: [
      { code: 'en', file: 'en.json', name: 'English' },
      { code: 'es', file: 'es.json', name: 'Spanish' }
    ],
    defaultLocale: 'en',
    detectBrowserLanguage: {
      useCookie: true,
      cookieKey: 'i18n_locale',
      fallbackLocale: 'en',
      alwaysRedirect: false,
      redirectOn: 'root' 
    }
  },

  vite: {
    plugins: [
      tailwindcss()
    ],
    server: {
      watch: {
        usePolling: true
      }
    }
  },
  
  runtimeConfig: {
    public: {
      apiBaseUrl: process.env.API_URL_BACKEND,
      secretHash: process.env.SECRET_HASH
    }
  }
})