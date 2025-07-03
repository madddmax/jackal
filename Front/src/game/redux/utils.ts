import { GameLevel } from '../types/gameContent';
import { LevelInfoResponse } from '../types/gameSaga';

export const constructGameLevel = (info: LevelInfoResponse): GameLevel => {
    return {
        info,
        pirates: {
            coins: 0,
            bigCoins: 0,
        },
        hasFreeMoney: function () {
            return this.pirates.coins < this.info.coins || this.pirates.bigCoins < this.info.bigCoins;
        },
    };
};
