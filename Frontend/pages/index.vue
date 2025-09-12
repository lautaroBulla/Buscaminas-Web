<script setup>
  /*
  Pagina principal de la aplicacion, 
  donde se muestra el tablero de juego y los componentes principales
  */
  import { ref, watch, computed } from 'vue';
  import { useMinesweeper } from '~/composables/useMinesweeper';

  const { currentTheme } = useCurrentTheme();

  const { locale, setLocale } = useI18n();
  const language = ref(locale.value);

  const authBus = useState('authBus', () => ({ logoutCallbacks: [] }));

  onMounted(() => {
    authBus.value.logoutCallbacks.push(resetGame);
  });
  
  //obtengo todas las variables de composable useMinesweeper
  const {
    rows,
    cols,
    mines,
    board,
    revealed,
    flags,
    interrogations,
    gameOver,
    gameWin,
    firstClick,
    boardFirstClick,
    resetGame,
    reveal,
    rightClick,
    seconds,
    firstClickZero,
    interrogationsActivated,
    explotedCell,
    help,
    helpCells,
    messageHelp,
    countHelp,
    click3BV,
    countClicks,
    userBestTime,
    globalBestTime,
    sendingToBackend
  } = useMinesweeper(9, 9, 10);
  resetGame();
    
  const difficulty = ref('easy'); //se setea la dificultad inicial en easy
  /*watch se utiliza para observar si la variable difficulty cambia
  si cambia, se setean los valores de filas, columnas y minas*/
  watch(difficulty, async(newDifficulty) => {
    if (difficulty !== newDifficulty) {
      switch (newDifficulty) {
        case "easy":
          rows.value = 9; cols.value = 9; mines.value = 10; resetGame(); break;
        case "intermediate":
          rows.value = 16; cols.value = 16; mines.value = 40; resetGame(); break;
        case "expert": 
          rows.value = 16; cols.value = 30; mines.value = 99; resetGame(); break;
      }
    }
  })
  //esta funcion es la responsable de setear los valores cuando se selecciona la dificultad personalizada
  const setCustomValues = ({ rows: customRows, cols: customCols, mines: customMines }) => {
    rows.value = customRows;
    cols.value = customCols;
    mines.value = customMines;
    resetGame();
  }

  //logica del modal para mostrarlo o ocualtarlo
  const modalSettings = ref(false);
  const viewSettings = () => {
    modalSettings.value = !modalSettings.value;
  }
  //actualiza los datos del juego cuando se cambia la configuracion
  const updateSettings = async (updateSettingsValues) => {
    const newLocale = updateSettingsValues.language;
    await setLocale(newLocale);
    currentTheme.value = updateSettingsValues.theme;
    firstClickZero.value = updateSettingsValues.firstClickZero;
    interrogationsActivated.value = updateSettingsValues.interrogationsActivated;

    viewSettings();
  }
  /*en caso de que el usuarios termine la partida con victoria
  se muestra el modal de finalizacion del juego*/
  const modalGameFinish = ref(false);
  watch(gameWin, (newValue) => {
    modalGameFinish.value = newValue;
  });
  const viewGameFinish = () => {
    modalGameFinish.value = !modalGameFinish.value;
  }
  
  // Aca se obtiene el valor de las minas restantes, conociendo el total se va a restando por cada flag que se pone
  const remainingMines = computed(() => mines.value - flags.value.flat().filter(cell => cell === true).length);
</script>

<template>
  <div v-if="sendingToBackend === true" class="bg-black opacity-50 absolute inset-0 flex items-center justify-center z-50">
      <MinePixelReveal 
        :width="'3px'"
        :height="'3px'"
      />
  </div>

  <div class="flex flex-col items-center pt-4 space-y-4">

    <div class="md:flex md:gap-x-4">
      <DifficultySelectorComponent
        v-model="difficulty"
        @update:customValues="setCustomValues"
      />
    </div>

    <div class="border-external">
      <div class="border">
        <HeaderComponent
        :remainingMines="remainingMines"
        :seconds="seconds"
        :gameOver="gameOver"
        :gameWin="gameWin"
        @restart-game="resetGame()"
        @view-settings="viewSettings()"
        @help="help()"
        />
        <div class="border-separator"></div>
        <BoardComponent 
          :board="board"
          :revealed="revealed"
          :flags="flags"
          :interrogations="interrogationsActivated ? interrogations : null"
          :explotedCell="explotedCell"
          :helpCells="helpCells"
          :messageHelp="messageHelp"
          @cell-left-click="({row, col}) => { countClicks++; firstClick ? boardFirstClick(row, col) : reveal(row, col) }"
          @cell-right-click="({row, col}) => { countClicks++; rightClick(row, col) }"
        />
      </div>
    </div>

  </div>

  <GameFinishComponent
    v-if="modalGameFinish && sendingToBackend === false"
    :seconds="seconds"
    :countHelp="countHelp"
    :click3BV="click3BV"
    :countClicks="countClicks"
    :userBestTime="userBestTime"
    :globalBestTime="globalBestTime"
    :sendingToBackend="sendingToBackend"
    @close="viewGameFinish"
  />
  <SettingsModal
    v-if="modalSettings"
    :language="language"
    :firstClickZero="firstClickZero"
    :interrogationsActivated="interrogationsActivated"
    @update="updateSettings"
    @close="viewSettings"
  />

</template>