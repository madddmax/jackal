import reducer, {
    setCurrentHumanTeam,
    chooseHumanPirate,
    initMap,
    initTeams,
    initPhotos,
    initSizes,
} from './gameSlice';
import { getMapData } from './gameSlice.test.mapData';
import { GameStat, GameState } from './types';
import { Constants } from '/app/constants';

describe('redux init tests', () => {
    const previousState: GameState = {
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
            gameSpeed: 1,
        },
        teams: [],
        pirates: [
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
                type: 'Usual',
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
                type: 'Usual',
            },
        ],
        currentHumanTeamId: 0,
        highlight_x: 0,
        highlight_y: 0,
        hasPirateAutoChange: true,
    };

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

    test('Инициализируем карту', () => {
        const result = reducer(previousState, initMap(getMapData));
        expect(result).toHaveProperty('fields');
        expect(result.fields).toHaveLength(5);
        result.fields.forEach((it) => {
            expect(it).toHaveLength(5);
        });
    });

    test('Инициализируем команды для игры 1х1', () => {
        const result = reducer(previousState, initTeams(stat2Data));
        expect(result.teams).toHaveLength(2);
        result.teams.forEach((it) => {
            expect(it.isHumanPlayer).not.toEqual(true);
        });
        expect(result.teams[0].group.id).toEqual(Constants.groupIds.girls);
        expect(result.teams[1].group.id).toEqual(Constants.groupIds.orcs);
    });

    test('Инициализируем команды для игры 2х2', () => {
        const result = reducer(previousState, initTeams(stat4Data));
        expect(result.teams).toHaveLength(4);
        result.teams.forEach((it) => {
            expect(it.isHumanPlayer).not.toEqual(true);
        });
        expect(result.teams[0].group.id).toEqual(Constants.groupIds.girls);
        expect(result.teams[1].group.id).toEqual(Constants.groupIds.redalert);
    });

    test('Определяем фотки пираток', () => {
        const currentState = reducer(previousState, initTeams(stat2Data));
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
            previousState,
            initSizes({
                width: 1000,
                height: 500,
            }),
        );
        expect(result.cellSize).toEqual(50);
        expect(result.pirateSize).toBeGreaterThanOrEqual(25);
        expect(result.pirateSize).toBeLessThanOrEqual(30);
        result = reducer(
            previousState,
            initSizes({
                width: 720,
                height: 680,
            }),
        );
        expect(result.cellSize).toEqual(120);
        expect(result.pirateSize).toBeGreaterThanOrEqual(60);
        expect(result.pirateSize).toBeLessThanOrEqual(70);
    });
});

describe('redux logic tests', () => {
    const previousState: GameState = {
        tilesPackNames: [],
        hasPirateAutoChange: false,
        cellSize: 50,
        pirateSize: 15,
        fields: [
            [{ availableMoves: [], levels: [{ level: 0, hasCoins: false }] }],
            [
                {
                    availableMoves: [],
                    levels: [
                        {
                            level: 0,
                            hasCoins: false,
                            pirates: [
                                {
                                    id: '100',
                                    teamId: 2,
                                    isActive: false,
                                    backgroundColor: 'red',
                                    photo: 'test',
                                    photoId: 1,
                                },
                            ],
                        },
                    ],
                },
            ],
            [
                {
                    availableMoves: [],
                    levels: [
                        {
                            level: 0,
                            hasCoins: false,
                            pirates: [
                                {
                                    id: '200',
                                    teamId: 2,
                                    isActive: false,
                                    backgroundColor: 'red',
                                    photo: 'test',
                                    photoId: 1,
                                },
                            ],
                        },
                    ],
                },
            ],
        ],
        lastMoves: [],
        pirates: [
            {
                id: '100',
                teamId: 1,
                position: {
                    level: 0,
                    x: 0,
                    y: 1,
                },
                groupId: Constants.groupIds.girls,
                photo: 'pirate_10',
                photoId: 10,
                type: Constants.pirateTypes.Usual,
            },
            {
                id: '200',
                teamId: 2,
                position: {
                    level: 0,
                    x: 0,
                    y: 2,
                },
                groupId: Constants.groupIds.somali,
                photo: 'pirate_20',
                photoId: 20,
                type: Constants.pirateTypes.Usual,
            },
        ],
        userSettings: {
            groups: [],
            mapSize: 11,
            gameSpeed: 1,
        },
        teams: [
            {
                id: 2,
                activePirate: '200',
                isHumanPlayer: false,
                backColor: 'red',
                group: {
                    id: Constants.groupIds.somali,
                    photoMaxId: 7,
                },
            },
        ],
        currentHumanTeamId: 2,
        highlight_x: 0,
        highlight_y: 0,
    };

    test('Устанавливаем текущую команду', () => {
        expect(reducer(previousState, setCurrentHumanTeam(2))).toEqual(
            expect.objectContaining({
                currentHumanTeamId: 2,
            }),
        );
    });

    test('Выбираем активного пирата', () => {
        expect(reducer(previousState, chooseHumanPirate({ pirate: '200', withCoinAction: true }))).toEqual(
            expect.objectContaining({
                teams: [
                    {
                        activePirate: '200',
                        backColor: 'red',
                        group: {
                            id: Constants.groupIds.somali,
                            photoMaxId: 7,
                        },
                        id: 2,
                        isHumanPlayer: false,
                    },
                ],
            }),
        );
    });
});
