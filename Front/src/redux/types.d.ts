import { AuthState } from './authSlice.types';
import { CommonState } from './commonSlice.types';
import { GameSettings } from './gameSlice.types';

export interface ReduxState {
    auth: AuthState;
    common: CommonState;
    game: GameState;
    lobby: LobbyState;
}

export interface GameState {
    stat?: GameStat;
    gameName?: string;
    gameMode?: string;
    tilesPackName?: string;
    mapId?: number;
    mapInfo?: string[];
    mapSize?: number;
    cellSize: number;
    pirateSize: number;
    hasPirateAutoChange: boolean;
    tilesPackNames: string[];

    userSettings: StorageState;
    fields: FieldState[][];
    pirates?: GamePirate[];
    lastMoves: GameMove[];
    teams: TeamState[];
    currentHumanTeamId: number;
    highlight_x: number;
    highlight_y: number;
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

export interface LobbyState {
    lobby?: LobbyInfo;
}

export interface TeamState {
    id: number;
    activePirate: string;
    backColor: string;
    group: TeamGroup;
    isHuman: boolean;
}

export interface TeamGroup {
    id: string;
    photoMaxId: number;
    extension?: string;
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

export interface GameStartResponse {
    gameName: string;
    gameMode?: string;
    tilesPackName: string;
    mapId: number;
    map: GameMap;
    pirates: GamePirate[];
    stats: GameStat;
    moves: GameMove[];
}

export interface GameTurnResponse {
    pirates: GamePirate[];
    pirateChanges: PirateDiff[];
    changes: GameCell[];
    stats: GameStat;
    moves: GameMove[];
}

export interface LobbyCreateResponse {
    lobby: LobbyInfo;
}

export interface LobbyJoinRequest {
    lobbyId: string;
}

export interface LobbyJoinResponse {
    lobby: LobbyInfo;
}

export interface LobbyGetRequest {
    lobbyId: string;
}

export interface LobbyGetResponse {
    lobby: LobbyInfo;
}

export interface LobbyInfo {
    id: string;
    ownerId: long;
    lobbyMembers: Record<number, LobbyMember>;
    gameSettings: GameSettings;
    gameId?: string;
}

export interface CheckMapRequest {
    mapId?: number;
    mapSize: number;
    tilesPackName?: string;
}

export interface CheckMapInfo {
    direction: string;
    difficulty: string;
}

export interface LobbyMember {
    userId?: number;
    userName: string;
    teamId?: number;
}

export interface GameStat {
    turnNo: number;
    currentTeamId: number;
    isGameOver: boolean;
    gameMessage: string;
    teams: GameTeamStat[];
}

interface GameTeamStat {
    id: number;
    name: string;
    coins: number;
    isHuman: boolean;
    ship: {
        x: number;
        y: number;
    };
}

export interface GameMap {
    changes: GameCell[];
    height: number;
    width: number;
}

interface GameCell {
    backgroundImageSrc: string;
    rotate: number;
    levels: GameLevel[];
    x: number;
    y: number;
}

interface GameLevel {
    level: number;
    coins: number;
    piratesWithCoinsCount?: number;
    freeCoinGirlId?: string;
    features?: LevelFeature[];
}

export interface LevelFeature {
    backgroundColor: string;
    photo: string;
}

export interface GamePlace {
    cell: FieldState;
    level: GameLevel;
}

export interface GameMove {
    moveNum: number;
    from: AcceptableMove;
    to: AcceptableMove;
    prev?: {
        x: number;
        y: number;
    };
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

export interface PirateDiff extends PiratePosition {
    type: string;
    teamId: number;
    isAlive?: boolean;
    isDrunk?: boolean;
    isInTrap?: boolean;
    isInHole?: boolean;
}

export interface PiratePosition {
    id: string;
    position: {
        level: number;
        x: number;
        y: number;
    };
}
