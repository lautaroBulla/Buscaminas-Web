<script setup>
  import { ref, onMounted } from 'vue';

  const { isMobile } = useIsMobile();

  onMounted(async () => {
    await changeDifficulty();
  });
  
  const { t } = useI18n();

  const { findByDifficulty, findByDifficultyUser } = useGame();
  const difficulty = ref('easy');
  const rows = ref(9);
  const cols = ref(9);
  const mines = ref(10);
  const page = ref(1);
  const games = ref([]);
  const totalPages = ref(null);
  const globalRanking = ref(true);
  const orderByTime = ref(true);
  const position = ref(null);
  const totalPositions = ref(null);
  const areGames = ref(false);

  const loading = ref(false);
  const loadingChange = ref(true);
  const errorMessage = ref(null);

  const setDefaultValues = (newPage) => {
    page.value = newPage ? newPage : 1;
    indexGame.value = 0;
    totalPages.value = null;
  }
  const setCustomValues = async ({ rows: newRows, cols: newCols, mines: newMines, difficulty: newDifficulty }) => {
    rows.value = newRows;
    cols.value = newCols;
    mines.value = newMines;
    difficulty.value = newDifficulty;
    loadingChange.value = false;
    await changeDifficulty();
  }
  const changePage = async (newPage) => {
    loadingChange.value = false;
    await changeDifficulty(newPage);
  }
  const changeRanking = async () => {
    loadingChange.value = false;
    globalRanking.value = !globalRanking.value;
    await changeDifficulty();
  }
  const changeOrder = async (newOrder) => {
    loadingChange.value = false;
    if (orderByTime.value !== newOrder) {
      orderByTime.value = newOrder;
      await changeDifficulty();
    }
  }

  const changeDifficulty = async (newPage) => {
    loading.value = false;
    areGames.value = false;
    setDefaultValues(newPage);
    try {
      let response;
      if (globalRanking.value) {
        response = await findByDifficulty(rows.value, cols.value, mines.value, page.value, 10, orderByTime.value);
      } else {
        response = await findByDifficultyUser(rows.value, cols.value, mines.value, page.value, 10, orderByTime.value);
      }
      games.value = response.games;
      totalPages.value = response.totalPages;
      
      if (games.value.length > 0) areGames.value = true;
      if (isMobile.value) await getGame();
      
      position.value = response.myPosition.position;
      totalPositions.value = response.myPosition.total;

      rows.value = response.rows;
      cols.value = response.cols;
      mines.value = response.mines;
    } catch (error) {
      errorMessage.value = t('ranking.errorMessage');
    }
    loading.value = true;
    loadingChange.value = true;
  }
  
  const indexGame = ref(0);
  const game = ref(null);
  const rankingGame = ref(1);
  const handleLookStats = (index) => {
    indexGame.value = index;
    getGame();
  };
  const getGame = () => {
    if (games.value.length > 0) {
      const globalIndex = (page.value - 1) * 10 + 1;
      game.value = games.value[indexGame.value];
      rankingGame.value = globalIndex + indexGame.value;
    } else {
      game.value = null;
      rankingGame.value = null;
    }
  }
</script>

<template>

  <div class="w-full flex justify-center">
    <div v-if="loading === false">
      <MinePixelReveal 
        class="absolute inset-0 flex items-center justify-center"
        :width="'3px'"
        :height="'3px'"
      />
    </div>
    <div v-else class="w-full flex flex-col max-w-5xl p-4 gap-y-2">  
      
      <TableHeaderComponent 
        :rows="rows"
        :cols="cols"
        :mines="mines"
        :areGames="areGames"
        :difficulty="difficulty"
        :globalRanking="globalRanking"
        :orderByTime="orderByTime"
        :position="position"
        :totalPositions="totalPositions"
        @change="setCustomValues"
        @changeRanking="changeRanking"
        @changeOrder="changeOrder"
      />

      <div v-if="errorMessage" class="text-center">
        {{ errorMessage }}
      </div>
      <div v-else>
        <TableComponent 
          v-if="loadingChange"
          :games="games"
          :page="page"
          :totalPages="totalPages"
          :globalRanking="globalRanking"
          @changePage="changePage"
          @lookStats="handleLookStats"
        />
  
        <StatsComponent
          v-if="isMobile"
          :game="game"
          :rankingGame="rankingGame"
          class="mt-4"
        />
      </div>
    </div>
  </div>

</template>