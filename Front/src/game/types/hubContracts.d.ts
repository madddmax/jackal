import { PlayerInfo, PlayersInfo } from '/common/types/players';

export interface makeGameMoveRequestProps {
    gameId: number;
    moveNum: number;
    pirateId: string;
    turnNumber?: number;
}

export interface GameSettingsFormData {
    players: PlayersInfo;
    allowedGamers: PlayerInfo[];
    mapId?: number;
    mapSize: number;
    tilesPackName?: string;
    isStoredMap: boolean;
}

export interface GameSettings {
    players: GamePlayer[];
    mapId?: number;
    mapSize: number;
    tilesPackName?: string;
    gameMode?: string;
}

export interface GamePlayer {
    userId: number;
    type: string;
    position: string;
}
