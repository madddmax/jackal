import { createSlice } from '@reduxjs/toolkit'
import { FieldState, GameCell, GameMap, GameState, GameMove } from './types';

export const gameSlice = createSlice({
  name: 'game',
  initialState: {
    fields: [[]],
    lastMoves: []
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
          else row.push({ image: gMap.Changes[j].BackgroundImageSrc });
          j++;
        }
        map.push(row);
      }
      state.fields = map;
    },
    highlightMoves: (state, action) => {

      // undraw previous moves
      state.lastMoves.forEach(move => {
        const cell = state.fields[move.To.Y][move.To.X];
        cell.moveNum = undefined;
      });

      state.lastMoves = action.payload as GameMove[];
      state.lastMoves.forEach(move => {
        const cell = state.fields[move.To.Y][move.To.X];
        cell.moveNum = move.MoveNum;
      });
    },
    applyChanges: (state, action) => {
      let changes = action.payload as GameCell[];
      changes.forEach(it => {
        const cell = state.fields[it.Y][it.X];
        cell.image = it.BackgroundImageSrc;
        cell.backColor = it.BackgroundColor;
        cell.rotate = it.Rotate;
      });
    },
    toggle: (state, action) => {
        const { row, col } = action.payload;
        const val = state.fields[row][col];
        state.fields[row][col] = { image: val.image };
    }
  },
})

export const { initMap, highlightMoves, applyChanges, toggle } = gameSlice.actions

export default gameSlice.reducer