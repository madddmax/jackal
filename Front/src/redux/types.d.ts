export interface ReduxState {
    game: GameState
}

export interface GameState {
    fields: FieldState[][];
    lastMoves: GameMove[];
    activePirate: number;
}

export interface FieldState {
    image?: string;
    backColor?: string;
    rotate?: number;
    levels?: GameLevel[];
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
    Rotate: number;
    Levels: GameLevel[];
    X: number;
    Y: number;
}

interface GameLevel {
    Level: number;
    hasPirates: boolean;
    Pirate?: GameThing;
    hasCoins: boolean;
    Coin?: GameThing
}

interface GameThing {
    ForeColor?: string;
    BackColor?: string;
    Text: string;
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

export interface PirateMoves {
    pirate: number;
    moves?: GameMove[];
}