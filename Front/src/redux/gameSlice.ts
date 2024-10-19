import { createSlice, current, PayloadAction } from '@reduxjs/toolkit';
import {
    CellPirate,
    FieldState,
    GameCell,
    GameLevel,
    GameMap,
    GamePirate,
    GamePlace,
    GameStartResponse,
    GameStat,
    GameState,
    LevelFeature,
    PirateChanges,
    PirateChoose,
    PirateMoves,
    StorageState,
    TeamState,
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
        userSettings: {
            groups: [
                Constants.groupIds.girls,
                Constants.groupIds.redalert,
                Constants.groupIds.orcs,
                Constants.groupIds.skulls,
            ],
            mapSize: 11,
            players: ['human', 'robot2', 'robot', 'robot2'],
            playersCount: 4,
        },
        teams: [],
        currentHumanTeamId: 0,
        highlight_x: 0,
        highlight_y: 0,
        hasPirateAutoChange: true,
    } satisfies GameState as GameState,
    reducers: {
        initMySettings: (state, action: PayloadAction<StorageState>) => {
            localStorage.state = JSON.stringify(action.payload, null, 2);
            Object.assign(state.userSettings, action.payload);
        },
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
                            availableMoves: [],
                        });
                    } else
                        row.push({
                            image: change.backgroundImageSrc,
                            levels: change.levels,
                            availableMoves: [],
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
            state.teams = action.payload.stats.teams.map((it, idx, arr) => {
                let grId = arr.length == 2 && idx == 1 ? 2 : idx;
                return {
                    id: it.id,
                    activePirate: '',
                    isHumanPlayer: it.name.includes('Human'),
                    backColor: it.backcolor,
                    group:
                        Constants.groups.find((gr) => gr.id == state.userSettings.groups[grId]) || Constants.groups[0],
                };
            });
            state.lastMoves = [];
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
                        it.photo = `${team.group.id}/pirate_${arr[index]}${team.group.extension || '.png'}`;
                        it.photoId = arr[index];
                        it.groupId = team.group.id;
                    });
            });
            debugLog('state.pirates', state.pirates);

            const width = window.innerWidth;
            const height = window.innerHeight - 56;
            const mSize = width > height ? height : width;
            if (mSize > 560) {
                state.cellSize = Math.floor(mSize / state.mapSize / 10) * 10;
            }
            state.pirateSize = state.cellSize * 0.55;
            state.highlight_x = 0;
            state.highlight_y = 0;
        },
        setCurrentHumanTeam: (state, action: PayloadAction<number>) => {
            if (action.payload !== undefined && action.payload !== state.currentHumanTeamId) {
                state.currentHumanTeamId = action.payload;
            }
        },
        setPirateAutoChange: (state, action: PayloadAction<boolean>) => {
            state.hasPirateAutoChange = action.payload;
        },
        chooseHumanPirate: (state, action: PayloadAction<PirateChoose>) => {
            let pirate = gameSlice.selectors.getPirateById({ game: state }, action.payload.pirate)!;
            let currentTeam = gameSlice.selectors.getCurrentTeam({ game: state })!;
            let hasPirateChanging = currentTeam.activePirate !== pirate.id;
            if (hasPirateChanging) {
                const prevPirate = gameSlice.selectors.getCellPirateById({ game: state }, currentTeam.activePirate);
                if (prevPirate) prevPirate.isActive = false;
                const nextPirate = gameSlice.selectors.getCellPirateById({ game: state }, pirate.id);
                if (nextPirate) nextPirate.isActive = false;

                state.teams
                    .filter((it) => it.id === currentTeam.id)
                    .forEach((it) => {
                        it.activePirate = pirate.id;
                    });
            }

            let hasCoinChanging = action.payload.withCoinAction && !hasPirateChanging && pirate.withCoin !== undefined;
            if (hasCoinChanging) {
                const level = state.fields[pirate.position.y][pirate.position.x].levels[pirate.position.level];
                const cellPirate = level.pirates?.find((it) => it.id == pirate.id)!;
                if (
                    level.pirates!.filter((pr) => pr.id != pirate.id && pr.withCoin).length < Number(level.coin?.text)
                ) {
                    pirate.withCoin = !pirate.withCoin;
                    cellPirate.withCoin = pirate.withCoin;
                }
            }

            if (hasPirateChanging || hasCoinChanging) {
                gameSlice.caseReducers.highlightHumanMoves(state, highlightHumanMoves({}));
            }
        },
        highlightHumanMoves: (state, action: PayloadAction<PirateMoves>) => {
            let currentTeam = gameSlice.selectors.getCurrentTeam({ game: state })!;
            console.log('highlightMoves');
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
                const prevPirate = gameSlice.selectors.getCellPirateById({ game: state }, currentTeam.activePirate);
                if (prevPirate) prevPirate.isActive = false;

                currentTeam.activePirate = state.lastMoves[0].from.pirateIds[0];
            }

            const pirate = gameSlice.selectors.getPirateById({ game: state }, currentTeam.activePirate);
            if (!pirate) return;

            if (pirate?.position.x != state.highlight_x || pirate?.position.y != state.highlight_y) {
                const prevCell = state.fields[state.highlight_y][state.highlight_x];
                prevCell.highlight = false;
            }

            state.highlight_x = pirate?.position.x || 0;
            state.highlight_y = pirate?.position.y || 0;
            const curCell = state.fields[state.highlight_y][state.highlight_x];
            curCell.highlight = true;

            const cellPirate = gameSlice.selectors.getCellPirateById({ game: state }, pirate.id);
            if (cellPirate) cellPirate.isActive = true;

            // собственно подсвечиваем ходы
            state.lastMoves
                .filter(
                    (move) =>
                        move.from.pirateIds.includes(currentTeam.activePirate) &&
                        ((pirate?.withCoin && move.withCoin) ||
                            pirate?.withCoin === undefined ||
                            (!pirate?.withCoin && !move.withCoin)),
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
        applyPirateChanges: (state, action: PayloadAction<PirateChanges>) => {
            let cached = {} as { [id: number]: GameLevel };

            action.payload.changes
                .filter((it) => it.isAlive === undefined)
                .forEach((it) => {
                    let pirate = state.pirates!.find((pr) => pr.id === it.id)!;
                    const prevLevel = state.fields[pirate.position.y][pirate.position.x].levels[pirate.position.level];
                    cached[pirate.position.y * 1000 + pirate.position.x * 10 + pirate.position.level] = prevLevel;
                    if (prevLevel.pirates != undefined) {
                        prevLevel.pirates = prevLevel.pirates.filter((it) => it.id != pirate.id);
                        if (prevLevel.pirates.length == 0) prevLevel.pirates = undefined;
                    }
                });
            action.payload.changes.forEach((it) => {
                let team = state.teams.find((tm) => tm.id == it.teamId)!;
                if (it.isAlive === false) {
                    const place = gameSlice.selectors.getPirateCell({ game: state }, it.id);
                    if (place?.pirate != undefined) {
                        debugLog('dead pirate', current(place?.pirate));
                        const skull: LevelFeature = {
                            photo: place.cell.image?.includes('arrow') ? 'skull.png' : 'skull_light.png',
                            backgroundColor: 'transparent',
                        };
                        if (place.level.features === undefined) place.level.features = [skull];
                        else place.level.features.push(skull);
                        place.level.pirates = place.level.pirates?.filter((pr) => pr.id !== it.id);
                    }
                    state.pirates = state.pirates?.filter((pr) => pr.id !== it.id);
                } else if (it.isAlive === true) {
                    let nm;
                    let pname;
                    if (it.type == Constants.pirateTypes.Gann && team.group.gannMaxId) {
                        pname = `${team.group.id}/gann`;
                        nm = getAnotherRandomValue(
                            1,
                            team.group.gannMaxId,
                            state.pirates
                                ?.filter((pr) => pr.teamId == it.teamId && pr.type == Constants.pirateTypes.Gann)
                                .map((pr) => pr.photoId ?? 0) ?? [],
                        );
                    } else if (it.type == Constants.pirateTypes.Gann && !team.group.gannMaxId) {
                        pname = 'commonganns/gann';
                        nm = getAnotherRandomValue(
                            1,
                            Constants.commonGannMaxId,
                            state.pirates
                                ?.filter(
                                    (pr) =>
                                        pr.type == Constants.pirateTypes.Gann &&
                                        !Constants.groups.find((gr) => gr.id == pr.id)?.gannMaxId,
                                )
                                .map((pr) => pr.photoId ?? 0) ?? [],
                        );
                    } else if (it.type == Constants.pirateTypes.Friday) {
                        pname = 'commonfridays/friday';
                        nm = getAnotherRandomValue(1, Constants.commonFridayMaxId, []);
                    } else {
                        pname = `${team.group.id}/pirate`;
                        nm = getAnotherRandomValue(
                            1,
                            team.group.photoMaxId,
                            state.pirates?.filter((pr) => pr.teamId == it.teamId).map((pr) => pr.photoId ?? 0) ?? [],
                        );
                    }

                    state.pirates?.push({
                        id: it.id,
                        teamId: it.teamId,
                        position: it.position,
                        groupId: team.group.id,
                        photo: `${pname}_${nm}${team.group.extension || '.png'}`,
                        photoId: nm,
                        type: it.type,
                    });
                    const level = state.fields[it.position.y][it.position.x].levels[it.position.level];
                    const drawPirate: CellPirate = {
                        id: it.id,
                        teamId: it.teamId,
                        photo: `${pname}_${nm}${team.group.extension || '.png'}`,
                        photoId: nm + 100 * it.type,
                        backgroundColor: team.backColor,
                        isActive: it.id === team.activePirate,
                    };
                    if (level.pirates == undefined) level.pirates = [drawPirate];
                    else level.pirates.push(drawPirate);
                } else {
                    let pirate = state.pirates!.find((pr) => pr.id === it.id)!;
                    debugLog('moved pirate', current(pirate));

                    let cachedId = pirate.position.y * 1000 + pirate.position.x * 10 + pirate.position.level;
                    if (!cached.hasOwnProperty(cachedId)) {
                        cached[cachedId] =
                            state.fields[pirate.position.y][pirate.position.x].levels[pirate.position.level];
                    }
                    const prevLevel = cached[cachedId];
                    if (prevLevel.pirates != undefined) {
                        prevLevel.pirates = prevLevel.pirates.filter((it) => it.id != pirate.id);
                        if (prevLevel.pirates.length == 0) prevLevel.pirates = undefined;
                    }
                    pirate.position = it.position;
                    pirate.withRum = it.isDrunk;
                    pirate.isInTrap = it.isInTrap;
                    pirate.isInHole = it.isInHole;

                    const level = state.fields[pirate.position.y][pirate.position.x].levels[pirate.position.level];
                    debugLog('drawPirate', current(pirate).position.x, current(pirate).position.y, current(pirate).id);
                    const drawPirate: CellPirate = {
                        id: pirate.id,
                        teamId: it.teamId,
                        photo: pirate.photo,
                        photoId: pirate.photoId,
                        withCoin: pirate.withCoin,
                        isInTrap: it.isInTrap,
                        isInHole: it.isInHole,
                        isDrunk: it.isDrunk,
                        isActive: it.id === team.activePirate,
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
                    let changeCoin = girlIds.has(it.id) ? true : undefined;
                    if (changeCoin != it.withCoin) {
                        let cachedId = it.position.y * 1000 + it.position.x * 10 + it.position.level;
                        if (!cached.hasOwnProperty(cachedId)) {
                            cached[cachedId] = state.fields[it.position.y][it.position.x].levels[it.position.level];
                        }

                        const level = cached[cachedId];
                        if (changeCoin !== undefined) {
                            changeCoin =
                                level.pirates!.filter((pr) => pr.id != it.id && pr.withCoin).length <
                                Number(level.coin?.text);
                        }
                        it.withCoin = changeCoin;
                        const prt = level.pirates?.find((pr) => pr.id == it.id)!;
                        prt.withCoin = changeCoin;
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
                    features: cell?.levels && cell?.levels[lev.level]?.features,
                }));
            });
        },
        applyStat: (state, action: PayloadAction<GameStat>) => {
            state.stat = action.payload;
        },
    },
    selectors: {
        getTeamById: (state, teamId: number): TeamState | undefined => state.teams.find((it) => it.id == teamId),
        getCurrentTeam: (state): TeamState | undefined => state.teams.find((it) => it.id == state.currentHumanTeamId),
        getPirateById: (state, pirateId: string): GamePirate | undefined =>
            state.pirates!.find((it) => it.id === pirateId),
        getCellPirateById: (state, pirateId: string): CellPirate | undefined => {
            let gamePirate = gameSlice.selectors.getPirateById({ game: state }, pirateId);
            if (!gamePirate) return undefined;
            let level = state.fields[gamePirate.position.y][gamePirate.position.x].levels[gamePirate.position.level];
            return level.pirates?.find((it) => it.id == gamePirate?.id);
        },
        getPirateCell: (state, pirateId: string): GamePlace | undefined => {
            let gamePirate = gameSlice.selectors.getPirateById({ game: state }, pirateId);
            if (!gamePirate) return undefined;
            let cell = state.fields[gamePirate.position.y][gamePirate.position.x];
            let level = cell.levels[gamePirate.position.level];
            return {
                cell,
                level,
                pirate: level.pirates?.find((it) => it.id == gamePirate?.id),
            };
        },
    },
});

export const {
    initMySettings,
    initMap,
    setCurrentHumanTeam,
    setPirateAutoChange,
    chooseHumanPirate,
    highlightHumanMoves,
    applyPirateChanges,
    applyChanges,
    initGame,
    applyStat,
} = gameSlice.actions;

export const { getCurrentTeam } = gameSlice.selectors;

export default gameSlice.reducer;
