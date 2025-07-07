import { GirlsLevel, GirlsPositions } from './gameLogic.types';

// словарь, отслеживающий размещение нескольких пираток на одной клетке
// для корректного их смещения относительно друг друга
export const girlsMap: GirlsPositions = {
    Map: {},
    AddPosition: function (it: GamePiratePosition, levelsCount: number) {
        const cachedId = it.position.y * 1000 + it.position.x * 10 + it.position.level;
        const level = this.Map[cachedId];
        if (!level) {
            this.Map[cachedId] = {
                level: it.position.level,
                levelsCountInCell: levelsCount,
                girls: [it.id],
            };
        } else {
            if (level.girls) {
                level.girls.push(it.id);
            } else {
                level.girls = [it.id];
            }
        }
    },
    RemovePosition: function (it: GamePiratePosition) {
        const cachedId = it.position.y * 1000 + it.position.x * 10 + it.position.level;
        const girlsLevel = this.Map[cachedId];
        if (girlsLevel?.girls != undefined) {
            girlsLevel.girls = girlsLevel.girls.filter((girl) => girl != it.id);
            if (girlsLevel.girls.length == 0) delete this.Map[cachedId];
        }
    },
    GetPosition: function (it: GamePiratePosition): GirlsLevel | undefined {
        const cachedId = it.position.y * 1000 + it.position.x * 10 + it.position.level;
        return this.Map[cachedId];
    },
    ScrollGirls: function (pos: GirlsLevel) {
        if (pos && pos.girls && pos.girls.length > 1) {
            pos.girls.push(pos.girls.shift()!);
        }
    },
};
