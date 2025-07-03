import { GameState } from '../types';
import { GameTeamResponse } from '../types/gameSaga';
import reducer, {
    applyChanges,
    applyPirateChanges,
    chooseHumanPirate,
    highlightHumanMoves,
    initMap,
    initPhotos,
    initPiratePositions,
    initSizes,
    initTeams,
    removeHumanMoves,
    setCurrentHumanTeam,
} from './gameSlice';
import { getMapData } from './mapDataForTests';
import { Constants } from '/app/constants';
import { girlsMap } from '/app/global';

const testTeamId = 12;

const twoTeamsData: GameTeamResponse[] = [
    {
        id: 5,
        name: 'girls',
        coins: 0,
        userId: 0,
        isHuman: false,
        ship: {
            x: 5,
            y: 0,
        },
    },
    {
        id: testTeamId,
        isCurrentUser: true,
        name: 'boys',
        userId: 2,
        isHuman: true,
        coins: 0,
        ship: {
            x: 5,
            y: 10,
        },
    },
];

const fourTeamsData: GameTeamResponse[] = [
    {
        id: 5,
        name: 'girls',
        userId: 0,
        isHuman: false,
        coins: 0,
        ship: {
            x: 5,
            y: 0,
        },
    },
    {
        id: testTeamId,
        name: 'boys',
        userId: 2,
        isHuman: true,
        coins: 0,
        ship: {
            x: 0,
            y: 5,
        },
    },
    {
        id: 7,
        name: 'cats',
        userId: 0,
        isHuman: false,
        coins: 0,
        ship: {
            x: 5,
            y: 10,
        },
    },
    {
        id: 8,
        name: 'dogs',
        userId: 0,
        isHuman: false,
        coins: 0,
        ship: {
            x: 10,
            y: 5,
        },
    },
];

const getPirates = (data: GamePiratePosition[]): GamePirate[] => {
    return data.map((it) => ({
        id: it.id,
        teamId: testTeamId,
        position: it.position,
        groupId: '',
        photo: '',
        photoId: 0,
        withCoin: false,
        type: Constants.pirateTypes.Usual,
    }));
};

const testPirates: GamePirate[] = [
    {
        id: '100',
        teamId: 5,
        position: {
            level: 0,
            x: 2,
            y: 0,
        },
        groupId: '',
        photo: '',
        photoId: 0,
        type: Constants.pirateTypes.Usual,
    },
    {
        id: '200',
        teamId: testTeamId,
        position: {
            level: 0,
            x: 2,
            y: 4,
        },
        groupId: '',
        photo: '',
        photoId: 0,
        type: Constants.pirateTypes.Usual,
    },
];

const getState = (pirates: GamePirate[]): GameState => ({
    fields: [[]],
    lastMoves: [],
    gameSettings: {
        mapSize: 5,
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
        gameSpeed: 0,
    },
    stat: {
        turnNumber: 1,
        currentTeamId: testTeamId,
        isGameOver: false,
        gameMessage: '',
    },
    teams: [],
    pirates: pirates,
    currentHumanTeamId: 0,
    highlight_x: 0,
    highlight_y: 0,
    hasPirateAutoChange: true,
});

describe('redux init tests', () => {
    let defaultState: GameState;

    beforeAll(() => {
        defaultState = getState(testPirates);
    });

    test('Инициализируем карту', () => {
        const result: GameState = reducer(defaultState, initMap(getMapData));
        expect(result).toHaveProperty('fields');
        expect(result.fields).toHaveLength(5);
        result.fields.forEach((it) => {
            expect(it).toHaveLength(5);
        });
    });

    test('Инициализируем команды для игры 1х1', () => {
        const result = reducer(defaultState, initTeams(twoTeamsData));
        expect(result.teams).toHaveLength(2);
        expect(result.teams[0].group.id).toEqual(Constants.groupIds.girls);
        expect(result.teams[1].group.id).toEqual(Constants.groupIds.orcs);
    });

    test('Инициализируем команды для игры 2х2', () => {
        const result = reducer(defaultState, initTeams(fourTeamsData));
        expect(result.teams).toHaveLength(4);
        expect(result.teams[0].group.id).toEqual(Constants.groupIds.girls);
        expect(result.teams[1].group.id).toEqual(Constants.groupIds.redalert);
    });

    test('Определяем фотки пираток', () => {
        const currentState = reducer(defaultState, initTeams(twoTeamsData));
        const result = reducer(currentState, initPhotos());
        expect(result.pirates).not.toBeUndefined();
        expect(result.pirates).not.toBeNull();
        expect(result.pirates).toHaveLength(2);
        result.pirates!.forEach((it) => {
            expect(it.photoId).toBeGreaterThan(0);
        });
        expect(result.pirates![0].groupId).toEqual(Constants.groupIds.girls);
        expect(result.pirates![0].photo).toContain(Constants.groupIds.girls + '/pirate_');
        expect(result.pirates![0].backgroundColor).toEqual('DarkRed');
        expect(result.pirates![1].groupId).toEqual(Constants.groupIds.orcs);
        expect(result.pirates![1].photo).toContain(Constants.groupIds.orcs + '/pirate_');
        expect(result.pirates![1].backgroundColor).toEqual('DarkBlue');
    });

    test('Определяем размеры объектов на карте', () => {
        let result = reducer(
            defaultState,
            initSizes({
                width: 1000,
                height: 500,
            }),
        );
        expect(result.gameSettings.cellSize).toEqual(50);
        expect(result.gameSettings.pirateSize).toBeGreaterThanOrEqual(25);
        expect(result.gameSettings.pirateSize).toBeLessThanOrEqual(30);
        result = reducer(
            defaultState,
            initSizes({
                width: 720,
                height: 680,
            }),
        );
        expect(result.gameSettings.cellSize).toEqual(120);
        expect(result.gameSettings.pirateSize).toBeGreaterThanOrEqual(60);
        expect(result.gameSettings.pirateSize).toBeLessThanOrEqual(70);
    });

    test('Инициализируем словарик с позициями пираток', () => {
        reducer(defaultState, initPiratePositions());
        expect(girlsMap.Map).toEqual(
            expect.objectContaining({
                '20': { girls: ['100'], level: 0, levelsCountInCell: 1 },
                '4020': { girls: ['200'], level: 0, levelsCountInCell: 1 },
            }),
        );
    });

    test('Инициализируем пираток на карте и словарик с позициями пираток', () => {
        let currentState = reducer(defaultState, initMap(getMapData));
        currentState = reducer(currentState, initTeams(twoTeamsData));
        const result = reducer(
            currentState,
            applyPirateChanges({
                changes: testPirates.map((it) => ({ ...it })),
                moves: [],
            }),
        );
        expect(result.pirates).toHaveLength(2);
        expect(girlsMap.Map).toEqual(
            expect.objectContaining({
                '20': { girls: ['100'], level: 0, levelsCountInCell: 1 },
                '4020': { girls: ['200'], level: 0, levelsCountInCell: 1 },
            }),
        );
    });

    test('Устанавливаем текущую команду', () => {
        const result = reducer(defaultState, initTeams(twoTeamsData));
        expect(reducer(result, setCurrentHumanTeam())).toEqual(
            expect.objectContaining({
                currentHumanTeamId: testTeamId,
            }),
        );
    });
});

describe('redux basic tests', () => {
    let previousState: GameState;

    beforeAll(() => {
        const pirates = getPirates([
            { id: '100', position: { level: 0, x: 2, y: 0 } },
            { id: '200', position: { level: 0, x: 2, y: 4 } },
            { id: '300', position: { level: 0, x: 2, y: 4 } },
        ]);

        previousState = getState(pirates);
        previousState = reducer(previousState, initMap(getMapData));
        previousState = reducer(previousState, initTeams(twoTeamsData));
        previousState = reducer(
            previousState,
            applyPirateChanges({
                changes: pirates,
                moves: [],
            }),
        );
        previousState = reducer(previousState, setCurrentHumanTeam());
    });

    test('Автовыбор пиратки, для которой возможны ходы, и подсвечивание её ходов', () => {
        expect(previousState.fields[2][2].availableMoves).toHaveLength(0);
        expect(previousState.highlight_x).toEqual(0);
        expect(previousState.highlight_y).toEqual(0);

        const result = reducer(
            previousState,
            highlightHumanMoves({
                moves: [
                    {
                        moveNum: 1,
                        from: { pirateIds: ['200'], level: 0, x: 2, y: 4 },
                        to: { pirateIds: ['200'], level: 0, x: 2, y: 2 },
                        withCoin: true,
                        withBigCoin: false,
                        withRespawn: false,
                    },
                ],
            }),
        );

        expect(result.fields[2][2].availableMoves).toHaveLength(1);
        expect(result.teams.find((it) => it.id === testTeamId)?.activePirate).toEqual('200');
        expect(result.highlight_x).toEqual(2);
        expect(result.highlight_y).toEqual(4);
        const girl = result.pirates?.find((it) => it.id == '200');
        expect(girl).not.toBeUndefined();
        expect(girl).not.toBeNull();
        expect(girl?.isActive).toBeTruthy();
    });

    test('Убираем подсветку ходов', () => {
        const currentState = reducer(
            previousState,
            highlightHumanMoves({
                moves: [
                    {
                        moveNum: 1,
                        from: { pirateIds: ['200'], level: 0, x: 2, y: 4 },
                        to: { pirateIds: ['200'], level: 0, x: 2, y: 2 },
                        withCoin: true,
                        withBigCoin: false,
                        withRespawn: false,
                    },
                ],
            }),
        );

        expect(currentState.fields[2][2].availableMoves).toHaveLength(1);

        const result = reducer(currentState, removeHumanMoves());

        expect(result.fields[2][2].availableMoves).toHaveLength(0);
    });

    test('Выбираем активного пирата', () => {
        const result = reducer(previousState, chooseHumanPirate({ pirate: '200', withCoinAction: true }));
        expect(result.teams.find((it) => it.id === testTeamId)?.activePirate).toEqual('200');
        expect(result.highlight_x).toEqual(2);
        expect(result.highlight_y).toEqual(4);
        const boy = result.pirates?.find((it) => it.id == '200');
        expect(boy).not.toBeUndefined();
        expect(boy).not.toBeNull();
        expect(boy?.isActive).toBeTruthy();
    });

    test('Меняем активного пирата', () => {
        const currentState = reducer(previousState, chooseHumanPirate({ pirate: '200', withCoinAction: true }));
        const result = reducer(currentState, chooseHumanPirate({ pirate: '300', withCoinAction: true }));
        expect(result.teams).toContainEqual({
            activePirate: '300',
            backColor: 'DarkBlue',
            group: {
                id: Constants.groupIds.orcs,
                extension: '.jpg',
                photoMaxId: 6,
            },
            name: 'boys',
            id: testTeamId,
            isHuman: true,
            isCurrentUser: true,
        });
        expect(result.highlight_x).toEqual(2);
        expect(result.highlight_y).toEqual(4);
        const pboy = result.pirates?.find((it) => it.id == '200');
        expect(pboy?.isActive).toBeFalsy();
        const sboy = result.pirates?.find((it) => it.id == '300');
        expect(sboy?.isActive).toBeTruthy();
    });

    test('Уходим с клетки, на клетке - никого', () => {
        const currentState = reducer(previousState, chooseHumanPirate({ pirate: '100', withCoinAction: true }));
        const result = reducer(
            currentState,
            applyPirateChanges({
                changes: [
                    {
                        id: '100',
                        type: Constants.pirateTypes.Usual,
                        teamId: testTeamId,
                        position: { level: 0, x: 2, y: 2 },
                    },
                ],
                moves: [],
            }),
        );
        expect(result.highlight_x).toEqual(2);
        expect(result.highlight_y).toEqual(2);
        const boy = result.pirates?.find((it) => it.id == '100');
        expect(boy?.position).toEqual({ level: 0, x: 2, y: 2 });
        expect(girlsMap.Map).toEqual(
            expect.objectContaining({
                '2020': { girls: ['100'], level: 0, levelsCountInCell: 1 },
                '4020': { girls: ['200', '300'], level: 0, levelsCountInCell: 1 },
            }),
        );
    });
});

describe('redux money actions tests', () => {
    let previousState: GameState;

    beforeAll(() => {
        const pirates = getPirates([
            { id: '100', position: { level: 0, x: 2, y: 0 } },
            { id: '200', position: { level: 0, x: 2, y: 4 } },
            { id: '300', position: { level: 0, x: 2, y: 4 } },
        ]);

        previousState = getState(pirates);
        previousState = reducer(previousState, initMap(getMapData));
        previousState = reducer(previousState, initTeams(twoTeamsData));
        previousState = reducer(
            previousState,
            applyPirateChanges({
                changes: pirates,
                moves: [
                    {
                        moveNum: 1,
                        from: { pirateIds: ['200'], level: 0, x: 2, y: 4 },
                        to: { pirateIds: ['200'], level: 0, x: 2, y: 2 },
                        withCoin: true,
                        withBigCoin: false,
                        withRespawn: false,
                    },
                    {
                        moveNum: 2,
                        from: { pirateIds: ['200'], level: 0, x: 2, y: 4 },
                        to: { pirateIds: ['200'], level: 0, x: 2, y: 2 },
                        withCoin: false,
                        withBigCoin: false,
                        withRespawn: false,
                    },
                ],
            }),
        );
    });

    test('Автоподнятие монеты по возможному действию', () => {
        expect(previousState.pirates?.find((it) => it.id == '200')?.withCoin).toBeTruthy();
        const level = previousState.fields[4][2].levels[0];
        expect(level).toEqual({
            info: {
                level: 0,
                coins: 2,
                bigCoins: 0,
            },
            pirates: {
                coins: 1,
                bigCoins: 0,
            },
            hasFreeMoney: true,
            freeCoinGirlId: '300',
        });
    });

    test('Кладём монету', () => {
        let currentState = reducer(previousState, setCurrentHumanTeam());
        currentState = reducer(currentState, chooseHumanPirate({ pirate: '200', withCoinAction: true }));
        expect(currentState.highlight_x).toEqual(2);
        expect(currentState.highlight_y).toEqual(4);
        expect(currentState.fields[4][2].levels[0]).toEqual({
            info: {
                level: 0,
                coins: 2,
                bigCoins: 0,
            },
            pirates: {
                coins: 1,
                bigCoins: 0,
            },
            hasFreeMoney: true,
            freeCoinGirlId: '300',
        });

        const result = reducer(currentState, chooseHumanPirate({ pirate: '200', withCoinAction: true }));

        const boy = result.pirates?.find((it) => it.id == '200');
        expect(boy?.withCoin).toBeFalsy();
        expect(boy?.isActive).toBeTruthy();
        expect(result.fields[4][2].levels[0]).toEqual({
            info: {
                level: 0,
                coins: 2,
                bigCoins: 0,
            },
            pirates: {
                coins: 0,
                bigCoins: 0,
            },
            hasFreeMoney: true,
            freeCoinGirlId: '200',
        });
    });
});

describe('redux logic tests', () => {
    let previousState: GameState;

    beforeAll(() => {
        const pirates = getPirates([
            { id: '100', position: { level: 0, x: 2, y: 0 } },
            { id: '200', position: { level: 0, x: 2, y: 4 } },
            { id: '300', position: { level: 0, x: 2, y: 4 } },
        ]);

        previousState = getState(pirates);
        previousState = reducer(previousState, initMap(getMapData));
        previousState = reducer(previousState, initTeams(twoTeamsData));
        previousState = reducer(
            previousState,
            applyPirateChanges({
                changes: pirates,
                moves: [],
            }),
        );
        previousState = reducer(previousState, setCurrentHumanTeam());
        previousState = reducer(previousState, chooseHumanPirate({ pirate: '200', withCoinAction: true }));
    });

    test('Производим изменения на карте', () => {
        const result = reducer(
            previousState,
            applyChanges([
                {
                    backgroundImageSrc: '/fields/forest.png',
                    rotate: 2,
                    levels: [
                        { level: 0, coins: 0, bigCoins: 0 },
                        { level: 1, coins: 0, bigCoins: 0 },
                        { level: 2, coins: 0, bigCoins: 0 },
                    ],
                    x: 2,
                    y: 4,
                },
            ]),
        );

        expect(result.fields[4][2].levels[0]).toEqual({
            info: {
                level: 0,
                coins: 0,
                bigCoins: 0,
            },
            pirates: {
                coins: 0,
                bigCoins: 0,
            },
            hasFreeMoney: false,
            features: undefined,
        });
    });

    test('Открываем Бен Ганна', () => {
        expect(previousState.pirates).toHaveLength(3);
        expect(previousState.highlight_x).toEqual(2);
        expect(previousState.highlight_y).toEqual(4);

        const result = reducer(
            previousState,
            applyPirateChanges({
                changes: [
                    {
                        id: '200',
                        type: Constants.pirateTypes.Usual,
                        teamId: testTeamId,
                        position: {
                            level: 0,
                            x: 2,
                            y: 3,
                        },
                    },
                    {
                        id: '400',
                        type: Constants.pirateTypes.BenGunn,
                        teamId: testTeamId,
                        position: {
                            level: 0,
                            x: 2,
                            y: 3,
                        },
                        isAlive: true,
                    },
                ],
                moves: [],
            }),
        );

        expect(result.pirates).toHaveLength(4);
        const boy = result.pirates?.find((it) => it.id == '200');
        expect(boy?.position).toEqual({ level: 0, x: 2, y: 3 });
        expect(boy?.isActive).toBeTruthy();
        const ben = result.pirates?.find((it) => it.id == '400');
        expect(ben?.position).toEqual({ level: 0, x: 2, y: 3 });
        expect(ben?.isActive).toBeFalsy();
        expect(ben?.backgroundColor).toEqual('DarkBlue');
        expect(result.highlight_x).toEqual(2);
        expect(result.highlight_y).toEqual(3);
        expect(result.fields[3][2].levels[0]).toEqual({
            info: {
                level: 0,
                coins: 0,
                bigCoins: 0,
            },
            pirates: {
                coins: 0,
                bigCoins: 0,
            },
            hasFreeMoney: false,
            // freeCoinGirlId: '200',
            features: undefined,
        });
        expect(girlsMap.Map).toEqual(
            expect.objectContaining({
                '20': { girls: ['100'], level: 0, levelsCountInCell: 1 },
                '3020': { girls: ['200', '400'], level: 0, levelsCountInCell: 1 },
                '4020': { girls: ['300'], level: 0, levelsCountInCell: 1 },
            }),
        );
    });

    test('Открываем людоеда', () => {
        const result = reducer(
            previousState,
            applyPirateChanges({
                changes: [
                    {
                        id: '200',
                        type: Constants.pirateTypes.Usual,
                        teamId: 2,
                        position: {
                            level: 0,
                            x: 2,
                            y: 2,
                        },
                        isAlive: false,
                    },
                ],
                moves: [],
            }),
        );

        expect(result.pirates).toHaveLength(2);
        expect(result.highlight_x).toEqual(2);
        expect(result.highlight_y).toEqual(4);
        expect(result.fields[4][2].levels[0]).toEqual({
            info: {
                level: 0,
                coins: 2,
                bigCoins: 0,
            },
            pirates: {
                coins: 0,
                bigCoins: 0,
            },
            hasFreeMoney: true,
            freeCoinGirlId: '300',
            features: [
                {
                    photo: 'skull_light.png',
                    backgroundColor: 'transparent',
                },
            ],
        });
        expect(girlsMap.Map).toEqual(
            expect.objectContaining({
                '20': { girls: ['100'], level: 0, levelsCountInCell: 1 },
                '4020': { girls: ['300'], level: 0, levelsCountInCell: 1 },
            }),
        );
    });
});
