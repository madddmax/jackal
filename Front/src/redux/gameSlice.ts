import { createSlice } from '@reduxjs/toolkit'
import { FieldState, GameMap, GameState, GameMove } from './types';

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
        let row: FieldState[] = [];
        for (let col = 0; col < gMap.Width ; col++) {
          if (!gMap.Changes[j].BackgroundImageSrc) {
            row.push({ backColor: gMap.Changes[j].BackgroundColor });
          }
          else row.push({ image: gMap.Changes[j].BackgroundImageSrc?.indexOf('water') > 0 ? 0 : 1});
          j++;
        }
        map.push(row);
      }
      state.fields = map;
    },
    highlightMoves: (state, action) => {
      let moves = action.payload as GameMove[];
      moves.forEach(move => {
        const cell = state.fields[move.To.X][move.To.Y];
        cell.moveNum = move.MoveNum;
      });
    },
    toggle: (state, action) => {
        const { row, col } = action.payload;
        const val = state.fields[row][col];
        state.fields[row][col] = { image: val.image == 0 ? 1 : 0 };
    }
  },
})

export const { initMap, highlightMoves, toggle } = gameSlice.actions

export default gameSlice.reducer