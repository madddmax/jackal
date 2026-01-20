import { LeaderBoardItemResponse } from './lobbySaga';
import { GameInfo, NetGameInfo } from './lobbySlice';

export interface LobbyState {
    gamelist: GameInfo[];
    netgamelist: GameInfo[];
    netGame?: NetGameInfo;
    leaders: LeaderBoardsInfo;
    usersOnline: number[];
}

export interface LeaderBoardsInfo {
    localLeaders: LeaderBoardItemResponse[];
    netLeaders: LeaderBoardItemResponse[];
    botLeaders: LeaderBoardItemResponse[];
    timestamp: number;
}
