import { createSlice, current, PayloadAction } from '@reduxjs/toolkit';
import {
    CellPirate,
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
import { debugLog, getAnotherRandomValue, getRandomValues } from '/app/global';
import { Constants } from '/app/constants';

export const gameSlice = createSlice({
    name: 'game',
    initialState: {
        cellSize: 50,
        pirateSize: 15,
        fields: [[]],
        lastMoves: [],
        teams: [],
        currentHumanTeam: {
            id: -1,
            activePirate: '',
            lastPirate: '',
            isHumanPlayer: true,
            backColor: 'red',
            group: {
                id: 'girls',
                photoMaxId: 7,
            },
        },
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
                    const change = action.payload.changes[j];
                    if (!change.backgroundImageSrc) {
                        row.push({
                            backColor: change.backgroundColor,
                            levels: change.levels,
                        });
                    } else
                        row.push({
                            image: change.backgroundImageSrc,
                            levels: change.levels,
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
            let humIndex = 0;
            let botIndex = 0;
            state.teams = action.payload.stat.teams.map((it) => {
                let index = it.name.includes('Human') ? humIndex++ : botIndex++;
                return {
                    id: it.id,
                    activePirate: '',
                    lastPirate: '',
                    isHumanPlayer: it.name.includes('Human'),
                    backColor: it.backcolor,
                    group: it.name.includes('Human')
                        ? Constants.groups.find((gr) => gr.id == Constants.humanTeamIds[index]) ||
                          Constants.groups.find((gr) => gr.id == Constants.humanTeamIds[0]) ||
                          Constants.groups[0]
                        : Constants.groups.find((gr) => gr.id == Constants.robotTeamIds[index]) ||
                          Constants.groups.find((gr) => gr.id == Constants.robotTeamIds[0]) ||
                          Constants.groups[0],
                };
            });
            state.pirates = action.payload.pirates;
            debugLog('state.teams', state.teams);
            state.teams.forEach((team) => {
                let arr = getRandomValues(
                    1,
                    team.group.photoMaxId,
                    state.pirates?.filter((it) => it.teamId == team.id).length ?? 0,
                );
                state.pirates
                    ?.filter((it) => it.teamId == team.id)
                    .forEach((it, index) => {
                        (it.photo = `${team.group.id}/pirate_${arr[index]}${team.group.extension || '.png'}`),
                            (it.photoId = arr[index]);
                        it.groupId = team.group.id;
                    });
            });
            debugLog('state.pirates', state.pirates);

            const width = window.innerWidth;
            const height = window.innerHeight - 56;
            const mSize = width > height ? height : width;
            if (mSize > 560) {
                state.cellSize = Math.floor(mSize / state.mapSize / 10) * 10;
                state.pirateSize = state.cellSize * 0.5;
            }
        },
        setCurrentHumanTeam: (state, action: PayloadAction<number>) => {
            if (action.payload !== undefined && action.payload !== state.currentHumanTeam.id) {
                state.currentHumanTeam = state.teams.find((it) => it.id == action.payload)!;
                debugLog('setCurrentHumanTeam', current(state.currentHumanTeam));
            }
        },
        chooseHumanPirate: (state, action: PayloadAction<PirateChoose>) => {
            let pirate = state.pirates!.find((it) => it.id === action.payload.pirate)!;
            let withCoin =
                pirate.withCoin === undefined || state.currentHumanTeam.activePirate !== pirate.id
                    ? pirate.withCoin
                    : !pirate.withCoin;

            state.currentHumanTeam.activePirate = action.payload.pirate;
            state.currentHumanTeam.lastPirate = action.payload.pirate;
            state.teams
                .filter((it) => it.id === state.currentHumanTeam.id)
                .forEach((it) => {
                    it.activePirate = action.payload.pirate;
                    it.lastPirate = action.payload.pirate;
                });

            debugLog('setCurrentHumanTeam', current(state.currentHumanTeam));
            debugLog('withCoin', withCoin);

            if (withCoin !== undefined) {
                pirate.withCoin = withCoin;
                const level = state.fields[pirate.position.y][pirate.position.x].levels[pirate.position.level];
                const cellPirate = level.pirates?.find((it) => it.id == pirate.id);
                if (cellPirate) cellPirate.withCoin = withCoin;
            }
            gameSlice.caseReducers.highlightHumanMoves(state, highlightHumanMoves({}));
        },
        highlightHumanMoves: (state, action: PayloadAction<PirateMoves>) => {
            console.log('highlightMoves');
            // undraw previous moves
            state.lastMoves.forEach((move) => {
                const cell = state.fields[move.to.y][move.to.x];
                cell.availableMove = undefined;
            });

            if (action.payload.moves) {
                state.lastMoves = action.payload.moves;
            }

            let hasNoMoves =
                state.lastMoves.length > 0 &&
                !state.lastMoves.some((move) => move.from.pirateIds.includes(state.currentHumanTeam.lastPirate));
            debugLog('hasNoMoves', state.currentHumanTeam.lastPirate, hasNoMoves);
            state.currentHumanTeam.activePirate = hasNoMoves
                ? state.lastMoves[0].from.pirateIds[0]
                : state.currentHumanTeam.lastPirate;

            const pirate = state.pirates?.find((it) => it.id == state.currentHumanTeam.activePirate);
            if (pirate?.position.x != state.highlight_x || pirate?.position.y != state.highlight_y) {
                const prevCell = state.fields[state.highlight_y][state.highlight_x];
                prevCell.highlight = false;
                state.highlight_x = pirate?.position.x || 0;
                state.highlight_y = pirate?.position.y || 0;
                const curCell = state.fields[state.highlight_y][state.highlight_x];
                curCell.highlight = true;
            }

            if (!pirate) return;

            // собственно подсвечиваем ходы
            state.lastMoves
                .filter(
                    (move) =>
                        move.from.pirateIds.includes(state.currentHumanTeam.activePirate) &&
                        ((pirate?.withCoin && move.withCoin) ||
                            pirate?.withCoin === undefined ||
                            (!pirate?.withCoin && !move.withCoin)),
                )
                .forEach((move) => {
                    const cell = state.fields[move.to.y][move.to.x];
                    cell.availableMove = {
                        num: move.moveNum,
                        pirate: pirate.id,
                    };
                });
        },
        applyPirateChanges: (state, action: PayloadAction<PirateChanges>) => {
            action.payload.changes.forEach((it) => {
                let team = state.teams.find((tm) => tm.id == it.teamId)!;
                if (it.isAlive === false) {
                    let pirate = state.pirates!.find((pr) => pr.id === it.id)!;
                    debugLog('dead pirate', current(pirate));
                    const prevCell = state.fields[pirate.position.y][pirate.position.x];
                    const prevLevel = prevCell.levels[pirate.position.level];
                    debugLog(
                        'prevLevel.pirates',
                        current(pirate).position.x,
                        current(pirate).position.y,
                        current(prevLevel).pirates,
                    );
                    if (prevLevel.pirates != undefined) {
                        let prevLevelPirate = prevLevel.pirates.find((pr) => pr.id === it.id);
                        if (prevLevelPirate) {
                            prevLevelPirate.photo = prevCell.image?.includes('arrow') ? 'skull.png' : 'skull_light.png';
                            prevLevelPirate.isTransparent = true;
                        }
                    }
                    state.pirates = state.pirates?.filter((pr) => pr.id !== it.id);
                } else if (it.isAlive === true) {
                    let nm = getAnotherRandomValue(
                        1,
                        team.group.photoMaxId,
                        state.pirates?.filter((pr) => pr.teamId == it.teamId).map((pr) => pr.photoId ?? 0) ?? [],
                    );
                    state.pirates?.push({
                        id: it.id,
                        teamId: it.teamId,
                        position: it.position,
                        groupId: team.group.id,
                        photo: `${team.group.id}/pirate_${nm}${team.group.extension || '.png'}`,
                        photoId: nm,
                    });
                    const level = state.fields[it.position.y][it.position.x].levels[it.position.level];
                    const drawPirate: CellPirate = {
                        id: it.id,
                        photo: `${team.group.id}/pirate_${nm}${team.group.extension || '.png'}`,
                        photoId: nm,
                        backgroundColor: team.backColor,
                    };
                    if (level.pirates == undefined) level.pirates = [drawPirate];
                    else level.pirates.push(drawPirate);
                } else {
                    let pirate = state.pirates!.find((pr) => pr.id === it.id)!;
                    debugLog('moved pirate', current(pirate));

                    const prevLevel = state.fields[pirate.position.y][pirate.position.x].levels[pirate.position.level];
                    debugLog(
                        'prevLevel.pirates',
                        current(pirate).position.x,
                        current(pirate).position.y,
                        current(prevLevel).pirates,
                    );
                    if (prevLevel.pirates != undefined) {
                        prevLevel.pirates = prevLevel.pirates.filter((it) => it.id != pirate.id);
                        if (prevLevel.pirates.length == 0) prevLevel.pirates = undefined;
                    }
                    pirate.position = it.position;

                    const level = state.fields[pirate.position.y][pirate.position.x].levels[pirate.position.level];
                    debugLog('drawPirate', current(pirate).position.x, current(pirate).position.y, current(pirate).id);
                    const drawPirate: CellPirate = {
                        id: pirate.id,
                        photo: pirate.photo,
                        photoId: pirate.photoId,
                        withCoin: pirate.withCoin,
                        backgroundColor: team.backColor,
                    };
                    if (level.pirates == undefined) level.pirates = [drawPirate];
                    else level.pirates.push(drawPirate);
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
                    let changeCoin = girlIds.has(it.id)
                        ? true //(it.withCoin ?? true)
                        : undefined;
                    if (changeCoin != it.withCoin) {
                        it.withCoin = changeCoin;
                        const level = state.fields[it.position.y][it.position.x].levels[it.position.level];
                        const pr = level.pirates?.find((pr) => pr.id == it.id)!;
                        pr.withCoin = changeCoin;
                    }
                });
            }
        },
        applyChanges: (state, action: PayloadAction<GameCell[]>) => {
            action.payload.forEach((it) => {
                const cell = state.fields[it.y][it.x];
                if (cell.image != it.backgroundImageSrc) {
                    cell.image = it.backgroundImageSrc;
                    cell.backColor = it.backgroundColor;
                    cell.rotate = it.rotate;
                }
                cell.levels = it.levels.map((lev) => ({
                    ...lev,
                    pirate: undefined,
                    pirates: cell?.levels && cell?.levels[lev.level]?.pirates,
                }));
            });
        },
        applyStat: (state, action: PayloadAction<GameStat>) => {
            state.stat = action.payload;
        },
    },
});

export const {
    initMap,
    setCurrentHumanTeam,
    chooseHumanPirate,
    highlightHumanMoves,
    applyPirateChanges,
    applyChanges,
    initGame,
    applyStat,
} = gameSlice.actions;

export default gameSlice.reducer;
