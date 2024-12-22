export interface GameStartRequest {
    gameName: string;
    settings: GameSettings;
}

export interface GameSettings {
    players?: GamePlayer[];
    mapId?: number;
    mapSize: number;
    tilesPackName?: string;
    mode?: number;
}

export interface GamePlayer {
    id: number;
    type: string;
    position: string;
}
