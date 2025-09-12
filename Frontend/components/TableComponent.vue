<script setup>
  import { useColorMode } from '#imports';
  
  import arrowLeftLight from '~/public/img/themes/light/arrowLeftLight.png';
  import arrowLeftLLight from '~/public/img/themes/light/arrowLeftLLight.png';
  import arrowRightLight from '~/public/img/themes/light/arrowRightLight.png';
  import arrowLeftDark from '~/public/img/themes/dark/arrowLeftDark.png';
  import arrowLeftLDark from '~/public/img/themes/dark/arrowLeftLDark.png';
  import arrowRightDark from '~/public/img/themes/dark/arrowRightDark.png'; 

  const props = defineProps({
    games: {
      type: Array,
      required: true
    },
    page: {
      type: Number,
      required: true
    },
    totalPages: {
      type: Number
    },
    globalRanking: {
      type: Boolean,
      requiered: true
    }
  });

  const colorMode = useColorMode();
  const isDark = computed(() => colorMode.value === 'dark');

  const emit = defineEmits(['changePage', 'lookStats']);

  const changePage = (newPage) => {
    emit('changePage', newPage);
  }

  //funcion para setear una clase de color segun la posicion del ranking
  const getRankingClass = (position) => {
    if (position === 1) return isDark.value ? 'ranking-gold-dark' : 'ranking-gold-light'; 
    if (position === 2) return isDark.value ? 'ranking-silver-dark' : 'ranking-silver-light'; 
    if (position === 3) return isDark.value ? 'ranking-bronze-dark' : 'ranking-bronze-light';
    return '';
  };
</script>

<style>
  .ranking-gold-dark td {
    color: #FFD700;
  }

  .ranking-silver-dark td {
    color: #C0C0C0;
  }

  .ranking-bronze-dark td {
    color: #CD7F32;
  }

  .ranking-gold-light td {
    color: #B8860B;
  }

  .ranking-silver-light td {
    color: #708090;
  }

  .ranking-bronze-light td {
    color: #8B4513;
  }
</style>

<template>
  <div>

    <div v-if="games.length > 0" class="table-container">
      <table class="table w-full border-collapse table-fixed">
        <thead>
          <tr>
            <th class="tableHeader w-1/12">#</th>
            <th class="tableHeader md:w-2/12">{{ $t('ranking.username') }}</th>
            <th class="tableHeader md:w-2/12">{{ $t('finishGame.time') }}</th>
            <th class="tableHeader md:hidden">{{ $t('ranking.stats') }}</th>
            <th class="tableHeader hidden md:table-cell">{{ $t('finishGame.efficiency') }}</th>
            <th class="tableHeader hidden md:table-cell">{{ $t('finishGame.3bv') }}</th>
            <th class="tableHeader hidden md:table-cell">{{ $t('finishGame.clicks') }}</th>
            <th class="tableHeader hidden md:table-cell">{{ $t('finishGame.help') }}</th>
            <th class="tableHeader hidden md:table-cell">{{ $t('ranking.date') }}</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="(game, index) in games" :key="game.id" 
            :class="globalRanking ? getRankingClass(index + 1 + ((page-1) * 10)) : ''">
            <td class="tableCell w-1/12">{{ index + 1 + ((page-1) * 10)}}</td>
            <td class="tableCell md:w-2/12">{{ game.user.username }}</td>
            <td class="tableCell md:w-2/12">{{ game.seconds }}s</td>
            <td 
              class="tableCell hover:cursor-pointer hover:opacity-80 md:hidden"
              @click="emit('lookStats', index)"
            >
              {{ $t('ranking.lookStats') }}
            </td>
            <td class="tableCell hidden md:table-cell">{{ game.efficiency }}%</td>
            <td class="tableCell hidden md:table-cell">{{ game.n3BV }}</td>
            <td class="tableCell hidden md:table-cell">{{ game.clicks }}</td>
            <td class="tableCell hidden md:table-cell">{{ game.help }}</td>
            <td class="tableCell hidden md:table-cell">{{ new Date(game.createdAt).toLocaleDateString() }}</td>
          </tr>
        </tbody>
      </table>
      <div v-if="totalPages > 1" class="pagination flex flex-row items-center justify-center gap-x-5">
        <button 
          class="secondary-sm md:secondary"
          :disabled="page === 1 ? true : false"
          @click="changePage(1)"
        >
          <img 
            :src="isDark ? arrowLeftLDark : arrowLeftLLight"
            class="w-[12px] h-[12px] md:w-[14px] md:h-[14px]"      
          >
        </button>
        <button 
          class="secondary-sm md:secondary" 
          :disabled="page === 1 ? true : false"
          @click="changePage(page-1)"
        >
          <img 
            :src="isDark ? arrowLeftDark : arrowLeftLight"
            class="w-[12px] h-[12px] md:w-[14px] md:h-[14px]"
          >
        </button>
        <button 
          v-if="page - 4 > 0 && page === totalPages" 
          class="buttonPagination"
          @click="changePage(page-4)"
        >
          {{ page - 4 }}
        </button>
        <button 
          v-if="page - 3 > 0 && (page === totalPages || page === totalPages-1)" 
          class="buttonPagination"
          @click="changePage(page-3)"
        >
          {{ page - 3 }}
        </button>
        <button 
          v-if="page - 2 > 0" 
          class="buttonPagination"
          @click="changePage(page-2)"
        >
          {{ page - 2 }}
        </button>
        <button 
          v-if="page - 1 > 0" 
          class="buttonPagination"
          @click="changePage(page-1)"
        >
          {{ page - 1 }}
        </button>

        <span class="underline">
          {{ page }}
        </span>

        <button 
          v-if="page + 1 <= totalPages" 
          class="buttonPagination"
          @click="changePage(page+1)"
        >
          {{ page + 1 }}
        </button>
        <button 
          v-if="page + 2 <= totalPages" 
          class="buttonPagination"
          @click="changePage(page+2)"
        >
          {{ page + 2 }}
        </button>
        <button 
          v-if="page + 3 <= totalPages && (page === 1 || page === 2)" 
          class="buttonPagination"
          @click="changePage(page+3)"
        >
          {{ page + 3 }}
        </button>
        <button 
          v-if="page + 4 <= totalPages && (page === 1)" 
          class="buttonPagination"
          @click="changePage(page+4)"
        >
          {{ page + 4 }}
        </button>
        <button 
          class="secondary-sm md:secondary" 
          :disabled="page === totalPages ? true : false"
          @click="changePage(page+1)"
        >
          <img 
            :src="isDark ? arrowRightDark : arrowRightLight"
            class="w-[12px] h-[12px] md:w-[14px] md:h-[14px]"
          >
        </button>
      </div>
    </div>

    <p v-else class="text-center">{{ $t('ranking.noGames') }}</p>
  </div>
</template>