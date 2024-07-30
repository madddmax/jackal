import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import {
    FieldState,
    GameCell,
    GameMainStat,
    GameMap,
    GameStat,
    GameState,
    PirateMoves,
} from './types';

export const gameSlice = createSlice({
    name: 'game',
    initialState: {
        fields: [[]],
        lastMoves: [],
        activePirate: '',
        highlight_x: 0,
        highlight_y: 0,
        lastPirate: '',
    } satisfies GameState as GameState,
    reducers: {
        initMap: (state, action: PayloadAction<GameMap>) => {
            let map = [];
            let j = 0;
            for (let i = 0; i < action.payload.Height; i++) {
                let row: FieldState[] = [];
                for (let col = 0; col < action.payload.Width; col++) {
                    if (!action.payload.Changes[j].BackgroundImageSrc) {
                        row.push({
                            backColor:
                                action.payload.Changes[j].BackgroundColor,
                        });
                    } else
                        row.push({
                            image: action.payload.Changes[j].BackgroundImageSrc,
                        });
                    j++;
                }
                map.push(row);
            }
            state.fields = map;
            state.pirates = undefined;
        },
        highlightMoves: (state, action: PayloadAction<PirateMoves>) => {
            // undraw previous moves
            state.lastMoves.forEach((move) => {
                const cell = state.fields[move.To.Y][move.To.X];
                cell.moveNum = undefined;
            });

            if (action.payload.moves) {
                state.lastMoves = action.payload.moves;
            }

            if (
                action.payload.pirate === undefined &&
                action.payload.withCoin === undefined
            ) {
                state.pirates = action.payload.pirates;
            }

            state.activePirate = state.lastPirate;
            if (action.payload.pirate) {
                state.activePirate = action.payload.pirate;
                state.lastPirate = action.payload.pirate;
            }
            if (
                state.lastMoves.length > 0 &&
                !state.lastMoves.some((move) =>
                    move.From.PirateIds.includes(state.activePirate),
                )
            ) {
                state.activePirate = state.lastMoves[0].From.PirateIds[0];
            }

            const prevCell = state.fields[state.highlight_y][state.highlight_x];
            prevCell.highlight = false;

            const pirate = state.pirates?.find(
                (it) => it.Id == state.activePirate,
            );
            state.highlight_x = pirate?.Position.X || 0;
            state.highlight_y = pirate?.Position.Y || 0;
            const curCell = state.fields[state.highlight_y][state.highlight_x];
            curCell.highlight = true;

            if (action.payload.withCoin !== undefined) {
                state.withCoin = action.payload.withCoin;
            } else {
                state.withCoin =
                    state.lastMoves
                        .filter((move) =>
                            move.From.PirateIds.includes(state.activePirate),
                        )
                        .some((move) => move.WithCoin) || undefined;
            }

            state.lastMoves
                .filter(
                    (move) =>
                        move.From.PirateIds.includes(state.activePirate) &&
                        ((state.withCoin && move.WithCoin) ||
                            state.withCoin === undefined ||
                            (!state.withCoin && !move.WithCoin)),
                )
                .forEach((move) => {
                    const cell = state.fields[move.To.Y][move.To.X];
                    cell.moveNum = move.MoveNum;
                });
        },
        applyChanges: (state, action: PayloadAction<GameCell[]>) => {
            action.payload.forEach((it) => {
                const cell = state.fields[it.Y][it.X];
                cell.image = it.BackgroundImageSrc;
                cell.backColor = it.BackgroundColor;
                cell.rotate = it.Rotate;
                cell.levels = it.Levels;
            });
        },
        applyMainStat: (state, action: PayloadAction<GameMainStat>) => {
            state.gameName = action.payload.gameName;
            state.mapId = action.payload.mapId;
        },
        applyStat: (state, action: PayloadAction<GameStat>) => {
            state.stat = action.payload;
        },
    },
});

export const {
    initMap,
    highlightMoves,
    applyChanges,
    applyMainStat,
    applyStat,
} = gameSlice.actions;

export default gameSlice.reducer;
