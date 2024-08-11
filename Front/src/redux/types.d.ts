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
    turnNo: number;
    currentTeamId: number;
    isHumanPlayer: boolean;
    isGameOver: boolean;
    teams: GameTeamStat[];
}

interface GameTeamStat {
    id: number;
    name: string;
    gold: number;
    backcolor: string;
}

export interface GameMap {
    changes: GameCell[];
    height: number;
    width: number;
}

interface GameCell {
    backgroundImageSrc: string;
    backgroundColor: string;
    rotate: number;
    levels: GameLevel[];
    x: number;
    y: number;
}

interface GameLevel {
    level: number;
    hasPirates: boolean;
    pirate?: GameThing;
    hasCoins: boolean;
    coin?: GameThing;
}

interface GameThing {
    foreColor?: string;
    backColor?: string;
    text: string;
}

export interface GameMove {
    moveNum: number;
    from: AcceptableMove;
    to: AcceptableMove;
    withCoin: boolean;
    withRespawn: boolean;
}

interface AcceptableMove {
    pirateIds: string[];
    level: number;
    x: number;
    y: number;
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
    id: string;
    teamId: number;
    position: {
        level: number;
        x: number;
        y: number;
    };
    withCoin?: boolean;
    photoId?: number;
}

export interface PirateDiff {
    id: string;
    teamId: number;
    position: {
        level: number;
        x: number;
        y: number;
    };
    isAlive?: boolean;
    isDrunk?: boolean;
    isInTrap?: boolean;
}
