import { PayloadAction, createSlice, current } from '@reduxjs/toolkit';
import { memoize } from 'proxy-memoize';

import { girlsMap } from '../logic/gameLogic';
import {
    ChooseHumanPirateActionProps,
    FieldState,
    GamePlace,
    GameState,
    GameStateSettings,
    HighlightHumanMovesActionProps,
    StorageState,
} from '../types';
import { GameLevel, GameLevelFeature } from '../types/gameContent';
import {
    CellDiffResponse,
    GameMapResponse,
    GamePirateChangesResponse,
    GameStartResponse,
    GameStatisticsResponse,
    GameTeamResponse,
} from '../types/gameSaga';
import { ScreenSizes, TeamScores } from './gameSlice.types';
import { constructGameLevel } from './utils';
import { Constants } from '/app/constants';
import { debugLog, getAnotherRandomValue } from '/app/global';

export const gameSlice = createSlice({
    name: 'game',
    initialState: {
        fields: [[]],
        lastMoves: [],
        gameSettings: {
            cellSize: 50,
            pirateSize: 15,
            tilesPackNames: [],
        },
        userSettings: {
            groups: [
                Constants.groupIds.girls,
                Constants.groupIds.redalert,
                Constants.groupIds.orcs,
                Constants.groupIds.skulls,
            ],
            mapSize: 11,
            players: ['human', 'robot2', 'robot', 'robot2'],
            playersMode: 4,
            gameSpeed: 1,
        },
        teams: [],
        currentHumanTeamId: 0,
        highlight_x: 0,
        highlight_y: 0,
        hasPirateAutoChange: true,
        includeMovesWithRum: false,
    } satisfies GameState as GameState,
    reducers: {
        initMySettings: (state, action: PayloadAction<StorageState>) => {
            Object.assign(state.userSettings, action.payload);
        },
        saveMySettings: (state, action: PayloadAction<StorageState>) => {
            localStorage.state = JSON.stringify(action.payload, null, 2);
            Object.assign(state.userSettings, action.payload);
        },
        initGame: (state, action: PayloadAction<GameStartResponse>) => {
            state.gameSettings.gameId = action.payload.gameId;
            state.gameSettings.gameMode = action.payload.gameMode;
            state.gameSettings.tilesPackName = action.payload.tilesPackName;
            state.gameSettings.mapId = action.payload.mapId;
            state.pirates = action.payload.pirates;
            state.lastMoves = [];
            state.highlight_x = 0;
            state.highlight_y = 0;

            gameSlice.caseReducers.initMap(state, initMap(action.payload.map));
            gameSlice.caseReducers.initTeams(state, initTeams(action.payload.teams));
            gameSlice.caseReducers.initPhotos(state);
            gameSlice.caseReducers.initSizes(
                state,
                initSizes({ width: window.innerWidth, height: window.innerHeight }),
            );
            gameSlice.caseReducers.initPiratePositions(state);
        },
        initMap: (state, action: PayloadAction<GameMapResponse>) => {
            const map = [];
            let j = 0;
            for (let i = 0; i < action.payload.height; i++) {
                const row: FieldState[] = [];
                for (let col = 0; col < action.payload.width; col++) {
                    const change = action.payload.changes[j];
                    row.push({
                        image: change.backgroundImageSrc,
                        rotate: change.rotate,
                        levels: change.levels.map(constructGameLevel),
                        availableMoves: [],
                    });
                    j++;
                }
                map.push(row);
            }
            state.gameSettings.mapSize = action.payload.width;
            state.fields = map;
        },
        initTeams: (state, action: PayloadAction<GameTeamResponse[]>) => {
            state.teams = action.payload.map((it, idx, arr) => {
                const grId = arr.length == 2 && idx == 1 ? 2 : idx;
                return {
                    id: it.id,
                    isCurrentUser: it.isCurrentUser,
                    activePirate: '',
                    backColor: Constants.teamColors[idx] ?? '',
                    name: it.name,
                    isHuman: it.isHuman,
                    group:
                        Constants.groups.find((gr) => gr.id == state.userSettings.groups[grId]) || Constants.groups[0],
                };
            });
        },
        initPhotos: (state) => {
            state.teams.forEach((team) => {
                state.pirates
                    ?.filter((it) => it.teamId == team.id)
                    .forEach((it) => {
                        let pname;
                        let pnumber;
                        let extension = '.png';

                        if (it.type == Constants.pirateTypes.BenGunn) {
                            pname = 'commonganns/gann';
                            pnumber = getAnotherRandomValue(
                                1,
                                Constants.commonGannMaxId,
                                state.pirates
                                    ?.filter((pr) => pr.type == Constants.pirateTypes.BenGunn)
                                    .map((pr) => pr.photoId ?? 0) ?? [],
                            );
                        } else if (it.type == Constants.pirateTypes.Friday) {
                            pname = 'commonfridays/friday';
                            pnumber = getAnotherRandomValue(1, Constants.commonFridayMaxId, []);
                        } else {
                            pname = `${team.group.id}/pirate`;
                            pnumber = getAnotherRandomValue(
                                1,
                                team.group.photoMaxId,
                                state.pirates?.filter((pr) => pr.teamId == it.teamId).map((pr) => pr.photoId ?? 0) ??
                                    [],
                            );
                            extension = team.group.extension || '.png';
                        }

                        it.photo = `${pname}_${pnumber}${extension}`;
                        it.photoId = pnumber;
                        it.groupId = team.group.id;
                        it.backgroundColor = team.backColor;
                    });
            });
        },
        initSizes: (state, action: PayloadAction<ScreenSizes>) => {
            const width = action.payload.width;
            const height = action.payload.height - 56;
            const mSize = width > height ? height : width;

            if (mSize > 560) {
                state.gameSettings.cellSize = Math.floor(mSize / state.gameSettings.mapSize! / 10) * 10;
            }
            state.gameSettings.pirateSize = state.gameSettings.cellSize * 0.55;
        },
        initPiratePositions: (state) => {
            girlsMap.Map = {};
            state.pirates!.forEach((it: GamePirate) => {
                girlsMap.AddPosition(it, 1);
            });
        },
        setCurrentHumanTeam: (state) => {
            const currentTeam = gameSlice.getSelectors().getCurrentTeam(state);
            if (currentTeam?.isCurrentUser && currentTeam.id !== state.currentHumanTeamId) {
                state.currentHumanTeamId = currentTeam.id;
            }
        },
        setPirateAutoChange: (state, action: PayloadAction<boolean>) => {
            state.hasPirateAutoChange = action.payload;
        },
        setIncludeMovesWithRum: (state, action: PayloadAction<boolean>) => {
            state.includeMovesWithRum = action.payload;
            gameSlice.caseReducers.highlightHumanMoves(state, highlightHumanMoves({}));
        },
        chooseHumanPirate: (state, action: PayloadAction<ChooseHumanPirateActionProps>) => {
            const selectors = gameSlice.getSelectors();
            const pirate = selectors.getPirateById(state, action.payload.pirate)!;
            const currentPlayerTeam = selectors.getCurrentPlayerTeam(state)!;
            const hasPirateChanging = currentPlayerTeam.activePirate !== pirate.id;
            if (hasPirateChanging) {
                const prevPirate = selectors.getPirateById(state, currentPlayerTeam.activePirate);
                if (prevPirate) prevPirate.isActive = false;
                const nextPirate = selectors.getPirateById(state, pirate.id);
                if (nextPirate) nextPirate.isActive = true;

                currentPlayerTeam.activePirate = pirate.id;
                gameSlice.caseReducers.highlightHumanMoves(state, highlightHumanMoves({}));
                return;
            }

            const hasCoinChanging =
                action.payload.withCoinAction && (pirate.withCoin !== undefined || pirate.withBigCoin !== undefined);
            if (hasCoinChanging) {
                const level = state.fields[pirate.position.y][pirate.position.x].levels[pirate.position.level];
                if (pirate.withBigCoin) {
                    pirate.withBigCoin = false;
                    if (level.pirates.coins < level.info.coins) {
                        pirate.withCoin = true;
                    }
                } else if (pirate.withCoin) {
                    pirate.withCoin = false;
                } else if (level.pirates.bigCoins < level.info.bigCoins) {
                    pirate.withBigCoin = true;
                } else if (level.pirates.coins < level.info.coins) {
                    pirate.withCoin = true;
                }
                gameSlice.caseReducers.updateLevelCoinsData(state, updateLevelCoinsData(pirate));
                gameSlice.caseReducers.highlightHumanMoves(state, highlightHumanMoves({}));
            }
        },
        highlightHumanMoves: (state, action: PayloadAction<HighlightHumanMovesActionProps>) => {
            const selectors = gameSlice.getSelectors();
            const currentTeam = selectors.getCurrentTeam(state)!;
            if (!currentTeam?.isCurrentUser) return;

            // undraw previous moves
            state.lastMoves.forEach((move) => {
                const cell = state.fields[move.to.y][move.to.x];
                cell.availableMoves = [];
            });

            if (action.payload.moves) {
                state.lastMoves = action.payload.moves;
            }

            if (
                state.hasPirateAutoChange &&
                state.lastMoves.length > 0 &&
                !state.lastMoves.some((move) => move.from.pirateIds.includes(currentTeam.activePirate))
            ) {
                const prevPirate = selectors.getPirateById(state, currentTeam.activePirate);
                if (prevPirate) prevPirate.isActive = false;

                currentTeam.activePirate = state.lastMoves[0].from.pirateIds[0];
            }

            const pirate = selectors.getPirateById(state, currentTeam.activePirate);
            if (!pirate) return;

            pirate.isActive = true;
            gameSlice.caseReducers.highlightPirate(state, highlightPirate(pirate));

            // собственно подсвечиваем ходы
            state.lastMoves
                .filter(
                    (move) =>
                        move.from.pirateIds.includes(currentTeam.activePirate) &&
                        (!move.withRumBottle || (move.withRumBottle && state.includeMovesWithRum)) &&
                        ((pirate?.withCoin && move.withCoin) ||
                            (pirate?.withBigCoin && move.withBigCoin) ||
                            (pirate?.withCoin === undefined && pirate?.withBigCoin === undefined) ||
                            (!pirate?.withCoin && !pirate?.withBigCoin && !move.withCoin && !move.withBigCoin)),
                )
                .forEach((move) => {
                    const cell = state.fields[move.to.y][move.to.x];
                    cell.availableMoves.push({
                        num: move.moveNum,
                        isRespawn: move.withRespawn,
                        pirateId: pirate.id,
                        prev: move.prev,
                    });
                });
        },
        removeHumanMoves: (state) => {
            // undraw previous moves
            state.lastMoves.forEach((move) => {
                const cell = state.fields[move.to.y][move.to.x];
                cell.availableMoves = [];
            });
        },
        highlightPirate: (state, action: PayloadAction<GamePirate>) => {
            const pirate = action.payload;
            if (!pirate) return;

            if (pirate?.position.x != state.highlight_x || pirate?.position.y != state.highlight_y) {
                const prevCell = state.fields[state.highlight_y][state.highlight_x];
                prevCell.highlight = false;
            }

            state.highlight_x = pirate?.position.x || 0;
            state.highlight_y = pirate?.position.y || 0;
            const curCell = state.fields[state.highlight_y][state.highlight_x];
            curCell.highlight = true;
        },
        applyPirateChanges: (state, action: PayloadAction<GamePirateChangesResponse>) => {
            const cached = {} as { [id: number]: GameLevel };
            const selectors = gameSlice.getSelectors();

            action.payload.changes.forEach((it) => {
                const team = state.teams.find((tm) => tm.id == it.teamId)!;
                if (it.isAlive === false) {
                    const place = selectors.getPirateCell(state, it.id);
                    if (place) {
                        const skull: GameLevelFeature = {
                            photo: place.cell.image?.includes('arrow') ? 'skull.png' : 'skull_light.png',
                            backgroundColor: 'transparent',
                        };
                        if (place.level.features === undefined) place.level.features = [skull];
                        else place.level.features.push(skull);
                    }
                    const pirate = state.pirates!.find((pr) => pr.id === it.id)!;
                    state.pirates = state.pirates?.filter((pr) => pr.id !== it.id);

                    debugLog('dead pirate', current(pirate));
                    girlsMap.RemovePosition(pirate);
                    gameSlice.caseReducers.updateLevelCoinsData(state, updateLevelCoinsData(pirate));
                } else if (it.isAlive === true) {
                    let pname;
                    let pnumber;
                    let extension = '.png';

                    if (it.type == Constants.pirateTypes.BenGunn) {
                        pname = 'commonganns/gann';
                        pnumber = getAnotherRandomValue(
                            1,
                            Constants.commonGannMaxId,
                            state.pirates
                                ?.filter((pr) => pr.type == Constants.pirateTypes.BenGunn)
                                .map((pr) => pr.photoId ?? 0) ?? [],
                        );
                    } else if (it.type == Constants.pirateTypes.Friday) {
                        pname = 'commonfridays/friday';
                        pnumber = getAnotherRandomValue(1, Constants.commonFridayMaxId, []);
                    } else {
                        pname = `${team.group.id}/pirate`;
                        pnumber = getAnotherRandomValue(
                            1,
                            team.group.photoMaxId,
                            state.pirates?.filter((pr) => pr.teamId == it.teamId).map((pr) => pr.photoId ?? 0) ?? [],
                        );
                        extension = team.group.extension || '.png';
                    }

                    state.pirates?.push({
                        id: it.id,
                        teamId: it.teamId,
                        position: it.position,
                        groupId: team.group.id,
                        photo: `${pname}_${pnumber}${extension}`,
                        photoId: pnumber,
                        type: it.type,
                        isActive: it.id === team.activePirate,
                        backgroundColor: team.backColor,
                    });
                    girlsMap.AddPosition(it, state.fields[it.position.y][it.position.x].levels.length);
                } else {
                    const pirate = state.pirates!.find((pr) => pr.id === it.id)!;

                    const cachedId = pirate.position.y * 1000 + pirate.position.x * 10 + pirate.position.level;
                    if (!Object.prototype.hasOwnProperty.call(cached, cachedId)) {
                        cached[cachedId] =
                            state.fields[pirate.position.y][pirate.position.x].levels[pirate.position.level];
                    }

                    girlsMap.RemovePosition(pirate);
                    gameSlice.caseReducers.updateLevelCoinsData(state, updateLevelCoinsData(pirate));

                    pirate.position = it.position;
                    pirate.isDrunk = it.isDrunk;
                    pirate.isInTrap = it.isInTrap;
                    pirate.isInHole = it.isInHole;
                    pirate.isActive = it.id === team.activePirate;

                    girlsMap.AddPosition(pirate, state.fields[it.position.y][it.position.x].levels.length);
                    gameSlice.caseReducers.updateLevelCoinsData(state, updateLevelCoinsData(pirate));

                    debugLog('move pirate', current(pirate).position.x, current(pirate).position.y, current(pirate).id);
                    if (it.id === team.activePirate) {
                        gameSlice.caseReducers.highlightPirate(state, highlightPirate(pirate));
                    }
                }
            });

            debugLog(current(state.teams));
            // автоподнятие монет
            const currentTeam = selectors.getCurrentTeam(state)!;
            if (currentTeam.isCurrentUser) {
                const girlIds = new Set();
                action.payload.moves
                    .filter((move) => move.withCoin || move.withBigCoin)
                    .forEach((move) => {
                        move.from.pirateIds.forEach((it) => girlIds.add(it));
                    });
                state.pirates?.forEach((it) => {
                    const changeCoin = girlIds.has(it.id) ? true : undefined;
                    if (changeCoin != it.withCoin || changeCoin != it.withBigCoin) {
                        const cachedId = it.position.y * 1000 + it.position.x * 10 + it.position.level;
                        if (!Object.prototype.hasOwnProperty.call(cached, cachedId)) {
                            cached[cachedId] = state.fields[it.position.y][it.position.x].levels[it.position.level];
                        }

                        const cell = girlsMap.GetPosition(it);
                        const level = cached[cachedId];
                        const levelPirates = state.pirates?.filter((it) => cell?.girls?.includes(it.id));

                        let changeSmallCoin = changeCoin;
                        let changeBigCoin = changeCoin;

                        if (changeCoin !== undefined) {
                            changeBigCoin =
                                levelPirates!.filter((pr) => pr.id != it.id && pr.withBigCoin).length <
                                level.info.bigCoins;
                            changeSmallCoin = changeBigCoin
                                ? false
                                : levelPirates!.filter((pr) => pr.id != it.id && pr.withCoin).length < level.info.coins;
                        }
                        it.withCoin = changeSmallCoin;
                        it.withBigCoin = changeBigCoin;
                        const prt = levelPirates?.find((pr) => pr.id == it.id);
                        if (prt) {
                            prt.withCoin = changeSmallCoin;
                            prt.withBigCoin = changeBigCoin;
                        }

                        gameSlice.caseReducers.updateLevelCoinsData(state, updateLevelCoinsData(it));
                    }
                });
            }
        },
        applyChanges: (state, action: PayloadAction<CellDiffResponse[]>) => {
            action.payload.forEach((it) => {
                const cell = state.fields[it.y][it.x];
                if (cell.image != it.backgroundImageSrc) {
                    cell.image = it.backgroundImageSrc;
                    cell.rotate = it.rotate;
                }
                if (state.stat?.isGameOver) {
                    cell.dark = true;
                }
                if (cell.levels.length !== it.levels.length) {
                    // открыли новую клетку или разлом
                    cell.levels = it.levels.map(constructGameLevel);
                } else {
                    cell.levels.forEach((lev) => {
                        lev.info = it.levels[lev.info.level];
                    });
                }
            });
        },
        updateLevelCoinsData: (state, action: PayloadAction<GamePiratePosition>) => {
            const field = state.fields[action.payload.position.y][action.payload.position.x];
            const level = field.levels[action.payload.position.level];
            const girlsLevel = girlsMap.GetPosition(action.payload);
            const levelPirates = state.pirates?.filter((it) => girlsLevel?.girls?.includes(it.id));
            level.pirates = {
                coins: levelPirates?.filter((it) => it.withCoin).length ?? 0,
                bigCoins: levelPirates?.filter((it) => it.withBigCoin).length ?? 0,
            };
            level.freeCoinGirlId = !field.image?.includes('ship')
                ? levelPirates?.find(
                      (it) =>
                          (!it.withBigCoin && level.pirates.bigCoins < level.info.bigCoins) ||
                          (!it.withCoin && level.pirates.coins < level.info.coins),
                  )?.id
                : undefined;
        },
        applyStat: (state, action: PayloadAction<GameStatisticsResponse>) => {
            state.stat = action.payload.stats;
            state.teamScores = action.payload.teamScores;

            // вызываем после присваивания stat, т.к. именно от туда приходит stat.currentTeamId
            gameSlice.caseReducers.setCurrentHumanTeam(state);
        },
        setTilesPackNames: (state, action: PayloadAction<string[]>) => {
            state.gameSettings.tilesPackNames = action.payload;
        },
        setMapForecasts: (state, action: PayloadAction<string[] | undefined>) => {
            state.mapForecasts = action.payload;
        },
    },
    selectors: {
        getTeamById: (state, teamId: number): TeamState | undefined => state.teams.find((it) => it.id == teamId),
        getCurrentPlayerTeam: (state): TeamState | undefined =>
            state.teams.find((it) => it.id == state.currentHumanTeamId),
        getCurrentPlayerPirates: (state): GamePirate[] | undefined => {
            const currentPlayerTeam = gameSlice.getSelectors().getCurrentPlayerTeam(state);
            return state.pirates?.filter((it) => it.teamId == currentPlayerTeam?.id);
        },
        getCurrentTeam: (state): TeamState | undefined => state.teams.find((it) => it.id == state.stat?.currentTeamId),
        getPiratesIds: memoize((state): string[] | undefined => state.pirates?.map((it) => it.id)),
        getPirateById: (state, pirateId: string): GamePirate | undefined =>
            state.pirates?.find((it) => it.id === pirateId),
        getPirateCell: (state, pirateId: string): GamePlace | undefined => {
            const gamePirate = gameSlice.getSelectors().getPirateById(state, pirateId);
            if (!gamePirate) return undefined;
            const cell = state.fields[gamePirate.position.y][gamePirate.position.x];
            const level = cell.levels[gamePirate.position.level];
            return {
                cell,
                level,
            };
        },
        getUserSettings: (state): StorageState => state.userSettings,
        getGameSettings: (state): GameStateSettings => state.gameSettings,
        getGameField: (state, row: number, col: number): FieldState => state.fields[row][col],
        getMapForecasts: (state): string[] | undefined => state.mapForecasts,
        getPirateAutoChange: (state): boolean => state.hasPirateAutoChange,
        getIncludeMovesWithRum: (state): boolean => state.includeMovesWithRum,
        getGameStatistics: (state): GameStat | undefined => state.stat,
        getTeamScores: (state): TeamScores[] | undefined => {
            return state.teamScores?.map((it) => {
                const team = gameSlice.getSelectors().getTeamById(state, it.teamId);
                return {
                    teamId: team?.id,
                    name: team?.name,
                    backColor: team?.backColor,
                    coins: it.coins,
                } as TeamScores;
            });
        },
    },
});

export const {
    initMySettings,
    saveMySettings,
    initMap,
    initGame,
    initTeams,
    initPhotos,
    initSizes,
    initPiratePositions,
    setCurrentHumanTeam,
    setPirateAutoChange,
    setIncludeMovesWithRum,
    chooseHumanPirate,
    highlightPirate,
    highlightHumanMoves,
    removeHumanMoves,
    applyPirateChanges,
    applyChanges,
    updateLevelCoinsData,
    applyStat,
    setTilesPackNames,
    setMapForecasts,
} = gameSlice.actions;

export const {
    getCurrentTeam,
    getCurrentPlayerTeam,
    getCurrentPlayerPirates,
    getPiratesIds,
    getPirateById,
    getGameField,
    getGameSettings,
    getUserSettings,
    getMapForecasts,
    getPirateAutoChange,
    getIncludeMovesWithRum,
    getGameStatistics,
    getTeamScores,
} = gameSlice.selectors;

export default gameSlice.reducer;
