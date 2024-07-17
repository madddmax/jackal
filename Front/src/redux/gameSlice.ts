import { createSlice, PayloadAction } from '@reduxjs/toolkit'
import { FieldState, GameCell, GameMap, GameState, PirateMoves } from './types';

export const gameSlice = createSlice({
  name: 'game',
  initialState: {
    fields: [[]],
    lastMoves: [],
    activePirate: 1,
    lastPirate: 1
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
    highlightMoves: (state, action: PayloadAction<PirateMoves>) => {

      // undraw previous moves
      state.lastMoves.forEach(move => {
        const cell = state.fields[move.To.Y][move.To.X];
        cell.moveNum = undefined;
      });

      if (action.payload.moves) {
        state.lastMoves = action.payload.moves;
      }

      state.activePirate = state.lastPirate;
      if (action.payload.pirate) {
        state.activePirate = action.payload.pirate;
        state.lastPirate = action.payload.pirate;
      } 
      if (state.lastMoves.length > 0 && !state.lastMoves.some(move => move.From.PirateNum == state.activePirate)) {
        state.activePirate = state.lastMoves[0].From.PirateNum;
      }

      if (action.payload.withCoin !== undefined) {
        state.withCoin = action.payload.withCoin;
      } else {
        state.withCoin = state.lastMoves.filter(move => move.From.PirateNum == state.activePirate).some(move => move.WithCoin) || undefined;
      }

      state.lastMoves.filter(move => move.From.PirateNum == state.activePirate &&
        ((state.withCoin && move.WithCoin) || state.withCoin === undefined || (!state.withCoin && !move.WithCoin))).forEach(move => {
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
        cell.levels = it.Levels;
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