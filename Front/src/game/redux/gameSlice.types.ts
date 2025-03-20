export interface GameSettings {
    players?: GamePlayer[];
    mapId?: number;
    mapSize: number;
    tilesPackName?: string;
    gameMode?: string;
}

export interface GamePlayer {
    id: number;
    type: string;
    position: string;
}

export interface ScreenSizes {
    width: number;
    height: number;
}
