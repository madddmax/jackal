import reducer, {
    setCurrentHumanTeam,
    chooseHumanPirate,
    initMap,
    initTeams,
    initPhotos,
    initSizes,
    initPiratePositions,
    applyPirateChanges,
} from './gameSlice';
import { getMapData } from './gameSlice.test.mapData';
import { GamePirate, GameStat, GameState, PiratePosition } from './types';
import { Constants } from '/app/constants';
import { girlsMap } from '/app/global';

const stat2Data: GameStat = {
    turnNo: 1,
    currentTeamId: 1,
    isHumanPlayer: true,
    isGameOver: false,
    gameMessage: 'пиратская песня',
    teams: [
        {
            id: 1,
            name: 'girls',
            gold: 0,
            backcolor: 'red',
        },
        {
            id: 2,
            name: 'boys',
            gold: 0,
            backcolor: 'green',
        },
    ],
};

const stat4Data: GameStat = {
    turnNo: 1,
    currentTeamId: 1,
    isHumanPlayer: true,
    isGameOver: false,
    gameMessage: 'пиратская песня',
    teams: [
        {
            id: 1,
            name: 'girls',
            gold: 0,
            backcolor: 'red',
        },
        {
            id: 2,
            name: 'boys',
            gold: 0,
            backcolor: 'green',
        },
        {
            id: 3,
            name: 'cats',
            gold: 0,
            backcolor: 'white',
        },
        {
            id: 4,
            name: 'dogs',
            gold: 0,
            backcolor: 'black',
        },
    ],
};

const getPirates = (data: PiratePosition[]): GamePirate[] => {
    return data.map((it) => ({
        id: it.id,
        teamId: 2,
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
        teamId: 1,
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
        teamId: 2,
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

const getState = (pirates: GamePirate[]) => ({
    cellSize: 50,
    mapSize: 5,
    pirateSize: 15,
    fields: [[]],
    lastMoves: [],
    tilesPackNames: [],
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
        const result = reducer(defaultState, initMap(getMapData));
        expect(result).toHaveProperty('fields');
        expect(result.fields).toHaveLength(5);
        result.fields.forEach((it) => {
            expect(it).toHaveLength(5);
        });
    });

    test('Инициализируем команды для игры 1х1', () => {
        const result = reducer(defaultState, initTeams(stat2Data));
        expect(result.teams).toHaveLength(2);
        result.teams.forEach((it) => {
            expect(it.isHumanPlayer).not.toEqual(true);
        });
        expect(result.teams[0].group.id).toEqual(Constants.groupIds.girls);
        expect(result.teams[1].group.id).toEqual(Constants.groupIds.orcs);
    });

    test('Инициализируем команды для игры 2х2', () => {
        const result = reducer(defaultState, initTeams(stat4Data));
        expect(result.teams).toHaveLength(4);
        result.teams.forEach((it) => {
            expect(it.isHumanPlayer).not.toEqual(true);
        });
        expect(result.teams[0].group.id).toEqual(Constants.groupIds.girls);
        expect(result.teams[1].group.id).toEqual(Constants.groupIds.redalert);
    });

    test('Определяем фотки пираток', () => {
        const currentState = reducer(defaultState, initTeams(stat2Data));
        const result = reducer(currentState, initPhotos());
        expect(result.pirates).not.toBeUndefined();
        expect(result.pirates).not.toBeNull();
        expect(result.pirates).toHaveLength(2);
        result.pirates!.forEach((it) => {
            expect(it.photoId).toBeGreaterThan(0);
        });
        expect(result.pirates![0].groupId).toEqual(Constants.groupIds.girls);
        expect(result.pirates![0].photo).toContain(Constants.groupIds.girls + '/pirate_');
        expect(result.pirates![1].groupId).toEqual(Constants.groupIds.orcs);
        expect(result.pirates![1].photo).toContain(Constants.groupIds.orcs + '/pirate_');
    });

    test('Определяем размеры объектов на карте', () => {
        let result = reducer(
            defaultState,
            initSizes({
                width: 1000,
                height: 500,
            }),
        );
        expect(result.cellSize).toEqual(50);
        expect(result.pirateSize).toBeGreaterThanOrEqual(25);
        expect(result.pirateSize).toBeLessThanOrEqual(30);
        result = reducer(
            defaultState,
            initSizes({
                width: 720,
                height: 680,
            }),
        );
        expect(result.cellSize).toEqual(120);
        expect(result.pirateSize).toBeGreaterThanOrEqual(60);
        expect(result.pirateSize).toBeLessThanOrEqual(70);
    });

    test('Инициализируем словарик с позициями пираток', () => {
        reducer(defaultState, initPiratePositions());
        expect(girlsMap.Map).toEqual(
            expect.objectContaining({
                '20': { girls: ['100'], level: 0 },
                '4020': { girls: ['200'], level: 0 },
            }),
        );
    });

    test('Инициализируем пираток на карте и словарик с позициями пираток', () => {
        let currentState = reducer(defaultState, initMap(getMapData));
        currentState = reducer(currentState, initTeams(stat2Data));
        const result = reducer(
            currentState,
            applyPirateChanges({
                changes: testPirates.map((it) => ({ ...it })),
                isHumanPlayer: true,
                moves: [],
            }),
        );
        expect(result.fields[0][2].levels[0].pirates).toHaveLength(1);
        expect(result.fields[4][2].levels[0].pirates).toHaveLength(1);
        expect(girlsMap.Map).toEqual(
            expect.objectContaining({
                '20': { girls: ['100'], level: 0 },
                '4020': { girls: ['200'], level: 0 },
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
        previousState = reducer(previousState, initTeams(stat2Data));
        previousState = reducer(
            previousState,
            applyPirateChanges({
                changes: pirates,
                isHumanPlayer: true,
                moves: [],
            }),
        );
    });

    test('Устанавливаем текущую команду', () => {
        expect(reducer(previousState, setCurrentHumanTeam(2))).toEqual(
            expect.objectContaining({
                currentHumanTeamId: 2,
            }),
        );
    });

    test('Выбираем активного пирата', () => {
        let currentState = reducer(previousState, setCurrentHumanTeam(2));
        const result = reducer(currentState, chooseHumanPirate({ pirate: '200', withCoinAction: true }));
        expect(result.teams).toContainEqual({
            activePirate: '200',
            backColor: 'green',
            group: {
                id: Constants.groupIds.orcs,
                extension: '.jpg',
                photoMaxId: 6,
            },
            id: 2,
            isHumanPlayer: false,
        });
        expect(result.highlight_x).toEqual(2);
        expect(result.highlight_y).toEqual(4);
        const boy = result.fields[4][2].levels[0].pirates?.find((it) => it.id == '200');
        expect(boy).not.toBeUndefined();
        expect(boy).not.toBeNull();
        expect(boy?.isActive).toBeTruthy();
    });

    test('Меняем активного пирата', () => {
        let currentState = reducer(previousState, setCurrentHumanTeam(2));
        currentState = reducer(currentState, chooseHumanPirate({ pirate: '200', withCoinAction: true }));
        const result = reducer(currentState, chooseHumanPirate({ pirate: '300', withCoinAction: true }));
        expect(result.teams).toContainEqual({
            activePirate: '300',
            backColor: 'green',
            group: {
                id: Constants.groupIds.orcs,
                extension: '.jpg',
                photoMaxId: 6,
            },
            id: 2,
            isHumanPlayer: false,
        });
        expect(result.highlight_x).toEqual(2);
        expect(result.highlight_y).toEqual(4);
        const boy = result.fields[4][2].levels[0].pirates?.find((it) => it.id == '300');
        expect(boy).not.toBeUndefined();
        expect(boy).not.toBeNull();
        expect(boy?.isActive).toBeTruthy();
    });

    test('Уходим с клетки, на клетке - никого', () => {
        let currentState = reducer(previousState, setCurrentHumanTeam(2));
        currentState = reducer(currentState, chooseHumanPirate({ pirate: '100', withCoinAction: true }));
        const result = reducer(
            currentState,
            applyPirateChanges({
                changes: [
                    {
                        id: '100',
                        type: Constants.pirateTypes.Usual,
                        teamId: 2,
                        position: { level: 0, x: 2, y: 2 },
                    },
                ],
                isHumanPlayer: true,
                moves: [],
            }),
        );
        expect(result.highlight_x).toEqual(2);
        expect(result.highlight_y).toEqual(2);
        expect(result.fields[0][2].levels[0].pirates).toBeUndefined();
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
        previousState = reducer(previousState, initTeams(stat2Data));
        previousState = reducer(
            previousState,
            applyPirateChanges({
                changes: pirates,
                isHumanPlayer: true,
                moves: [
                    {
                        moveNum: 1,
                        from: { pirateIds: ['200'], level: 0, x: 2, y: 4 },
                        to: { pirateIds: ['200'], level: 0, x: 2, y: 2 },
                        withCoin: true,
                        withRespawn: false,
                    },
                    {
                        moveNum: 2,
                        from: { pirateIds: ['200'], level: 0, x: 2, y: 4 },
                        to: { pirateIds: ['200'], level: 0, x: 2, y: 2 },
                        withCoin: false,
                        withRespawn: false,
                    },
                ],
            }),
        );
    });

    test('Автоподнятие монеты по возможному действию', () => {
        expect(previousState.pirates?.find((it) => it.id == '200')?.withCoin).toBeTruthy();
    });

    test('Кладём монету', () => {
        let currentState = reducer(previousState, setCurrentHumanTeam(2));
        currentState = reducer(currentState, chooseHumanPirate({ pirate: '200', withCoinAction: true }));
        expect(currentState.highlight_x).toEqual(2);
        expect(currentState.highlight_y).toEqual(4);

        const result = reducer(currentState, chooseHumanPirate({ pirate: '200', withCoinAction: true }));

        expect(result.pirates?.find((it) => it.id == '200')?.withCoin).toBeFalsy();
        const boy = result.fields[4][2].levels[0].pirates?.find((it) => it.id == '200');
        expect(boy).not.toBeUndefined();
        expect(boy).not.toBeNull();
        expect(boy?.isActive).toBeTruthy();
        expect(boy?.withCoin).toBeFalsy();
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
        previousState = reducer(previousState, initTeams(stat2Data));
        previousState = reducer(
            previousState,
            applyPirateChanges({
                changes: pirates,
                isHumanPlayer: true,
                moves: [],
            }),
        );
        previousState = reducer(previousState, setCurrentHumanTeam(2));
        previousState = reducer(previousState, chooseHumanPirate({ pirate: '200', withCoinAction: true }));
    });

    test('Открываем Бен Ганна', () => {
        expect(previousState.fields[0][2].levels[0].pirates).toHaveLength(1);
        expect(previousState.fields[4][2].levels[0].pirates).toHaveLength(2);
        expect(previousState.highlight_x).toEqual(2);
        expect(previousState.highlight_y).toEqual(4);

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
                            y: 3,
                        },
                    },
                    {
                        id: '400',
                        type: Constants.pirateTypes.BenGunn,
                        teamId: 2,
                        position: {
                            level: 0,
                            x: 2,
                            y: 3,
                        },
                        isAlive: true,
                    },
                ],
                isHumanPlayer: true,
                moves: [],
            }),
        );

        expect(result.pirates?.find((it) => it.id == '200')?.position).toEqual({
            level: 0,
            x: 2,
            y: 3,
        });
        expect(result.highlight_x).toEqual(2);
        expect(result.highlight_y).toEqual(3);
        expect(result.fields[4][2].levels[0].pirates).toBeUndefined;
        expect(result.fields[3][2].levels[0].pirates).toHaveLength(2);
        expect(result.fields[3][2].levels[0].features).toBeUndefined();
        expect(girlsMap.Map).toEqual(
            expect.objectContaining({
                '20': { girls: ['100'], level: 0 },
                '3020': { girls: ['200', '400'], level: 0 },
                '4020': { girls: ['300'], level: 0 },
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
                isHumanPlayer: true,
                moves: [],
            }),
        );

        expect(result.pirates).toHaveLength(2);
        expect(result.highlight_x).toEqual(2);
        expect(result.highlight_y).toEqual(4);
        expect(result.fields[2][2].levels[0].pirates).toBeUndefined();
        expect(result.fields[4][2].levels[0].pirates).toHaveLength(1);
        expect(result.fields[4][2].levels[0].features).toHaveLength(1);
        expect(girlsMap.Map).toEqual(
            expect.objectContaining({
                '20': { girls: ['100'], level: 0 },
                //'2020': { girls: undefined, level: 0 },
                '4020': { girls: ['300'], level: 0 },
            }),
        );
    });
});
