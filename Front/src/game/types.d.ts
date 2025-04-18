export interface GameState {
    stat?: GameStat;
    teamScores?: GameScore[];
    mapForecasts?: string[];
    hasPirateAutoChange: boolean;

    gameSettings: GameStateSettings;
    userSettings: StorageState;
    fields: FieldState[][];
    pirates?: GamePirate[];
    lastMoves: GameMove[];
    teams: TeamState[];
    currentHumanTeamId: number;
    highlight_x: number;
    highlight_y: number;
}

export interface GameStateSettings {
    gameId?: number;
    gameMode?: string;
    mapId?: number;
    mapSize?: number;
    cellSize: number;
    pirateSize: number;
    tilesPackName?: string;

    tilesPackNames: string[];
}

export interface StorageState {
    groups: string[];
    players?: string[];
    playersMode?: number;
    mapSize: number;
    mapId?: number;
    tilesPackName?: string;
    gameSpeed: number;
}

export interface GamePlace {
    cell: FieldState;
    level: GameLevel;
}

export interface FieldState {
    image?: string;
    rotate?: number;
    levels: GameLevel[];
    availableMoves: AvailableMove[];
    highlight?: boolean;
    dark?: boolean;
}

export interface AvailableMove {
    num: number;
    isRespawn: boolean;
    pirateId: string;
    prev?: {
        x: number;
        y: number;
    };
}

export interface PirateMoves {
    moves?: GameMove[];
}

export interface PirateChoose {
    pirate: string;
    withCoinAction: bool;
}
