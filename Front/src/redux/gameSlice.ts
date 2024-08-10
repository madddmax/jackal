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
                lastPirate: '',
                hasPhotos: false,
            }));

            const width = window.innerWidth;
            const height = window.innerHeight - 56;
            const mSize = width > height ? height : width;
            if (mSize > 560) {
                state.cellSize = Math.floor(mSize / state.mapSize / 10) * 10;
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
                    state.pirates?.filter((it) => it.TeamId == team.id)
                        .length ?? 0,
                );
                state.pirates
                    ?.filter((it) => it.TeamId == team.id)
                    .forEach((it, index) => {
                        it.PhotoId = arr[index];
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
                    if (it.Id == team.activePirate) {
                        it.WithCoin = action.payload.withCoin;
                    }
                });
            }
            gameSlice.caseReducers.highlightMoves(state, highlightMoves({}));
        },
        highlightMoves: (state, action: PayloadAction<PirateMoves>) => {
            console.log('highlightMoves');
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
            action.payload.changes.forEach((it) => {
                if (it.IsAlive === false) {
                    state.pirates = state.pirates?.filter(
                        (pr) => pr.Id !== it.Id,
                    );
                } else if (it.IsAlive === true) {
                    let nm = getAnotherRandomValue(
                        Constants.PhotoMinId,
                        Constants.PhotoMaxId,
                        state.pirates
                            ?.filter((pr) => pr.TeamId == it.TeamId)
                            .map((pr) => pr.PhotoId ?? 0) ?? [],
                    );
                    state.pirates?.push({
                        Id: it.Id,
                        TeamId: it.TeamId,
                        Position: it.Position,
                        PhotoId: nm,
                    });
                } else {
                    let pirate = state.pirates!.find((pr) => pr.Id === it.Id)!;
                    pirate.Position = it.Position;
                }
            });

            if (action.payload.isHumanPlayer) {
                let girls = [] as string[];
                action.payload.moves
                    .filter((move) => move.WithCoin)
                    .forEach((move) => {
                        girls.push(...move.From.PirateIds);
                    });
                let girlIds = new Set(girls);
                state.pirates?.forEach((it) => {
                    it.WithCoin = girlIds.has(it.Id)
                        ? (it.WithCoin ?? true)
                        : undefined;
                });
            }
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
