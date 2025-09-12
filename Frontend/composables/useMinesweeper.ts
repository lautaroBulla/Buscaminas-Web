import { form } from '#build/ui';
import { ref } from 'vue';
import SHA256 from 'crypto-js/sha256';
import Base64 from 'crypto-js/enc-base64';

export function useMinesweeper(initialRows = 9, initialCols = 9, initialMines = 10) {
  const { t } = useI18n();
  const { isMobile } = useIsMobile();
  const { user } = useAuth();
  const { getMyBestTime, getBestTimes, saveGame } = useGame();

  const rows = ref<number>(initialRows); //rows del tablero
  const cols = ref<number>(initialCols); //cols del tablero
  const mines = ref<number>(initialMines); //mines del tablero

  const board = ref<(number | 'M')[][]>([]); //board contendra los valores de las celdas, tanto numericos como las minas
  const revealed = ref<boolean[][]>([]); //esta matriz llevara la logica a las celdas ya reveladas
  const flags = ref<boolean[][]>([]); //esta matriz llevara la logica a las celdas marcadas como flags
  const interrogations = ref<boolean[][]>([]); //esta matriz llevara la logica a las celdas marcadas como interrogations
  const gameOver = ref(false); //en caso de que pierda
  const gameWin = ref(false); //en caso de que gane 
  const firstClick = ref(true); //para saber si es el primer click de la partida

  const firstClickZero = ref(true); //opcion de juego, para que el primer click sea en una celda vacia
  const interrogationsActivated = ref(false); //opcion de juego, para habilidar el uso de ?

  const seconds = ref(0); //lleva ek timepo de la partida
  let intervalId: ReturnType<typeof setInterval> | null = null;

  const directions = [[-1, -1],[-1, 0],[-1, 1],[0, -1],[0, 1],[1, -1],[1, 0],[1, 1]]; //celdas adyacentes de una celda

  const explotedCell = ref<{row: number, col: number} | null>(null); //necesario para saber cual mina fue la que exploto, para mostrarla al con fondo rojo

  const helpCells = ref<{ row: number; col: number }[]>([]); //las celdas que se le mostraran al usuario como ayuda
  const lastHelp = ref<{ row: number; col: number }>(); //controla cual fue la ultima ayuda
  const countHelp = ref(0); //contador de ayudas
  const messageHelp = ref<string | null>(null); //mensaje para cuando no ha iniciado el game o no hay ayudas

  const click3BV = ref(0);
  const countClicks = ref(0);

  const userBestTime = ref<number | null>(null);
  const globalBestTime = ref<number | null>(null);

  const sendingToBackend = ref(false);

  //verifica que sea una celda valida del tablero
  function isValidCell(r: number, c: number): boolean {
    return r >= 0 && r < rows.value && c >= 0 && c < cols.value;
  }

  //se setean las variables para el inicio de un nuevo juego e inicia un nuevo board
  function resetGame () {
    explotedCell.value = null;
    helpCells.value = [];
    countHelp.value = 0;
    click3BV.value = 0;
    countClicks.value = 0;
    userBestTime.value = null;
    globalBestTime.value = null;
    stopTime();
    seconds.value = 0;
    initBoard();
  }

  //se setean los valores iniciales del tablero
  function initBoard() {
    board.value = Array.from({length: rows.value}, () => Array(cols.value).fill(0));
    revealed.value = Array.from({length: rows.value}, () => Array(cols.value).fill(false));
    flags.value = Array.from({length: rows.value}, () => Array(cols.value).fill(false));
    interrogations.value = Array.from({length: rows.value}, () => Array(cols.value).fill(false));   
    gameOver.value = false;
    gameWin.value = false;
    firstClick.value = true;    
  }

  //esta funcion sirve para que el jugador no pierda en el primer moviminto en caso de apretar una mina
  function boardFirstClick(row: number, col: number) {
    firstClick.value = false;

    let bvv = 0;
    do {
      placeMines(row, col);
      bvv = calculate3BV();
      if (bvv <= 1) {
        initBoard(); 
      }
    } while (bvv <= 1);

    click3BV.value = bvv;
    startTime();
    reveal(row, col);
    firstClick.value = false;
  }

  /*esta funcion se encarga de calcular la cantidad de clicks minimos necesarios para completar
  el tablero, de esta manera obtener la eficiencia de clicks de la partida
  */
  function calculate3BV() {
    const visited = Array.from({ length: rows.value }, () =>
      Array(cols.value).fill(false)
    );

    const dfs = (r: number, c: number) => {
      visited[r][c] = true;
      for (const [dr, dc] of directions) {
        const nr = r + dr;
        const nc = c + dc;
        if (
          isValidCell(nr, nc) &&
          board.value[nr][nc] === 0 &&
          !visited[nr][nc]
        ) {
          dfs(nr, nc);
        }
      }
    };

    let zeroZones = 0;
    let isolatedNumbers = 0;

    for (let r = 0; r < rows.value; r++) {
      for (let c = 0; c < cols.value; c++) {
        const cell = board.value[r][c];

        // Buscar zonas de ceros
        if (cell === 0 && !visited[r][c]) {
          dfs(r, c);
          zeroZones++;
        }

        // Buscar números aislados (no adyacentes a un cero)
        if (cell !== 'M' && cell !== 0) {
          let adjacentToZero = false;
          for (const [dr, dc] of directions) {
            const nr = r + dr;
            const nc = c + dc;
            if (isValidCell(nr, nc) && board.value[nr][nc] === 0) {
              adjacentToZero = true;
              break;
            }
          }
          if (!adjacentToZero) isolatedNumbers++;
        }
      }
    }

    return zeroZones + isolatedNumbers;
  }

  /*coloca las minas en el tablero, evitando colocar una mina en la cell incial
  en caso de que firstClickZero (opcion de juego) sea true, lo que va a hacer es setear esa primer cell como vacia
  evitando que se coloquen minas al rededor de la misma
  */
  function placeMines(row: number, col: number) {
    let mineCount = 0;
    const invalidCells = new Set();

    invalidCells.add(`${row}-${col}`);
    if (firstClickZero.value) { 
        directions.forEach(([dr, dc]) => {
            const r = dr + row;
            const c = dc + col;
            if (isValidCell(r, c)) {
                invalidCells.add(`${r}-${c}`)
            }
        });
    }
    
    while (mineCount < mines.value) {
        const r = Math.floor(Math.random() * rows.value);
        const c = Math.floor(Math.random() * cols.value);
        const key = `${r}-${c}`
        if (board.value[r][c] !== 'M' && !invalidCells.has(key)) {
            board.value[r][c] = 'M';
            mineCount++;
        }
    }

    adjacentMines();
  }

  //recorre cada casilla del tablero, en caso de no ser una mina, cuenta las minas adyacentes y setea el numero
  function adjacentMines() {
    for (let r = 0; r < rows.value; r++) {
      for (let c = 0; c < cols.value; c++) {
        if (board.value[r][c] === 'M') continue;

        let count = 0;
        directions.forEach(([dr, dc]) => {
          const nr = r + dr;
          const nc = c + dc;
          if (isValidCell(nr, nc) && board.value[nr][nc] === 'M') {
              count++;
          }
        });

        board.value[r][c] = count;
      }
    } 
  }

  //funciones del tiempo de la partida
  function startTime() {
    if (intervalId !== null) return // evitar múltiples timers

    intervalId = setInterval(() => {
      seconds.value += 10;
    }, 10)
  }
  function stopTime() {
    if (intervalId !== null) {
      clearInterval(intervalId)
      intervalId = null
    }
  }

  /*funcion que se encargara de la revelacion de las minas
  por lo tanto a su vez contemplara, que no se puede revelar una vez el juego terminado o una cell con flag marcada

  en caso de que sea una cell no revelada 
  verificara si el usuario perdio, al revelar una cell con mina
  en caso de revelar una celda vacia, lo que hara es autollamarse con el valor de las celdas adyacentes
  por ultimo checkea si el usuario gano

  en caso de que sea un cell revelada
  cuenta las flags a su alrededor, si estas son igual al numero de la cell
  revelara las celdas adyacentes que esten sin revelar
  las flags no la revelara por el if inical de la funcion
  */
  async function reveal(row: number, col: number) {
    if ( flags.value[row][col] || gameOver.value || gameWin.value ) return

    if (!revealed.value[row][col]) {
      // lógica si no estaba revelada
      revealed.value[row][col] = true;
      
      if (board.value[row][col] === 'M') {
        stopTime();
        explotedCell.value = { row, col };
        gameOver.value = true;
        revealGameOver();
        return;
      }

      if (board.value[row][col] === 0) {
        directions.forEach(([dr, dc]) => {
            const nr = row + dr;
            const nc = col + dc;
            if (isValidCell(nr, nc)) {
                reveal(nr, nc);
            }
        });
      }

      if (gameWin.value === false) {
        checkWin();
      }
      return;
    }

    // ahora manejamos el caso de celda ya revelada
    let adjacentFlags = 0;
    directions.forEach(([dr, dc]) => {
      const nr = row + dr;
      const nc = col + dc;
      if (isValidCell(nr, nc) && flags.value[nr][nc]) {
        adjacentFlags++;
      }
    });

    if (adjacentFlags > 0 && board.value[row][col] === adjacentFlags) {
      directions.forEach(([dr, dc]) => {
        const nr = row + dr;
        const nc = col + dc;
        if (isValidCell(nr, nc) && !revealed.value[nr][nc]) {
          reveal(nr, nc);
        }
      });
    }
  }

  //simplemente revela todas las minas sin revelar cuando el usuario pierde
  function revealGameOver() {
    for (let r=0; r<rows.value; r++){
      for (let c=0; c<cols.value; c++){
        if (board.value[r][c] === 'M' && !flags.value[r][c]) {
          revealed.value[r][c] = true;
        }
      }
    }
  }

  /*verifica si todas las casillas con valor diferente a M han sido reveladas, en ese caso el usuario gano la partida
  si gano la partida, y existe un usuario autenticado, se manda la informacion del juego al backend para guardarla
  y luego se obtiene le mejor tiempo para mostrar en el componente GameFinish
  en caso de que no haya un usuario autenticado, se guarda la informacion del juego en una cookie,
  para ser guardada en el backend luego de que el usuario se autentique, en caso de que no expirara
  */
  async function checkWin() {
    if (board.value.flat().filter(cell => cell !== 'M').length === revealed.value.flat().filter(cell => cell === true).length){
      stopTime();
      revealGameWin();
      const difficulty = rows.value === 9 && cols.value === 9 && mines.value === 10 ? 'easy' :
        rows.value === 16 && cols.value === 16 && mines.value === 40 ? 'intermediate' :
        rows.value === 16 && cols.value === 30 && mines.value === 99 ? 'expert' :
        'custom';
      if (user.value !== null) {
        sendingToBackend.value = true;
        if (countClicks.value < click3BV.value) {
          countClicks.value = click3BV.value; 
        }
        await saveGame(countHelp.value, seconds.value / 1000, difficulty, rows.value, cols.value, mines.value, click3BV.value, countClicks.value, Math.round(100 * (click3BV.value / countClicks.value)));
        const data = await getBestTimes(rows.value, cols.value, mines.value);    
        userBestTime.value = data.userBestTime;
        globalBestTime.value = data.globalBestTime;
        sendingToBackend.value = false;
      } else {
        if (countClicks.value < click3BV.value) {
          countClicks.value = click3BV.value; 
        }
        const gameData = {
          help: countHelp.value,
          seconds: seconds.value / 1000,
          difficulty,
          rows: rows.value,
          cols: cols.value,
          mines: mines.value,
          n3BV: click3BV.value,
          clicks: countClicks.value,
          efficiency: Math.round(100 * (click3BV.value / countClicks.value))
        };

        const secret = useRuntimeConfig().public.secretHash;
        const payload = JSON.stringify(gameData);
        const hash = Base64.stringify(SHA256(payload + secret));

        useCookie<GameToSave>('gameToSave', {
          expires: new Date(Date.now() + 1000 * 60 * 10)
        }).value = {
          ...gameData,
          hash
        };
      }
      gameWin.value = true;
    }
  }

  //esta funcion seimplemente los que hace es marcar las flags faltantes al ganar la partida
  function revealGameWin() {
    for (let r=0; r<rows.value; r++){
      for (let c=0; c<cols.value; c++){
        if (board.value[r][c] === 'M' && !flags.value[r][c]) {
          flags.value[r][c] = true;
        }
      }
    }
  }


  function rightClick(row: number, col: number) {
    if (revealed.value[row][col] || gameOver.value || gameWin.value) return;
    // sirve para ir alterando la casilla con las marcas flags e interrogations
    if (!isMobile.value) { //si no es mobile se dejara usar ?
      if (interrogationsActivated.value) {
        if (!flags.value[row][col] && !interrogations.value[row][col]){ 
          flags.value[row][col] = true;
        } else if (flags.value[row][col]) {
          flags.value[row][col] = false;
          interrogations.value[row][col] = true;
        } else if (interrogations.value[row][col]) {
          interrogations.value[row][col] = false;
        }
      } else if (!interrogationsActivated.value) {
        flags.value[row][col] = !flags.value[row][col];
      }
    } else { //en caso de ser mobile solo se alternara entre flag y no flag
      flags.value[row][col] = !flags.value[row][col];
    }
  }

  /*hace el calculo de las celdas con valor de ser ayuda
  y te devuelve una al azar
  */
  function getRandomRevealedCell() {
    const revealedCells: { row: number; col: number}[] = [];

    for (let r=0; r<rows.value; r++) {
      for (let c=0; c<cols.value; c++) {
        const cellValue = board.value[r][c];

        if (!revealed.value[r][c]) continue;
        if (cellValue === 'M' || cellValue === 0) continue;

        let flagsAround = 0;
        let hiddenUnFlagged = 0;

        directions.forEach(([dr, dc]) => {
          const nr = r + dr;
          const nc = c + dc;
          if (!isValidCell(nr, nc)) return;

          if (flags.value[nr][nc]) {
            flagsAround++;
          } else if (!revealed.value[nr][nc]) {
            hiddenUnFlagged++;
          }
        });

        const totalHidden = flagsAround + hiddenUnFlagged;

        /*
        las celdas consideradas como ayuda van a ser 2 tipos
        1. las que tienen la cantidad de celdas adyacentes sin revelar igual al valor de la celda,
        con la intencion de que marque las celdas con una flag
        2. las que tienen la cantidad de celdas adyacentes con flag igual al valor de la celda y
        tienes celdas sin revelar sin marcar como flag,
        con la intencion de que revele las celdas 
        */
        if (
          (totalHidden === cellValue && hiddenUnFlagged > 0) ||
          (flagsAround === cellValue && hiddenUnFlagged > 0)
        ) {
          revealedCells.push({ row: r, col: c });
        }
      }
    }

    if (revealedCells.length === 0) return null;

    if (lastHelp.value) { //lastHelp sireve para devolver la misma ayuda, si no fue utilizada
      const { row, col } = lastHelp.value;
      const sameHelp = revealedCells.find(cell => cell.row === row && cell.col === col)
      if (sameHelp) {
        return sameHelp;
      }
    }

    countHelp.value += 1;

    const randomIndex = Math.floor(Math.random() * revealedCells.length);
    return revealedCells[randomIndex];
  } 

  /*obtiene la celda de ayuda, de la funcion anterior
  y crea el conjunto de celdas de ayuda que va a ser la celda devuela mas sus adyacentes
  las cuales se marcaran en el board, para informarle al usuario de la ayuda.
  */
  function help() { 
    const helpCellFunction = getRandomRevealedCell();
    helpCells.value = [];
    
    if (helpCellFunction) {
      helpCells.value.push({ row: helpCellFunction.row, col: helpCellFunction.col});
      directions.forEach(([dr, dc]) => {
        const nr = helpCellFunction.row + dr;
        const nc = helpCellFunction.col + dc;
        
        if (isValidCell(nr, nc)) {
          helpCells.value.push({ row: nr, col: nc});
        }
      })
      
      lastHelp.value = helpCells.value[0];

      setTimeout(() => {
        helpCells.value = []
      }, 2000);
    } else {
      messageHelp.value = t('helpMessage.noHelp'); //en caso de que no hayan celdas de ayuda
      setTimeout(() => {
        messageHelp.value = null;
      }, 1000);
    }

    if (firstClick.value) {
      messageHelp.value = t('helpMessage.firstClick'); //si el usuario no ha arrancado la partida
      setTimeout(() => {
        messageHelp.value = null;
      }, 1000);
    }
  }
  
  return {
    rows,
    cols,
    mines,
    board,
    revealed,
    flags,
    interrogations,
    gameWin,
    gameOver,
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
  }
}
