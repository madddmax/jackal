import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import {
    FieldState,
    GameCell,
    GameMap,
    GameStartResponse,
    GameStat,
    GameState,
    PirateChanges,
    PirateChoose,
    PirateMoves,
} from './types';

export const gameSlice = createSlice({
    name: 'game',
    initialState: {
        fields: [[]],
        lastMoves: [],
        teams: [],
        highlight_x: 0,
        highlight_y: 0,
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
        },
        initGame: (state, action: PayloadAction<GameStartResponse>) => {
            state.gameName = action.payload.gameName;
            state.mapId = action.payload.mapId;
            state.mapSize = action.payload.map.Width;
            state.pirates = action.payload.pirates;
            state.teams = action.payload.stat.Teams.map((it) => ({
                id: it.id,
                activePirate: '',
            }));
        },
        setTeam: (state, action: PayloadAction<number>) => {
            if (
                action.payload !== undefined &&
                action.payload !== state.currentTeamId
            ) {
                state.currentTeamId = action.payload;
            }
        },
        choosePirate: (state, action: PayloadAction<PirateChoose>) => {
            let team = state.teams.find((it) => it.id == state.currentTeamId!)!;
            team.activePirate = action.payload.pirate;
            team.lastPirate = action.payload.pirate;

            if (action.payload.withCoin !== undefined) {
                state.pirates?.forEach((it) => {
                    if (it.Id == team.activePirate) {
                        it.WithCoin = action.payload.withCoin;
                    }
                });
            }
            gameSlice.caseReducers.highlightMoves(state, highlightMoves({}));
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

            let team = state.teams.find((it) => it.id == state.currentTeamId!)!;
            let hasNoMoves =
                state.lastMoves.length > 0 &&
                !state.lastMoves.some((move) =>
                    move.From.PirateIds.includes(team.lastPirate),
                );
            team.activePirate = hasNoMoves
                ? state.lastMoves[0].From.PirateIds[0]
                : team.lastPirate;

            const pirate = state.pirates?.find(
                (it) => it.Id == team.activePirate,
            );
            if (
                pirate?.Position.X != state.highlight_x ||
                pirate?.Position.Y != state.highlight_y
            ) {
                const prevCell =
                    state.fields[state.highlight_y][state.highlight_x];
                prevCell.highlight = false;
                state.highlight_x = pirate?.Position.X || 0;
                state.highlight_y = pirate?.Position.Y || 0;
                const curCell =
                    state.fields[state.highlight_y][state.highlight_x];
                curCell.highlight = true;
            }

            // собственно подсвечиваем ходы
            state.lastMoves
                .filter(
                    (move) =>
                        move.From.PirateIds.includes(team.activePirate) &&
                        ((pirate?.WithCoin && move.WithCoin) ||
                            pirate?.WithCoin === undefined ||
                            (!pirate?.WithCoin && !move.WithCoin)),
                )
                .forEach((move) => {
                    const cell = state.fields[move.To.Y][move.To.X];
                    cell.moveNum = move.MoveNum;
                });
        },
        applyPirateChanges: (state, action: PayloadAction<PirateChanges>) => {
            let girls = [] as string[];
            action.payload.moves
                .filter((move) => move.WithCoin)
                .forEach((move) => {
                    girls.push(...move.From.PirateIds);
                });
            let girlIds = new Set(girls);
            state.pirates?.forEach((it) => {
                let pos = action.payload.pirates?.find(
                    (pt) => pt.Id === it.Id,
                )?.Position;
                if (pos) it.Position = pos;
                if (!girlIds.has(it.Id)) it.WithCoin = undefined;
                else it.WithCoin = it.WithCoin ?? true;
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
        applyStat: (state, action: PayloadAction<GameStat>) => {
            state.stat = action.payload;
        },
    },
});

export const {
    initMap,
    setTeam,
    choosePirate,
    highlightMoves,
    applyPirateChanges,
    applyChanges,
    initGame,
    applyStat,
} = gameSlice.actions;

export default gameSlice.reducer;
