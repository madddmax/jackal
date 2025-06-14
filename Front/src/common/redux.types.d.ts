import { AuthState } from '../auth/redux/authSlice.types';
import { GameSettings } from '../game/types/hubContracts';
import { CommonState } from './redux/commonSlice.types';

export interface LobbyState {
    gamelist: GameInfo[];
    netgamelist: GameInfo[];
    netGame?: NetGameInfo;
    lobby?: LobbyInfo;
}

export interface GameInfo {
    id: number;
    creator: {
        id: number;
        name: string;
    };
    timeStamp: number;
}

export interface NetGameInfo {
    id: number;
    gameId?: number;
    settings: GameSettings;
    viewers: number[];
}

export interface NetGameListResponse {
    gamesEntries: NetGameEntryResponse[];
}

export interface NetGameEntryResponse {
    gameId: number;
    creator: {
        id: number;
        name: string;
    };
    timeStamp: number;
}

export interface LobbyCreateResponse {
    lobby: LobbyInfo;
}

export interface LobbyJoinRequest {
    lobbyId: string;
}

export interface LobbyJoinResponse {
    lobby: LobbyInfo;
}

export interface LobbyGetRequest {
    lobbyId: string;
}

export interface LobbyGetResponse {
    lobby: LobbyInfo;
}

export interface LobbyInfo {
    id: string;
    ownerId: long;
    lobbyMembers: Record<number, LobbyMember>;
    gameSettings: GameSettings;
    gameId?: string;
}

export interface CheckMapRequest {
    mapId?: number;
    mapSize: number;
    tilesPackName?: string;
}

export interface CheckMapInfo {
    direction: string;
    difficulty: string;
}

export interface LobbyMember {
    userId?: number;
    login: string;
    teamId?: number;
}
