export interface GameStartRequest {
    gameName: string;
    settings: GameSettings;
}

export interface GameSettings {
    players?: GamePlayer[];
    mapId?: number;
    mapSize: number;
    tilesPackName?: string;
    gameMode?: number;
}

export interface GamePlayer {
    id: number;
    type: string;
    position: string;
}
