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
                girls: [{ id: it.id, teamId: it.teamId, order: 0 }],
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
                level.girls.push({ id: it.id, teamId: it.teamId, order: ord });
            } else {
                level.girls = [{ id: it.id, teamId: it.teamId, order: 0 }];
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
    CalcTopOffset: function (girl: GamePiratePosition, mapSize: number, cellSize: number, pirateSize: number): number {
        const calcMarginTop = (girl: GamePiratePosition, cellSize: number, pirateSize: number): number => {
            const position = this.GetPosition(girl);
            const levelsCount = position!.levelsCountInCell;
            const level = position!.level;

            const unitSize = cellSize - pirateSize;
            const mul_x_times = cellSize / 50;
            const xAddSize = (mul_x_times - 1) * 5;
            const xUnitSize = cellSize - pirateSize / 2;

            if (levelsCount === 1) {
                const addSize = position!.girls!.length > 3 ? cellSize / 10 : 0;
                const idx = position!.girls!.find((it) => it.id == girl.id)?.order;
                if (idx === 0) return -addSize;
                if (idx === 1) return unitSize + addSize;
                if (idx === 2) return unitSize + addSize;
                if (idx === 3) return unitSize / 2;
                return -addSize;
            } else if (levelsCount === 3) {
                if (level === 2) return xUnitSize * 0.7 + xAddSize;
                else if (level == 1) return xUnitSize * 0.3 + xAddSize;
            } else if (levelsCount === 2) {
                if (level === 1) return xUnitSize * 0.7 + xAddSize;
            } else if (levelsCount === 4) {
                if (level === 3) return xUnitSize * 0.7 + xAddSize;
                else if (level == 2) return xUnitSize * 0.5;
                else if (level == 1) return xUnitSize * 0.2;
            } else if (levelsCount === 5) {
                if (level === 4) return xAddSize;
                else if (level == 3) return xAddSize;
                else if (level == 2) return xUnitSize * 0.3;
                else if (level == 1) return xUnitSize * 0.7 - xAddSize;
                else if (level == 0) return xUnitSize * 0.7;
            }
            return 0;
        };

        return (mapSize - 1 - girl.position.y) * (cellSize + 1) + calcMarginTop(girl, cellSize, pirateSize);
    },
    CalcLeftOffset: function (girl: GamePiratePosition, cellSize: number, pirateSize: number): number {
        const calcMarginLeft = (girl: GamePiratePosition, cellSize: number, pirateSize: number): number => {
            const position = girlsMap.GetPosition(girl);
            const levelsCount = position!.levelsCountInCell;
            const level = position!.level;

            const unitSize = cellSize - pirateSize;
            const mul_x_times = cellSize / 50;
            const xAddSize = (mul_x_times - 1) * 5;
            const xUnitSize = cellSize - pirateSize / 2;

            if (levelsCount === 1) {
                const addSize = position!.girls!.length > 3 ? cellSize / 10 : 0;
                const idx = position!.girls!.find((it) => it.id == girl.id)?.order;
                if (idx === 0) return -addSize;
                if (idx === 1) return unitSize + addSize;
                if (idx === 2) return -addSize;
                if (idx === 3) return unitSize / 2;
                return -addSize;
            } else if (levelsCount === 3) {
                if (level === 2) return xUnitSize * 0.7 + xAddSize;
                else if (level == 1) return xAddSize * 3;
                else if (level == 0) return xUnitSize * 0.7 + xAddSize;
            } else if (levelsCount === 2) {
                if (level === 0) return xUnitSize * 0.7 + xAddSize;
            } else if (levelsCount === 4) {
                if (level === 3) return xUnitSize * 0.7 - xAddSize;
                else if (level == 2) return xAddSize * 2;
                else if (level == 1) return xUnitSize * 0.5 + xAddSize;
                else if (level == 0) return xAddSize * 2;
            } else if (levelsCount === 5) {
                if (level === 4) return xUnitSize * 0.7 + xAddSize;
                else if (level === 3) return xUnitSize * 0.3 + xAddSize;
                else if (level == 2) return xAddSize;
                else if (level == 1) return xAddSize * 3;
                else if (level == 0) return xUnitSize * 0.7;
            }

            return 0;
        };

        return girl.position.x * (cellSize + 1) + calcMarginLeft(girl, cellSize, pirateSize);
    },
    ScrollGirls: function (pos: GirlsLevel) {
        if (pos && pos.girls && pos.girls.length > 1) {
            pos.girls.push(pos.girls.shift()!);
        }
    },
};

export default girlsMap;
