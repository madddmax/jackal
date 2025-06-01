export interface makeGameMoveRequestProps {
    gameId: number;
    turnNum: number;
    pirateId: string;
}

export interface GameSettingsExt extends GameSettings {
    groups: string[];
    members: string[];
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
