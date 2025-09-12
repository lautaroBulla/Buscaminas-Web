/*
Composable encargado de realizar las peticiones relacionadas con los games
*/
export const useGame = () => {
  const { $apiFetch } = useNuxtApp();

  //obtiene el mejor tiempo del usuario, segun la dificultad
  const getMyBestTime = async (rows: number, cols: number, mines: number) => {
    try {
      const data = await $apiFetch('/games/myBestTime', {
        method: 'GET',
        query: {rows, cols, mines}
      })
      return Number(data);
    } catch (error) {
      throw error;
    }
  }

  //obtiene los mejores tiempos globales y del usuario, segun la dificultad
  const getBestTimes = async (rows: number, cols: number, mines: number) => {
    try {
      const data = await $apiFetch('/games/bestTimes', {
        method: 'GET',
        query: {rows, cols, mines}
      })
      data.userBestTime = Number(data.userBestTime);
      data.globalBestTime = Number(data.globalBestTime);

      return data;
    } catch (error) {
      throw error;
    }
  }

  //guarda un game en la base de datos
  const saveGame = async (
    helpLocal: number,
    secondsLocal: number,
    difficultyLocal: string,
    rowsLocal: number, 
    colsLocal: number, 
    minesLocal: number, 
    n3bvLocal: number,
    clicksLocal: number,
    efficiencyLocal: number
  ) => {
    try {
      const res = await $apiFetch('/games', {
        method: 'POST',
        body: {
          help: helpLocal,
          seconds: secondsLocal,
          difficulty: difficultyLocal,
          rows: rowsLocal,
          cols: colsLocal,
          mines: minesLocal,
          n3BV: n3bvLocal,
          clicks: clicksLocal,
          efficiency: efficiencyLocal
        }
      })
      return res;
    } catch (error) {
      throw error;
    }
  }

  //obtiene los datos de las partidas segun la dificultad, en orden de tiempo y efficiencia
  const findByDifficulty = async (
    rows: number,
    cols: number,
    mines: number,
    page: number,
    take: number,
    orderByTime: boolean
  ) => {
    try {
      const data = await $apiFetch('/games/difficulty', {
        method: 'GET',
        query: {rows, cols, mines, page, take, orderByTime}
      })
      return data;
    } catch (error) {
      throw error;
    }
  }

  //obtiene los datos de las partidas del usuario segun la dificultad, en orden de tiempo y efficiencia
  const findByDifficultyUser = async (
    rows: number,
    cols: number,
    mines: number,
    page: number,
    take: number,
    orderByTime: boolean
  ) => {
    try {
      const data = await $apiFetch('/games/difficultyUser', {
        method: 'GET',
        query: {rows, cols, mines, page, take, orderByTime}
      })
      return data;
    } catch (error) {
      throw error;
    }
  }

  return {
    getMyBestTime,
    getBestTimes,
    saveGame,
    findByDifficulty,
    findByDifficultyUser
  }
}