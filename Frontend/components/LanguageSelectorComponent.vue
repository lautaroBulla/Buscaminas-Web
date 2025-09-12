<script setup>
  //componente que se encarga de cambiar el idioma de la aplicacion
  import esFlag from '~/assets/img/es.png'
  import ukFlag from '~/assets/img/uk.png'

  const { locale, setLocale } = useI18n();
  
  const changeLanguage = async () => {
    const newLocale = locale.value === 'en' ? 'es' : 'en';
    await setLocale(newLocale);
  }

  const flagSrc = computed(() => (locale.value === 'en' ? esFlag : ukFlag));
  const flagAlt = computed(() => (locale.value === 'en' ? 'Spanish' : 'English'));
</script>

<template>
  <div class="flex items-center">
      <button
        type="button"
        @click="changeLanguage"
        class="secondary flex items-center gap-x-2"
        :title="$t('languageSelector.changeLanguageTitle')"
      >
        {{ $t('languageSelector.changeLanguage') }}
        <img :src="flagSrc" :alt="flagAlt" class="w-[20px] h-[15px]" />
      </button>
  </div>
</template>
