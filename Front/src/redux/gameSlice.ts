import { createSlice } from '@reduxjs/toolkit'
import { GameMap, GameState } from './types';

export const gameSlice = createSlice({
  name: 'game',
  initialState: {
    fields: [[]]
  } satisfies GameState as GameState,
  reducers: {
    initMap: (state, action) => {
      let gMap = action.payload as GameMap
      let map = [];
      let j = 0;
      for (let i = 0; i < gMap.Height ; i++) {
        let row = [];
        for (let col = 0; col < gMap.Width ; col++) {
          row.push(gMap.Changes[j].BackgroundImageSrc?.indexOf('water') > 0 ? 0 : 1);
          j++;
        }
        map.push(row);
      }
      state.fields = map;
    },
    toggle: (state, action) => {
        const { row, col } = action.payload;
        const val = state.fields[row][col];
        state.fields[row][col] = val == 0 ? 1 : 0;
    }
  },
})

export const { initMap, toggle } = gameSlice.actions

export default gameSlice.reducer