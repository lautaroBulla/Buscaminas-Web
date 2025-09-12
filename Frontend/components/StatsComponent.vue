<script setup>
  //componente para mobile, se vera la informacion del juego seleccionado
  const props = defineProps({
    game: {
      type: Object
    },
    rankingGame: {
      type: Number
    }
  });
</script>

<template>
   <div class="flex justify-center">
    <div v-if="game && rankingGame" class="statsComponent flex flex-col gap-y-3 items-center w-full sm:w-fit sm:px-8 sm:py-2">
      
      <h2 v-if="rankingGame === 1" class="text-xl sm:text-2xl">{{ $t('ranking.bestTime') }}</h2>
      <h2 v-else class="text-xl sm:text-2xl">{{ $t('ranking.randomTime') }} {{ rankingGame }}</h2>
      
      <div class="grid grid-cols-2 gap-y-2 gap-x-8 sm:gap-x-16 text-lg sm:text-xl">
        <div class="flex gap-x-1 justify-between">
          <span class="underline">{{ $t('ranking.username') }}:</span>
          <span>{{ game.user?.username }}</span>
        </div>
        <div class="flex gap-x-1 justify-between">
          <span class="underline">{{ $t('finishGame.time') }}:</span>
          <span>{{ game.seconds }}s</span>
        </div>
        <div class="flex gap-x-1 justify-between">
          <span class="underline">{{ $t('ranking.date') }}:</span>
          <span>{{ new Date(game.createdAt).toLocaleDateString() }}</span>
        </div>
        <div class="flex gap-x-1 justify-between">
          <span class="underline">{{ $t('finishGame.help') }}:</span>
          <span>{{ game.help }}</span>
        </div>
      </div>
      
      <div class="grid grid-cols-3 gap-x-5 sm:gap-x-10 text-lg sm:text-xl">
        <div class="flex gap-x-1">
          <span class="underline" :title="$t('finishGame.3bvTitle')">{{ $t('finishGame.3bv') }}:</span>
          <span>{{ game.n3BV }}</span>
        </div>
        <div class="flex gap-x-1">
          <span class="underline" :title="$t('finishGame.clicksTitle')">{{ $t('finishGame.clicks') }}:</span>
          <span>{{ game.clicks }}</span>
        </div>
        <div class="flex gap-x-1">
          <span class="underline" :title="$t('finishGame.efficiencyTitle')">{{ $t('finishGame.efficiency') }}:</span>
          <span>{{ game.efficiency }}%</span>
        </div>
      </div>

    </div>
  </div>

</template>