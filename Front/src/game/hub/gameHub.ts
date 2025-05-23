import { GamePlayer } from '../redux/gameSlice.types';
import { debugLog, hubConnection } from '/app/global';
import { sagaActions } from '/common/sagas';

interface startGameRequestProps {
    players: GamePlayer[];
    mapId?: number;
    mapSize?: number;
    tilesPackName?: string;
    gameMode?: string;
}

interface makeGameMoveRequestProps {
    gameId: number;
    turnNum: number;
    pirateId: string;
}

const gameHub = {
    startGame: (settings: startGameRequestProps) => {
        hubConnection.invoke('start', { settings }).catch((err) => {
            debugLog(err);
        });
    },
    loadGame: (gameId: number) => {
        hubConnection.invoke('load', { gameId }).catch((err) => {
            debugLog(err);
        });
    },
    makeGameMove: (payload: makeGameMoveRequestProps) => {
        hubConnection.send('Move', payload);
    },
    getEventHandlers: [
        { name: 'GetStartData', sagaAction: sagaActions.GAME_START_APPLY_DATA },
        { name: 'LoadGameData', sagaAction: sagaActions.GAME_START_LOOKING_DATA },
        { name: 'GetMoveChanges', sagaAction: sagaActions.GAME_TURN_APPLY_DATA },
        { name: 'GetActiveGames', sagaAction: sagaActions.NET_GAMES_APPLY_DATA },
    ],
};

export default gameHub;
