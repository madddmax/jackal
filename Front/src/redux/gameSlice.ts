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
import { getAnotherRandomValue, getRandomValues } from '/app/global';
import { Constants } from '/app/constants';

export const gameSlice = createSlice({
    name: 'game',
    initialState: {
        cellSize: 50,
        pirateSize: 15,
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
            for (let i = 0; i < action.payload.height; i++) {
                let row: FieldState[] = [];
                for (let col = 0; col < action.payload.width; col++) {
                    if (!action.payload.changes[j].backgroundImageSrc) {
                        row.push({
                            backColor:
                                action.payload.changes[j].backgroundColor,
                        });
                    } else
                        row.push({
                            image: action.payload.changes[j].backgroundImageSrc,
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
            state.mapSize = action.payload.map.width;
            state.pirates = action.payload.pirates;
            state.teams = action.payload.stat.teams.map((it) => ({
                id: it.id,
                activePirate: '',
                lastPirate: '',
                hasPhotos: false,
            }));

            const width = window.innerWidth;
            const height = window.innerHeight - 56;
            const mSize = width > height ? height : width;
            if (mSize > 560) {
                state.cellSize = Math.floor(mSize / state.mapSize / 10) * 10;
                state.pirateSize = state.cellSize * 0.5;
            }
        },
        setTeam: (state, action: PayloadAction<number>) => {
            if (
                action.payload !== undefined &&
                action.payload !== state.currentTeamId
            ) {
                state.currentTeamId = action.payload;
            }
            let team = state.teams.find((it) => it.id == state.currentTeamId!)!;
            if (!team.hasPhotos) {
                let arr = getRandomValues(
                    Constants.PhotoMinId,
                    Constants.PhotoMaxId,
                    state.pirates?.filter((it) => it.teamId == team.id)
                        .length ?? 0,
                );
                state.pirates
                    ?.filter((it) => it.teamId == team.id)
                    .forEach((it, index) => {
                        it.photoId = arr[index];
                    });
                team.hasPhotos = true;
            }
        },
        choosePirate: (state, action: PayloadAction<PirateChoose>) => {
            let team = state.teams.find((it) => it.id == state.currentTeamId!)!;
            team.activePirate = action.payload.pirate;
            team.lastPirate = action.payload.pirate;

            if (action.payload.withCoin !== undefined) {
                state.pirates?.forEach((it) => {
                    if (it.id == team.activePirate) {
                        it.withCoin = action.payload.withCoin;
                    }
                });
            }
            gameSlice.caseReducers.highlightMoves(state, highlightMoves({}));
        },
        highlightMoves: (state, action: PayloadAction<PirateMoves>) => {
            console.log('highlightMoves');
            // undraw previous moves
            state.lastMoves.forEach((move) => {
                const cell = state.fields[move.to.y][move.to.x];
                cell.moveNum = undefined;
            });

            if (action.payload.moves) {
                state.lastMoves = action.payload.moves;
            }

            let team = state.teams.find((it) => it.id == state.currentTeamId!)!;
            let hasNoMoves =
                state.lastMoves.length > 0 &&
                !state.lastMoves.some((move) =>
                    move.from.pirateIds.includes(team.lastPirate),
                );
            team.activePirate = hasNoMoves
                ? state.lastMoves[0].from.pirateIds[0]
                : team.lastPirate;

            const pirate = state.pirates?.find(
                (it) => it.id == team.activePirate,
            );
            if (
                pirate?.position.x != state.highlight_x ||
                pirate?.position.y != state.highlight_y
            ) {
                const prevCell =
                    state.fields[state.highlight_y][state.highlight_x];
                prevCell.highlight = false;
                state.highlight_x = pirate?.position.x || 0;
                state.highlight_y = pirate?.position.y || 0;
                const curCell =
                    state.fields[state.highlight_y][state.highlight_x];
                curCell.highlight = true;
            }

            // собственно подсвечиваем ходы
            state.lastMoves
                .filter(
                    (move) =>
                        move.from.pirateIds.includes(team.activePirate) &&
                        ((pirate?.withCoin && move.withCoin) ||
                            pirate?.withCoin === undefined ||
                            (!pirate?.withCoin && !move.withCoin)),
                )
                .forEach((move) => {
                    const cell = state.fields[move.to.y][move.to.x];
                    cell.moveNum = move.moveNum;
                });
        },
        applyPirateChanges: (state, action: PayloadAction<PirateChanges>) => {
            action.payload.changes.forEach((it) => {
                if (it.isAlive === false) {
                    state.pirates = state.pirates?.filter(
                        (pr) => pr.id !== it.id,
                    );
                } else if (it.isAlive === true) {
                    let nm = getAnotherRandomValue(
                        Constants.PhotoMinId,
                        Constants.PhotoMaxId,
                        state.pirates
                            ?.filter((pr) => pr.teamId == it.teamId)
                            .map((pr) => pr.photoId ?? 0) ?? [],
                    );
                    state.pirates?.push({
                        id: it.id,
                        teamId: it.teamId,
                        position: it.position,
                        photoId: nm,
                    });
                } else {
                    let pirate = state.pirates!.find((pr) => pr.id === it.id)!;
                    pirate.position = it.position;
                }
            });

            if (action.payload.isHumanPlayer) {
                let girls = [] as string[];
                action.payload.moves
                    .filter((move) => move.withCoin)
                    .forEach((move) => {
                        girls.push(...move.from.pirateIds);
                    });
                let girlIds = new Set(girls);
                state.pirates?.forEach((it) => {
                    it.withCoin = girlIds.has(it.id)
                        ? (it.withCoin ?? true)
                        : undefined;
                });
            }
        },
        applyChanges: (state, action: PayloadAction<GameCell[]>) => {
            action.payload.forEach((it) => {
                const cell = state.fields[it.y][it.x];
                cell.image = it.backgroundImageSrc;
                cell.backColor = it.backgroundColor;
                cell.rotate = it.rotate;
                cell.levels = it.levels;
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
