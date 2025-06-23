import { LevelInfoResponse } from './gameSaga';

export interface GameLevel {
    info: LevelInfoResponse;
    hasFreeMoney: boolean; // TODO: = piratesWithCoinsCount < info.coins || info.bigCoins > 0;
    piratesWithCoinsCount: number;
    freeCoinGirlId?: string;
    features?: GameLevelFeature[];
}

export interface GameLevelFeature {
    backgroundColor: string;
    photo: string;
}
