import { LeaderBoardItemResponse } from '../types/sagaContracts';
import { UserInfo } from '/auth/redux/authSlice.types';
import { GameInfo, LobbyInfo, NetGameEntryResponse, NetGameInfoResponse } from '/common/redux.types';
import { GameSettings } from '/game/types/hubContracts';

export interface LobbyState {
    gamelist: GameInfo[];
    netgamelist: GameInfo[];
    netGame?: NetGameInfo;
    lobby?: LobbyInfo;
    leaderBoard?: LeaderBoardItemResponse[];
}

export interface LobbyGamesEntriesList {
    currentUserId?: number;
    gamesEntries: NetGameEntryResponse[];
}

export interface LobbyGameInfo {
    currentUserId?: number;
    gameInfo: NetGameInfoResponse;
}

export interface NetGameInfo {
    id: number;
    gameId?: number;
    isCreator: boolean;
    settings: GameSettings;
    viewers: number[];
    users: UserInfo[];
}
