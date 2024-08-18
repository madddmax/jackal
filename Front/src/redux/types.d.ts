export interface ReduxState {
    game: GameState;
}

export interface GameState {
    stat?: GameStat;
    gameName?: string;
    mapId?: number;
    mapSize?: number;
    cellSize: number;
    pirateSize: number;

    fields: FieldState[][];
    pirates?: GamePirate[];
    lastMoves: GameMove[];
    teams: TeamState[];
    currentHumanTeam: TeamState;
    highlight_x: number;
    highlight_y: number;
}

export interface TeamState {
    id: number;
    activePirate: string;
    lastPirate: string;
    isHumanPlayer: boolean;
    group: string;
}
export interface FieldState {
    image?: string;
    backColor?: string;
    rotate?: number;
    levels: GameLevel[];
    availableMove?: {
        num: number;
        pirate: string;
    };
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
    pirate?: GameThing;
    pirates?: CellPirate[];
    hasCoins: boolean;
    coin?: GameThing;
}

export interface CellPirate {
    id: string;
    withCoin?: boolean;
    isTransparent?: boolean;
    photo: string;
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
    group: string;
    photo: string;
    photoId: number;
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
