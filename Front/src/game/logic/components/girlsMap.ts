import { GirlsLevel, GirlsPositions } from '../gameLogic.types';

// словарь, отслеживающий размещение нескольких пираток на одной клетке
// для корректного их смещения относительно друг друга
const girlsMap: GirlsPositions = {
    Map: {},
    AddPosition: function (it: GamePiratePosition, levelsCount: number) {
        const cachedId = it.position.y * 1000 + it.position.x * 10 + it.position.level;
        const level = this.Map[cachedId];
        if (!level) {
            this.Map[cachedId] = {
                level: it.position.level,
                levelsCountInCell: levelsCount,
                girls: [{ id: it.id, order: 0 }],
            };
        } else {
            if (level.girls) {
                let ords = level.girls.map((x) => x.order).sort();
                let ord = level.girls.length;
                for (let i = 0; i < level.girls.length; ++i) {
                    if (i != ords[i]) {
                        ord = i;
                        break;
                    }
                }
                level.girls.push({ id: it.id, order: ord });
            } else {
                level.girls = [{ id: it.id, order: 0 }];
            }
        }
    },
    RemovePosition: function (it: GamePiratePosition) {
        const cachedId = it.position.y * 1000 + it.position.x * 10 + it.position.level;
        const girlsLevel = this.Map[cachedId];
        if (girlsLevel?.girls != undefined) {
            girlsLevel.girls = girlsLevel.girls.filter((girl) => girl.id != it.id);
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

export default girlsMap;
