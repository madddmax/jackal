import { ImageGroupsIds, ImagesPacksIds } from '/app/constants';
import { GameLevel } from '/game/types/gameContent';

export interface GameState {
    stat?: GameStat;
    teamScores?: GameScore[];
    mapForecasts?: string[];
    hasPirateAutoChange: boolean;
    includeMovesWithRum: boolean;

    gameSettings: GameStateSettings;
    userSettings: BrowserStorage;
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

export interface BrowserStorage {
    groups: ImageGroupsIds[];
    players?: string[];
    playersMode?: number;
    mapSize: number;
    hasChessBar: boolean;
    mapId?: number;
    imagesPackName: ImagesPacksIds;
    tilesPackName?: string;
    gameSpeed: number;
}

export interface TeamState {
    id: number;
    isCurrentUser?: boolean;
    activePirate: string;
    name: string;
    backColor: string;
    imageGroupId: ImageGroupsIds;
    isHuman: boolean;
}

export interface GamePlace {
    cell: FieldState;
    level: GameLevel;
}

export interface FieldState {
    tileType: string;
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
    isQuake: boolean;
    isLighthouse: boolean;
    pirateId: string;
    prev?: {
        x: number;
        y: number;
    };
}

export interface HighlightHumanMovesActionProps {
    moves?: GameMove[];
}

export interface ChooseHumanPirateActionProps {
    pirate: string;
}

export interface TakeOrPutCoinActionProps {
    pirate: string;
}

export interface ChangeTeamImageGroupActionProps {
    teamId: number;
    imageGroupId: ImageGroupsIds;
}
