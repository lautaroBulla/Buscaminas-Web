<script setup>
  const props = defineProps({
    rows: {
      type: Number,
      requiered: true
    },
    cols: {
      type: Number,
      requiered: true
    },
    mines: {
      type: Number,
      requiered: true
    },
    areGames: {
      type: Boolean,
      requiered: true
    },
    difficulty: {
      type: String,
      requiered: true
    },
    globalRanking: {
      type: Boolean,
      requiered: true
    },
    orderByTime: {
      type: Boolean,
      requiered: true
    },
    position: {
      type: Number
    },
    totalPositions: {
      type: Number
    }
  })

  const { user, isAuthReady } = useAuth();

  const emit = defineEmits(['change', 'changeRanking', 'changeOrder']);
  const customRows = ref(props.rows);
  const customCols = ref(props.cols);
  const customMines = ref(props.mines);

  const difficultys = ['easy', 'intermediate', 'expert'];

  const selectDifficulty = (difficultyFunction) => {
    switch (difficultyFunction) {
      case "easy":
        customRows.value = 9; customCols.value = 9; customMines.value = 10; break;
      case "intermediate":
        customRows.value = 16; customCols.value = 16; customMines.value = 40; break;
      case "expert": 
        customRows.value = 16; customCols.value = 30; customMines.value = 99; break;
    }
    emit('change', 
    {
      rows: customRows.value,
      cols: customCols.value,
      mines: customMines.value,
      difficulty: difficultyFunction
    });
  }
    
  const minRowsAndCols = 5;
  const maxRowsAndCols = 100;
  const customVlues = () => {
    //Validar filas
    if (customRows.value < minRowsAndCols)
      customRows.value = minRowsAndCols;
    if (customRows.value > maxRowsAndCols)
      customRows.value = maxRowsAndCols;

    // Validar columnas
    if (customCols.value < minRowsAndCols)
      customCols.value = minRowsAndCols;
    if (customCols.value > maxRowsAndCols)
      customCols.value = maxRowsAndCols;

    // Validar minas
    const maxMinesValue = Math.floor((customRows.value * customCols.value) / 3);
    const minMinesValue = Math.floor((customRows.value * customCols.value) / 10);
    if (customMines.value < minMinesValue)
      customMines.value = minMinesValue;
    if (customMines.value > maxMinesValue)
      customMines.value = maxMinesValue;
    
    emit(
      'change',
      {
        rows: customRows.value,
        cols: customCols.value,
        mines: customMines.value,
        difficulty: "custom"
    })
  }
</script>

<template>

  <div class="flex flex-col gap-y-4">
    <div class="flex items-center gap-x-3 
                justify-center
                md:flex-row md:items-start md:justify-between"
    >
      <div class="flex flex-row gap-x-2">
        <button 
          v-for="difficultyFor in difficultys"
          class="primaryRanking"
          :class="difficulty === difficultyFor ? 'underline' : ''"
          @click="selectDifficulty(difficultyFor)"
        >
          {{ $t(`difficultySelector.${difficultyFor}`) }}
        </button>
      </div>
  
      <div class="hidden md:flex flex-row gap-x-2">
        <div class="flex flex-row gap-x-1">
          <label>{{ $t('ranking.rows') }}:</label>
          <input 
            class="inputCustom"
            v-model="customRows"
            type="number"
          />
        </div>
        <div class="flex flex-row gap-x-1">
          <label>{{ $t('ranking.cols') }}:</label>
          <input 
            class="inputCustom"
            v-model="customCols"
            type="number"
          />
        </div>
        <div class="flex flex-row gap-x-1">
          <label>{{ $t('ranking.mines') }}:</label>
          <input 
            class="inputCustom"
            v-model="customMines"
            type="number"
          />
        </div>
        <button 
          class="primaryRanking"
          @click.prevent="customVlues()" 
        >
          {{ $t('ranking.update') }}
        </button>
      </div>
    </div>
  
    <div class="border-b-2 border-[#adb5bd]"></div>
  
    <div 
      v-if="areGames"
      class="flex"
      :class="user && isAuthReady === true && position && totalPositions ? 'flex-col gap-y-2 md:flex-row md:justify-between' : 'flex-col gap-y-2 md:flex-row md:justify-end'"
    >
      <div 
        v-if="user && isAuthReady === true && position && totalPositions" 
        class="flex flex-row gap-x-2
                  justify-between"
      >
        <button 
          v-if="globalRanking" 
          class="secondaryRanking"
          @click="emit('changeRanking')"
        >
          {{ $t('ranking.personalRanking') }}
        </button>
        <button 
          v-else 
          class="secondaryRanking"
          @click="emit('changeRanking')"
        >
          {{ $t('ranking.globalRanking') }}
        </button>

        <div class="flex flex-row gap-x-1">
          <span>{{ $t('ranking.position') }}:</span>
          <span>{{ position }}/{{ totalPositions }}</span>
        </div>
      </div>

      <div class="flex flex-row gap-x-2 justify-between">
        <span>{{ $t('ranking.orderBy') }}:</span>
        <div class="flex flex-row gap-x-2">
          <button 
            class="secondaryRanking"
            :class="orderByTime ? 'underline' : ''"
            :disabled="orderByTime"
            @click="emit('changeOrder', true)"
          >
            {{ $t('finishGame.time') }}
          </button>
          <button 
            class="secondaryRanking"
            :class="!orderByTime ? 'underline' : ''"
            :disabled="!orderByTime"
            @click="emit('changeOrder', false)"
          >
            {{ $t('finishGame.efficiency') }}
          </button>
        </div>
      </div>
    </div>
  </div>

</template>