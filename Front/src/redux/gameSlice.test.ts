import reducer, { setCurrentHumanTeam } from './gameSlice';
import { GameState } from './types';

test('should handle a todo being added to an empty list', () => {
    const previousState: GameState = {
        cellSize: 50,
        pirateSize: 15,
        fields: [[]],
        lastMoves: [],
        teams: [
            {
                id: 2,
                activePirate: '123',
                lastPirate: '123',
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

    expect(reducer(previousState, setCurrentHumanTeam(2))).toEqual(
        expect.objectContaining({
            currentHumanTeam: {
                id: 2,
                activePirate: '123',
                lastPirate: '123',
                isHumanPlayer: false,
                group: 'somali',
            },
        }),
    );
});
