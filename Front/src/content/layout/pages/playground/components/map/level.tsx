import CoinPhoto from './coinPhoto';
import PiratePhoto from './piratePhoto';
import { FieldState, GameLevel } from '/redux/types';

interface LevelProps {
    cellSize: number;
    pirateSize: number;
    field: FieldState;
    data: GameLevel;
}

const Level = ({ cellSize, pirateSize, field, data }: LevelProps) => {
    const mul_x_times = cellSize / 50;
    const addSize = (mul_x_times - 1) * 10;
    const unitSize = cellSize - pirateSize / 2;

    const getMarginTop = (field: FieldState, level: number) => {
        if (field.levels?.length === 3) {
            if (level === 2) return unitSize * 0.7 + addSize;
            else if (level == 1) return unitSize * 0.3 + addSize;
        } else if (field.levels?.length === 2) {
            if (level === 1) return unitSize * 0.7 + addSize;
        } else if (field.levels?.length === 4) {
            if (level === 3) return unitSize * 0.7 + addSize;
            else if (level == 2) return unitSize * 0.5;
            else if (level == 1) return unitSize * 0.2;
        } else if (field.levels?.length === 5) {
            if (level === 4) return addSize;
            else if (level == 3) return addSize;
            else if (level == 2) return unitSize * 0.3;
            else if (level == 1) return unitSize * 0.7 - addSize;
            else if (level == 0) return unitSize * 0.7;
        }
        return 0;
    };

    const getMarginLeft = (field: FieldState, level: number) => {
        if (field.levels?.length === 3) {
            if (level === 2) return unitSize * 0.7 + addSize;
            else if (level == 1) return addSize * 3;
            else if (level == 0) return unitSize * 0.7 + addSize;
        } else if (field.levels?.length === 2) {
            if (level === 0) return unitSize * 0.7 + addSize;
        } else if (field.levels?.length === 4) {
            if (level === 3) return unitSize * 0.7 - addSize;
            else if (level == 2) return addSize * 2;
            else if (level == 1) return unitSize * 0.5 + addSize;
            else if (level == 0) return addSize * 2;
        } else if (field.levels?.length === 5) {
            if (level === 4) return unitSize * 0.7 + addSize;
            else if (level === 3) return unitSize * 0.3 + addSize;
            else if (level == 2) return addSize;
            else if (level == 1) return addSize * 3;
            else if (level == 0) return unitSize * 0.7;
        }

        return 0;
    };

    const getWidth = (field: FieldState): number | undefined => {
        if (field.levels?.length === 1) {
            return cellSize;
        }
        return undefined;
    };

    let pirate = undefined;
    if (data.pirates) {
        let arr = [...data.pirates];
        arr.sort((a, b) => {
            if (a?.isTransparent) return 1;
            if (b?.isTransparent) return -1;
            return a.photoId - b.photoId;
        });
        pirate = arr[0];
    }

    return (
        <div
            key={`cell_level_${data.level}`}
            className="level"
            style={{
                marginTop: getMarginTop(field, data.level),
                marginLeft: getMarginLeft(field, data.level),
                width: getWidth(field),
            }}
        >
            {data.coin && <CoinPhoto coin={data.coin} pirate={pirate} />}
            {pirate && <PiratePhoto pirate={pirate} pirateSize={pirateSize} />}
        </div>
    );
};

export default Level;
