export interface ReduxState {
    game: GameState
}

export interface GameState {
    fields: number[][]
}

export interface GameStartResponse {
    data: {
        gameName: string;
        mapId: number;
        map: GameMap;
        stat: any;
    }
}

export interface GameMap {
    Changes: [GameCell];
    Height: number;
    Width: number;
}

interface GameCell {
    BackgroundImageSrc: string;
}