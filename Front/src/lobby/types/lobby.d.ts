import { GameInfo, NetGameInfo } from './lobbySlice';

export interface LobbyState {
    gamelist: GameInfo[];
    netgamelist: GameInfo[];
    netGame?: NetGameInfo;
}
