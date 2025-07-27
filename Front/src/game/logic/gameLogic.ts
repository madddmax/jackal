import { GameLevel } from '../types/gameContent';
import { LevelInfoResponse } from '../types/gameSaga';
import girlsMap from './components/girlsMap';

export { girlsMap };
export const hasFreeMoney = (level: GameLevel) => {
    return level.pirates.coins < level.info.coins || level.pirates.bigCoins < level.info.bigCoins;
};
export const constructGameLevel = (info: LevelInfoResponse): GameLevel => ({
    info,
    pirates: {
        coins: 0,
        bigCoins: 0,
    },
});
