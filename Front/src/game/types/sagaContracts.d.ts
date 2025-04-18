export interface GameStartResponse {
    gameId: number;
    gameMode?: string;
    tilesPackName: string;
    mapId: number;
    map: GameMapResponse;
    teams: GameTeamResponse[];
    pirates: GamePirate[];
    moves: GameMove[];
    stats: GameStat;
    teamScores?: GameScore[];
}

export interface GameTurnResponse {
    pirates: GamePirate[];
    pirateChanges: PirateDiffResponse[];
    changes: GameCell[];
    moves: GameMove[];
    stats: GameStat;
    teamScores?: GameScore[];
}

export interface GameMapResponse {
    changes: GameCell[];
    height: number;
    width: number;
}

export interface GameTeamResponse {
    id: number;
    name: string;
    coins: number;
    isHuman: boolean;
    ship: {
        x: number;
        y: number;
    };
}

export interface GameStatisticsResponse {
    stats: GameStat;
    teamScores?: GameScore[];
}

export interface GamePirateChangesResponse {
    changes: PirateDiffResponse[];
    moves: GameMove[];
}

interface PirateDiffResponse extends GamePiratePosition {
    type: string;
    teamId: number;
    isAlive?: boolean;
    isDrunk?: boolean;
    isInTrap?: boolean;
    isInHole?: boolean;
}
