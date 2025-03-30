export interface GameSettings {
    players?: GamePlayer[];
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

export interface ScreenSizes {
    width: number;
    height: number;
}

export interface TeamScores {
    teamId: number;
    name: string;
    backColor: string;
    coins: number;
}
