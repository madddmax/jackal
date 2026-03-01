import { PayloadAction, createSlice, current } from '@reduxjs/toolkit';
import { memoize } from 'proxy-memoize';

import { InitPiratesPhoto } from '../logic/components/initPiratesPhoto';
import { constructGameLevel, girlsMap } from '../logic/gameLogic';
import {
    ChooseHumanPirateActionProps,
    FieldState,
    GamePlace,
    GameState,
    GameStateSettings,
    HighlightHumanMovesActionProps,
    StorageState,
    TakeOrPutCoinActionProps,
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
import { Constants, ImagesPacksIds } from '/app/constants';
import { debugLog } from '/app/global';
import { PlayerTypes } from '/common/constants';

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
            hasChessBar: true,
            players: [PlayerTypes.Human, PlayerTypes.Robot2, PlayerTypes.Robot, PlayerTypes.Robot2],
            playersMode: 4,
            gameSpeed: 1,
            imagesPackName: ImagesPacksIds.classic,
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
                        tileType: change.tileType,
                        image: Constants.imagesPacks[state.userSettings.imagesPackName] + change.tileType + '.png',
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
        refreshMap: (state) => {
            state.fields.map((col) => {
                col.map((it) => {
                    it.image = Constants.imagesPacks[state.userSettings.imagesPackName] + it.tileType + '.png';
                });
            });
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
                        const initPhoto = InitPiratesPhoto({
                            girlType: it.type,
                            allGirls: state.pirates,
                            teamId: it.teamId,
                            teamGroup: team.group,
                        });
                        it.photo = initPhoto.photo;
                        it.photoId = initPhoto.photoId;
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
            if (state.stat?.isCurrentUsersMove && state.stat.currentTeamId !== state.currentHumanTeamId) {
                state.currentHumanTeamId = state.stat.currentTeamId;
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
        },
        takeOrPutCoin: (state, action: PayloadAction<TakeOrPutCoinActionProps>) => {
            const selectors = gameSlice.getSelectors();
            const pirate = selectors.getPirateById(state, action.payload.pirate)!;

            const hasCoinChanging = pirate.withCoin !== undefined || pirate.withBigCoin !== undefined;
            if (hasCoinChanging) {
                const level = state.fields[pirate.position.y][pirate.position.x].levels[pirate.position.level];
                if (pirate.withBigCoin) {
                    pirate.withBigCoin = false;
                    if (level.pirates.coins < level.info.coins) {
                        pirate.withCoin = true;
                    }
                } else if (pirate.withCoin) {
                    pirate.withCoin = false;
                } else if (level.info.bigCoins > 0) {
                    if (level.pirates.bigCoins < level.info.bigCoins) {
                        // поднимаем лежачую большую монету
                        pirate.withBigCoin = true;
                    } else {
                        // нет лежачих - отнимаем у товарища
                        const cell = girlsMap.GetPosition(pirate);
                        const girls = selectors.getPiratesByIds(
                            state,
                            cell!.girls!.map((x) => x.id),
                        );
                        const bcGirl = girls?.find((it) => it.withBigCoin);
                        if (bcGirl) {
                            bcGirl.withBigCoin = false;
                            pirate.withBigCoin = true;
                        }
                    }
                } else if (level.info.coins > 0) {
                    if (level.pirates.coins < level.info.coins) {
                        // поднимаем лежачую монету
                        pirate.withCoin = true;
                    } else {
                        // нет лежачих - отнимаем у товарища
                        const cell = girlsMap.GetPosition(pirate);
                        const girls = selectors.getPiratesByIds(
                            state,
                            cell!.girls!.map((x) => x.id),
                        );
                        const bcGirl = girls?.find((it) => it.withCoin);
                        if (bcGirl) {
                            bcGirl.withCoin = false;
                            pirate.withCoin = true;
                        }
                    }
                }
                gameSlice.caseReducers.updateLevelCoinsData(state, updateLevelCoinsData(pirate));
                gameSlice.caseReducers.highlightHumanMoves(state, highlightHumanMoves({}));
            }
        },
        highlightHumanMoves: (state, action: PayloadAction<HighlightHumanMovesActionProps>) => {
            const selectors = gameSlice.getSelectors();
            const currentTeam = state.teams.find((it) => it.id == state.stat?.currentTeamId)!;

            if (!state.stat?.isCurrentUsersMove) return;

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
                        isQuake: move.withQuake,
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
                    const initPhoto = InitPiratesPhoto({
                        girlType: it.type,
                        allGirls: state.pirates,
                        teamId: it.teamId,
                        teamGroup: team.group,
                    });

                    state.pirates?.push({
                        id: it.id,
                        teamId: it.teamId,
                        position: it.position,
                        groupId: team.group.id,
                        photo: initPhoto.photo,
                        photoId: initPhoto.photoId,
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
            // поднятие/опускание и автоподнятие монет
            if (selectors.getGameStatistics(state)!.isCurrentUsersMove) {
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
                        const levelPirates = state.pirates?.filter((it) => cell?.girls?.some((x) => x.id == it.id));

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
                const backgroundImageSrc =
                    Constants.imagesPacks[state.userSettings.imagesPackName] + it.tileType + '.png';
                if (cell.image != backgroundImageSrc) {
                    cell.image = backgroundImageSrc;
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
            const levelPirates = state.pirates?.filter((it) => girlsLevel?.girls?.some((x) => x.id == it.id));
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
        checkBottles: (state, action: PayloadAction<GameScore[] | undefined>) => {
            const curTeam = gameSlice.getSelectors().getCurrentPlayerTeam(state);
            const curScores = state.teamScores?.find((it) => it.teamId === curTeam?.id);
            const newScores = action.payload?.find((it) => it.teamId === curTeam?.id);

            if (curScores && newScores && curScores.rumBottles > newScores.rumBottles) {
                state.includeMovesWithRum = false;
            }
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
            // const currentPlayerTeam = gameSlice.getSelectors().getCurrentPlayerTeam(state);
            return state.pirates?.filter((it) => it.teamId == state.currentHumanTeamId);
        },
        getPiratesIds: memoize((state): string[] | undefined => state.pirates?.map((it) => it.id)),
        getPirateById: (state, pirateId: string): GamePirate | undefined =>
            state.pirates?.find((it) => it.id === pirateId),
        getPiratesByIds: (state, pirateIds: string[]): GamePirate[] | undefined =>
            state.pirates?.filter((it) => pirateIds.indexOf(it.id) >= 0),
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
                    bottles: it.rumBottles,
                    wasteTime: it.wasteTime,
                } as TeamScores;
            });
        },
        getRumBottles: (state): number => {
            const curTeam = gameSlice.getSelectors().getCurrentPlayerTeam(state);
            const curScores = state.teamScores?.find((it) => it.teamId === curTeam?.id);
            return curScores?.rumBottles ?? 0;
        },
    },
});

export const {
    initMySettings,
    saveMySettings,
    initMap,
    refreshMap,
    initGame,
    initTeams,
    initPhotos,
    initSizes,
    initPiratePositions,
    setCurrentHumanTeam,
    setPirateAutoChange,
    setIncludeMovesWithRum,
    chooseHumanPirate,
    takeOrPutCoin,
    highlightPirate,
    highlightHumanMoves,
    removeHumanMoves,
    applyPirateChanges,
    applyChanges,
    updateLevelCoinsData,
    checkBottles,
    applyStat,
    setTilesPackNames,
    setMapForecasts,
} = gameSlice.actions;

export const {
    getCurrentPlayerTeam,
    getCurrentPlayerPirates,
    getPiratesIds,
    getPirateById,
    getPiratesByIds,
    getGameField,
    getGameSettings,
    getUserSettings,
    getMapForecasts,
    getPirateAutoChange,
    getIncludeMovesWithRum,
    getGameStatistics,
    getTeamScores,
    getRumBottles,
} = gameSlice.selectors;

export default gameSlice.reducer;
