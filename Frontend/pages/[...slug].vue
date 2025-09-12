<script setup>
  setResponseStatus(404);

  useHead({
    title: 'Página no encontrada - 404',
    meta: [
      { name: 'robots', content: 'noindex' }
    ]
  });
</script>

<template>
  <div class="flex flex-col items-center mt-10">
    <h1 class="text-3xl mb-2">404 Not Found</h1>
    <button class="primary">
      <NuxtLink 
        to="/" 
      >
        {{ $t('error.goHome') }}
      </NuxtLink>
    </button>

  </div>
</template>
