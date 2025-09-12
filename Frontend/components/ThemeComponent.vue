<script setup>
  //componente para cambiar entre tema claro y oscuro
  import { useColorMode } from '#imports';
  import ligthImg from '~/public/img/themes/light/light.png';
  import darkImg from '~/public/img/themes/dark/dark.png';

  const colorMode = useColorMode();

  const isDark = computed(() => colorMode.value === 'dark');
  
  const toggleDark = () => {
    colorMode.preference = colorMode.value === 'dark' ? 'light' : 'dark'
  }
</script>

<template>
  <div class="flex items-center">
      <button
        @click="toggleDark"
        class="secondary flex items-center gap-x-2"
        :title="$t('themeSelector.themeSelectorTitle')"
      >
        {{ isDark ? 'Light' : 'Dark' }}	
        <img
          :src="isDark ? ligthImg : darkImg"
          alt="Theme Icon"
          class="w-[15px] h-[15px]"
        />
      </button>
  </div>
</template>