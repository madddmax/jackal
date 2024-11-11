import reducer, { setCurrentHumanTeam, chooseHumanPirate } from './gameSlice';
import { GameState } from './types';
import { Constants } from '/app/constants';

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
                type: Constants.pirateTypes.Base,
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
                type: Constants.pirateTypes.Base,
            },
        ],
        userSettings: {
            groups: [],
            mapSize: 11,
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
