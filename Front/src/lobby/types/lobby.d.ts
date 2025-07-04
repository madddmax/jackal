import { LeaderBoardItemResponse } from './lobbySaga';
import { GameInfo, NetGameInfo } from './lobbySlice';

export interface LobbyState {
    gamelist: GameInfo[];
    netgamelist: GameInfo[];
    netGame?: NetGameInfo;
    leaderBoard?: LeaderBoardItemResponse[];
}
