export interface ReduxState {
    game: GameState
}

export interface GameState {
    stat?: GameStat;
    gameName?: string;
    mapId?: number;

    fields: FieldState[][];
    lastMoves: GameMove[];
    activePirate: number;
    withCoin?: boolean;
    lastPirate: number;
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
        stat: GameStat;
    }
}

export interface GameTurnResponse {
    data: {
        changes: GameCell[];
        stat: GameStat;
        moves: GameMove[];
    }
}

export interface GameMainStat {
    gameName: string;
    mapId: number;
}

export interface GameStat {
    TurnNo: number;
    CurrentTeamId: number;
    IsHumanPlayer: boolean;
    IsGameOver: boolean;
    Teams: GameTeamStat[];
}

interface GameTeamStat {
    id: number;
    name: string;
    gold: number;
    backcolor: string;
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
    pirate?: number;
    withCoin?: boolean;
    moves?: GameMove[];
}