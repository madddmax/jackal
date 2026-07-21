import { DocsPiratePosition } from '../types/types';

const docsLogic = {
    CalcTopOffset: function (girl: GamePiratePosition, mapSize: number, cellSize: number): number {
        return (mapSize - 1 - girl.position.y) * (cellSize + 1);
    },
    CalcLeftOffset: function (girl: GamePiratePosition, cellSize: number): number {
        return girl.position.x * (cellSize + 1);
    },
    CalcAvailableMoves: (pos: DocsPiratePosition): number[][] => {
        if (pos.img && pos.img.indexOf('airplane') >= 0) {
            const ret: number[][] = [];
            Array(pos.mapSize)
                .fill(0)
                .forEach((_, rIndex) => {
                    Array(pos.mapSize)
                        .fill(0)
                        .forEach((_, cIndex) => {
                            ret.push([cIndex + 1, rIndex + 1]);
                        });
                });
            return ret;
        }
        return [
            [pos.position.x - 1, pos.position.y - 1],
            [pos.position.x - 1, pos.position.y],
            [pos.position.x - 1, pos.position.y + 1],
            [pos.position.x, pos.position.y + 1],
            [pos.position.x + 1, pos.position.y + 1],
            [pos.position.x + 1, pos.position.y],
            [pos.position.x + 1, pos.position.y - 1],
            [pos.position.x, pos.position.y - 1],
        ];
    },
};

export default docsLogic;
