<script setup>
  import { onMounted } from 'vue';

  const mounted = ref(false);

  onMounted(() => {
    mounted.value = true;
  });

  const { currentTheme } = useCurrentTheme();
</script>

<template>
  <div v-if="mounted === true" class="layout-container">
    <NavComponent />
    <main class="main-content background" :class="`${currentTheme}`">
      <NuxtPage />
    </main>
  </div>
  <MinePixelReveal v-else />
</template>