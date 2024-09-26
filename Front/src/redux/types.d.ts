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

    userSettings: StorageState;
    fields: FieldState[][];
    pirates?: GamePirate[];
    lastMoves: GameMove[];
    teams: TeamState[];
    currentHumanTeam: TeamState;
    highlight_x: number;
    highlight_y: number;
}

export interface StorageState {
    groups: string[];
    players?: string[];
    playersCount?: number;
    mapSize: number;
    mapId?: number;
}

export interface TeamState {
    id: number;
    activePirate: string;
    lastPirate: string;
    isHumanPlayer: boolean;
    backColor: string;
    group: TeamGroup;
}

export interface TeamGroup {
    id: string;
    photoMaxId: number;
    gannMaxId?: number;
    extension?: string;
}

export interface FieldState {
    image?: string;
    backColor?: string;
    rotate?: number;
    levels: GameLevel[];
    availableMove?: {
        num: number;
        isRespawn: boolean;
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
    pirates?: CellPirate[];
    hasCoins: boolean;
    coin?: {
        text: string;
    };
}

export interface CellPirate {
    id: string;
    withCoin?: boolean;
    isTransparent?: boolean;
    backgroundColor: string;
    photo: string;
    photoId: number;
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
    withCoinAction: bool;
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
    groupId: string;
    photo: string;
    photoId: number;
    type: number;
}

export interface PirateDiff {
    id: string;
    type: number;
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
