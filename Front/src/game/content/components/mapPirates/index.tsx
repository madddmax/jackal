import { useSelector } from 'react-redux';

import { getGameSettings, getPiratesIds } from '../../../redux/gameSlice';
import MapPirate from './mapPirate';
import { girlsMap } from '/app/global';
import { PiratePosition } from '/common/redux.types';

interface MapPiratesProps {
    mapSize: number;
    cellSize: number;
}

const MapPirates = ({ mapSize, cellSize }: MapPiratesProps) => {
    const piratesIds = useSelector(getPiratesIds);
    const { pirateSize } = useSelector(getGameSettings);

    const unitSize = cellSize - pirateSize;

    const mul_x_times = cellSize / 50;
    const xAddSize = (mul_x_times - 1) * 5;
    const xUnitSize = cellSize - pirateSize / 2;

    const getMarginTop = (girl: PiratePosition): number => {
        const position = girlsMap.GetPosition(girl);
        const levelsCount = position!.levelsCountInCell;
        const level = position!.level;

        if (levelsCount === 1) {
            const addSize = position!.girls!.length > 3 ? cellSize / 10 : 0;
            const idx = position!.girls!.findIndex((it) => it == girl.id);
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

    const getMarginLeft = (girl: PiratePosition): number => {
        const position = girlsMap.GetPosition(girl);
        const levelsCount = position!.levelsCountInCell;
        const level = position!.level;

        if (levelsCount === 1) {
            const addSize = position!.girls!.length > 3 ? cellSize / 10 : 0;
            const idx = position!.girls!.findIndex((it) => it == girl.id);
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

    return (
        <>
            {piratesIds &&
                piratesIds.map((girlId) => (
                    <MapPirate
                        key={`girl_${girlId}`}
                        id={girlId}
                        pirateSize={pirateSize}
                        getMarginTop={getMarginTop}
                        getMarginLeft={getMarginLeft}
                        mapSize={mapSize}
                        cellSize={cellSize}
                    />
                ))}
        </>
    );
};

export default MapPirates;
