import { GameSettings, makeGameMoveRequestProps } from '../types/hubContracts';
import { debugLog, hubConnection } from '/app/global';
import { sagaActions } from '/common/sagas';

const gameHub = {
    startGame: (settings: GameSettings) => {
        hubConnection.invoke('start', { settings }).catch((err) => {
            debugLog(err);
        });
    },
    startPublicGame: (id: number, settings: GameSettings) => {
        hubConnection.invoke('startPublic', { id, settings }).catch((err) => {
            debugLog(err);
        });
    },
    loadGame: (gameId: number) => {
        hubConnection.invoke('load', { gameId }).catch((err) => {
            debugLog(err);
        });
    },
    makeGameMove: (payload: makeGameMoveRequestProps) => {
        hubConnection.send('move', payload);
    },
    netCreate: (settings: GameSettings) => {
        hubConnection.send('netStart', { settings });
    },
    netChange: (id: number, settings: GameSettings) => {
        hubConnection.send('netUpdate', { id, settings });
    },
    netJoin: (id: number) => {
        hubConnection.send('netJoin', { id });
    },
    getEventHandlers: [
        { name: 'GetStartData', sagaAction: sagaActions.GAME_START_APPLY_DATA },
        { name: 'LoadGameData', sagaAction: sagaActions.GAME_START_LOOKING_DATA },
        { name: 'GetMoveChanges', sagaAction: sagaActions.GAME_TURN_APPLY_DATA },
        { name: 'GetActiveGames', sagaAction: sagaActions.ACTIVE_GAMES_APPLY_DATA },
        { name: 'GetActiveNetGames', sagaAction: sagaActions.NET_GAMES_APPLY_DATA },
        { name: 'GetNetGameData', sagaAction: sagaActions.NET_GAME_APPLY_DATA },
        { name: 'GetUsersOnline', sagaAction: sagaActions.NET_GAME_USERS_ONLINE },
    ],
};

export default gameHub;
