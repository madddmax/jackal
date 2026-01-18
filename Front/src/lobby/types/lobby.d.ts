import { LeaderBoardItemResponse } from './lobbySaga';
import { GameInfo, NetGameInfo } from './lobbySlice';

export interface LobbyState {
    gamelist: GameInfo[];
    netgamelist: GameInfo[];
    netGame?: NetGameInfo;
    leaders: LeaderBoardsInfo;
}

export interface LeaderBoardsInfo {
    localLeaders: LeaderBoardItemResponse[];
    netLeaders: LeaderBoardItemResponse[];
    timestamp: number;
}
