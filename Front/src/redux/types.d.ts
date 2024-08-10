export interface ReduxState {
    game: GameState;
}

export interface GameState {
    stat?: GameStat;
    gameName?: string;
    mapId?: number;
    mapSize?: number;
    cellSize: number;

    fields: FieldState[][];
    pirates?: GamePirate[];
    lastMoves: GameMove[];
    teams: TeamState[];
    currentTeamId?: number;
    highlight_x: number;
    highlight_y: number;
}

export interface TeamState {
    id: number;
    activePirate: string;
    lastPirate: string;
    hasPhotos: boolean;
}
export interface FieldState {
    image?: string;
    backColor?: string;
    rotate?: number;
    levels?: GameLevel[];
    moveNum?: number;
    highlight?: boolean;
}

export interface GameStartResponse {
    gameName: string;
    mapId: number;
    map: GameMap;
    pirates: GamePirate[];
    stat: GameStat;
    moves: GameMove[];
}

export interface GameTurnResponse {
    pirates: GamePirate[];
    pirateChanges: PirateDiff[];
    changes: GameCell[];
    stat: GameStat;
    moves: GameMove[];
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
    Coin?: GameThing;
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
    PirateIds: string[];
    PirateNum: number;
    Level: number;
    X: number;
    Y: number;
}

export interface PirateMoves {
    moves?: GameMove[];
}

export interface PirateChoose {
    pirate: string;
    withCoin?: boolean;
}

export interface PirateChanges {
    changes: PirateDiff[];
    moves: GameMove[];
    isHumanPlayer: boolean;
}

export interface GamePirate {
    Id: string;
    TeamId: number;
    Position: {
        Level: number;
        X: number;
        Y: number;
    };
    WithCoin?: boolean;
    PhotoId?: number;
}

export interface PirateDiff {
    Id: string;
    TeamId: number;
    Position: {
        Level: number;
        X: number;
        Y: number;
    };
    IsAlive?: boolean;
    IsDrunk?: boolean;
    IsInTrap?: boolean;
}
