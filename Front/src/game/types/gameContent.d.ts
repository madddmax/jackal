import { LevelInfoResponse } from './gameSaga';

export interface GameLevel {
    info: LevelInfoResponse;
    pirates: GameLevelPirates;
    freeCoinGirlId?: string;
    features?: GameLevelFeature[];
}

export interface GameLevelPirates {
    coins: number;
    bigCoins: number;
}

export interface GameLevelFeature {
    backgroundColor: string;
    photo: string;
}
