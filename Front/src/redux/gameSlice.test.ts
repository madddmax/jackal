import reducer, { setCurrentHumanTeam, chooseHumanPirate } from './gameSlice';
import { GameState } from './types';

describe('redux logic tests', () => {
    const previousState: GameState = {
        cellSize: 50,
        pirateSize: 15,
        fields: [
            [{ levels: [{ level: 0, hasCoins: false }] }],
            [{ levels: [{ level: 0, hasCoins: false }] }],
            [{ levels: [{ level: 0, hasCoins: false }] }],
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
                group: 'girls',
                photo: 'pirate_10',
                photoId: 10,
            },
            {
                id: '200',
                teamId: 2,
                position: {
                    level: 0,
                    x: 0,
                    y: 2,
                },
                group: 'somali',
                photo: 'pirate_20',
                photoId: 20,
            },
        ],
        teams: [
            {
                id: 2,
                activePirate: '200',
                lastPirate: '200',
                isHumanPlayer: false,
                group: 'somali',
            },
        ],
        currentHumanTeam: {
            id: -1,
            activePirate: '',
            lastPirate: '',
            isHumanPlayer: true,
            group: 'girls',
        },
        highlight_x: 0,
        highlight_y: 0,
    };

    test('Устанавливаем текущую команду', () => {
        expect(reducer(previousState, setCurrentHumanTeam(2))).toEqual(
            expect.objectContaining({
                currentHumanTeam: {
                    id: 2,
                    activePirate: '200',
                    lastPirate: '200',
                    isHumanPlayer: false,
                    group: 'somali',
                },
            }),
        );
    });

    test('Выбираем активного пирата', () => {
        expect(reducer(previousState, chooseHumanPirate({ pirate: '200' }))).toEqual(
            expect.objectContaining({
                currentHumanTeam: {
                    activePirate: '200',
                    lastPirate: '200',
                    group: 'girls',
                    id: -1,
                    isHumanPlayer: true,
                },
                teams: [
                    {
                        activePirate: '200',
                        lastPirate: '200',
                        group: 'somali',
                        id: 2,
                        isHumanPlayer: false,
                    },
                ],
            }),
        );
    });
});
