import { LevelInfoResponse } from './gameSaga';

export interface GameLevel {
    info: LevelInfoResponse;
    hasFreeMoney: boolean; // TODO: = pirates.coins < info.coins || info.bigCoins > 0;
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
