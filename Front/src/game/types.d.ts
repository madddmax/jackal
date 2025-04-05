import { GameCell, GameLevel, LevelFeature } from './types/gameCell';
import { GameMove } from './types/gameMove';
import { GameTeam, TeamState } from './types/gameTeam';
import { PiratePosition } from '/common/redux.types';

export { GameCell, GameLevel, LevelFeature, TeamState, GameMove, GameTeam };

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

export interface GameStartResponse extends GameStatistics {
    gameId: number;
    gameMode?: string;
    tilesPackName: string;
    mapId: number;
    map: GameMap;
    teams: GameTeam[];
    pirates: GamePirate[];
    moves: GameMove[];
}

export interface GameTurnResponse extends GameStatistics {
    pirates: GamePirate[];
    pirateChanges: PirateDiff[];
    changes: GameCell[];
    moves: GameMove[];
}

export interface GameMap {
    changes: GameCell[];
    height: number;
    width: number;
}

export interface GameStatistics {
    stats: GameStat;
    teamScores?: GameScore[];
}

export interface GameStat {
    turnNo: number;
    currentTeamId: number;
    isGameOver: boolean;
    gameMessage: string;
}

interface GameScore {
    teamId: number;
    coins: number;
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

export interface GamePirate extends PiratePosition {
    teamId: number;
    withCoin?: boolean;
    isDrunk?: boolean;
    isInTrap?: boolean;
    isInHole?: boolean;
    groupId: string;
    photo: string;
    photoId: number;
    type: string;
    isActive?: boolean;
    backgroundColor?: string;
}

export interface PirateChanges {
    changes: PirateDiff[];
    moves: GameMove[];
}

export interface PirateDiff extends PiratePosition {
    type: string;
    teamId: number;
    isAlive?: boolean;
    isDrunk?: boolean;
    isInTrap?: boolean;
    isInHole?: boolean;
}

export interface PirateMoves {
    moves?: GameMove[];
}

export interface PirateChoose {
    pirate: string;
    withCoinAction: bool;
}
