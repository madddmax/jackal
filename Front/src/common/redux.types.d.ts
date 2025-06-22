import { GameSettings } from '../game/types/hubContracts';
import { CommonState } from './redux/commonSlice.types';

export interface GameInfo {
    id: number;
    creatorName: string;
    isCreator: boolean;
    isPlayer: boolean;
    isPublic: boolean;
    timeStamp: number;
}

export interface NetGameInfoResponse {
    id: number;
    gameId?: number;
    creatorId: number;
    settings: GameSettings;
    viewers: number[];
    users: UserInfo[];
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
    players: [
        {
            id: number;
        },
    ];
    timeStamp: number;
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
