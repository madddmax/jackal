export interface ReduxState {
    game: GameState
}

export interface GameState {
    fields: FieldState[][]
}

export interface FieldState {
    image?: number;
    backColor?: string;
    moveNum?: number;
}

export interface GameStartResponse {
    data: {
        gameName: string;
        mapId: number;
        map: GameMap;
        stat: any;
    }
}

export interface GameTurnResponse {
    data: {
        changes: GameCell[];
        stat: {
            IsHumanPlayer: boolean;
        };
        moves: GameMove[];
    }
}

export interface GameMap {
    Changes: GameCell[];
    Height: number;
    Width: number;
}

interface GameCell {
    BackgroundImageSrc: string;
    BackgroundColor: string;
}

export interface GameMove {
    MoveNum: number;
    From: AcceptableMove;
    To: AcceptableMove;
    WithCoin: boolean;
    WithRespawn: boolean;
}

interface AcceptableMove {
    PirateNum: number;
    Level: number;
    X: number;
    Y: number;
}
