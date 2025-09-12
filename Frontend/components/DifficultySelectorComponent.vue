<script setup>
  //componente que permite seleccionar la dificultad del juego
  import { ref, computed } from 'vue';

  import faceEasyClassic from '~/assets/img/themes/classicTheme/faceEasy.png';
  import faceIntermediateClassic from '~/assets/img/themes/classicTheme/faceIntermediate.png';
  import faceExpertClassic from '~/assets/img/themes/classicTheme/faceExpert.png';
  import rowsClassic from '~/assets/img/themes/classicTheme/rows.png';
  import colsClassic from '~/assets/img/themes/classicTheme/cols.png';
  import mineClassic from '~/assets/img/themes/classicTheme/mine.png';
  import updateClassic from '~/assets/img/themes/classicTheme/update.png';

  import faceEasyDark from '~/assets/img/themes/darkTheme/faceEasyDark.png';
  import faceIntermediateDark from '~/assets/img/themes/darkTheme/faceIntermediateDark.png';
  import faceExpertDark from '~/assets/img/themes/darkTheme/faceExpertDark.png';
  import rowsDark from '~/assets/img/themes/darkTheme/rowsDark.png';
  import colsDark from '~/assets/img/themes/darkTheme/colsDark.png';
  import mineDark from '~/assets/img/themes/darkTheme/mineDark.png';
  import updateDark from '~/assets/img/themes/darkTheme/updateDark.png';
  
  defineProps({
    modelValue: {
      type: String,
      required: true,
    }
  })

  const { currentTheme } = useCurrentTheme();
  const { isMobile } = useIsMobile();

  const imgByTheme = {
    classicTheme: {
      easy: {
        face: faceEasyClassic
      },
      intermediate: {
        face: faceIntermediateClassic
      },
      expert: {
        face: faceExpertClassic
      },
      rows: rowsClassic,
      cols: colsClassic,
      mine: mineClassic,
      update: updateClassic
    },
		darkTheme: {
      easy: {
        face: faceEasyDark
      },
      intermediate: {
        face: faceIntermediateDark
      },
      expert: {
        face: faceExpertDark
      },
      rows: rowsDark,
      cols: colsDark,
      mine: mineDark,     
      update: updateDark
		}	
  };

  //se obtiene el tema actual del board
	const currentThemeComputed = computed(() => {
    return currentTheme.value;
  });

  // se definen dos emits, uno para la dificultad y otro para los valores personalizados
  const emit = defineEmits(['update:modelValue', 'update:customValues']);
  const customRows = ref(null);
  const customCols = ref(null);
  const customMines = ref(null);

  const selectDifficulty = (difficulty) => {
    if (difficulty === 'custom') {
      customRows.value = null;
      customCols.value = null;
      customMines.value = null;
    }
    emit('update:modelValue', difficulty);
  }

  // datos por defecto para la dificultad personalizada
  const maxMines = computed(() => Math.floor((customRows.value * customCols.value) / 3))
  const minRowsAndCols = 5;
  const maxRowsAndCols = 50;

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
      'update:customValues',
      {
        rows: customRows.value,
        cols: customCols.value,
        mines: customMines.value
      }
    )
  }

  /*Nescesario para definir cuando es mobile y no, en caso de ser mobile no se mostrara custom
  y en caso de cambiar de pc a mobile setear la dificultad a easy
  */
  const maxDifficulty = ref(isMobile.value ? 2 : 3);
  const minDifficulty = ref(0);
  watch(isMobile, (newValue) => {
    if (newValue) {
      maxDifficulty.value = 2;
      minDifficulty.value = 0;
      difficulty.value = 'easy';
      changeDifficulty(0);
    } else {
      maxDifficulty.value = 3;
      minDifficulty.value = 0;
    }
  });
  
  const index = ref(0);
  const difficultys = ['easy', 'intermediate', 'expert', 'custom'];
  const difficulty = ref('easy');
  const changeDifficulty = (newIndex) => {
    if (newIndex > maxDifficulty.value) {
      index.value = minDifficulty.value;
    } else if (newIndex < minDifficulty.value) {
      index.value = maxDifficulty.value;
    } else {
      index.value = newIndex;
    }
    difficulty.value = difficultys[index.value];
    selectDifficulty(difficulty.value);
  }
  
</script>

<template>

  <div class="border-external-sm flex flex-col space-y-2 w-[250px] md:w-[325px]">
    <div class="border-sm">
      <div class="border-internal-sm flex flex-row justify-between items-center px-1">

        <div class="button-border"> 
          <button class="button-sm w-[20px] h-[20px]" 
                  @click="changeDifficulty(index - 1)">
            <
          </button>
        </div>

        <label
          v-if="difficulty !== 'custom'"
          :key="difficulty"
          class="text text-color flex flex-row items-center gap-x-2"
          :title="$t(`difficultySelector.${difficulty}Title`)"
        >
          <img
            :src="imgByTheme[currentThemeComputed][difficulty].face"
            class="w-[15px] h-[15px]"
          />
          {{ $t(`difficultySelector.${difficulty}`) }}
          <img
            :src="imgByTheme[currentThemeComputed][difficulty].face"
            class="w-[15px] h-[15px]"
          />
        </label>
        <div v-if="difficulty === 'custom'" class="hidden md:flex md:items-center md:gap-x-2">
          <div class="flex flex-row items-center">
            <img
              :src="imgByTheme[currentThemeComputed].rows"
              class="w-[15px] h-[15px]"
              :title="$t('difficultySelector.rowsTitle')"
            />
            <div class="border-sm">
              <input 
                v-model.number="customRows"
                type="number" 
                class="border-internal-sm w-[40px] h-[30px] text-color"
              >
              </input>
            </div>
          </div>
          <div class="flex flex-row items-center">
            <img
              :src="imgByTheme[currentThemeComputed].cols"
              class="w-[15px] h-[15px]"           
              :title="$t('difficultySelector.colsTitle')"
            />
            <div class="border-sm">
              <input 
                v-model.number="customCols"
                type="number" 
                class="border-internal-sm w-[40px] h-[30px] text-color"
              >
              </input>
            </div>
          </div>
          <div class="flex flex-row items-center">
            <img
              :src="imgByTheme[currentThemeComputed].mine"
              class="w-[15px] h-[15px]"
              :title="$t('difficultySelector.minesTitle')"
            />
            <div class="border-sm">
              <input 
                v-model.number="customMines"
                type="number" 
                class="border-internal-sm w-[40px] h-[30px] text-color"
              >
              </input>
            </div>
          </div>
          <div class="button-border"> 
            <button
              @click="customVlues()" 
              class="text-color button-sm w-[30px] h-[30px]"
              :title="$t('difficultySelector.updateTitle')"
            >
              <img
                :src="imgByTheme[currentThemeComputed].update"
                class="w-[15px] h-[15px]"
              />
            </button>
          </div>
        </div>

        <div class="button-border"> 
          <button class="button-sm w-[20px] h-[20px]" 
                  @click="changeDifficulty(index + 1)">
            >
          </button>
        </div>

      </div>
    </div>
  </div>

</template>

<style scoped>
  /* Para navegadores basados en WebKit (Chrome, Safari, Edge) */
  input[type="number"]::-webkit-inner-spin-button,
  input[type="number"]::-webkit-outer-spin-button {
  -webkit-appearance: none;
  margin: 0;
  }

  /* Para Firefox */
  input[type="number"] {
  -moz-appearance: textfield;
  }
</style>