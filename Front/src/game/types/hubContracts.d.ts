import { PlayerInfo, PlayersInfo } from '/app/content/layout/components/types';

export interface makeGameMoveRequestProps {
    gameId: number;
    turnNum: number;
    pirateId: string;
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
