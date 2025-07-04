import { NetGameEntryResponse, NetGameInfoResponse } from './lobbySaga';
import { GameSettings } from '/game/types/hubContracts';

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

export interface GameInfo {
    id: number;
    creatorName: string;
    isCreator: boolean;
    isPlayer: boolean;
    isPublic: boolean;
    timeStamp: number;
}
