import { useSelector } from 'react-redux';

import { getGameSettings, getPiratesIds } from '../../../redux/gameSlice';
import MapPirate from './mapPirate';

interface MapPiratesProps {
    mapSize: number;
    cellSize: number;
    chessBarSize: number;
}

const MapPirates = ({ mapSize, cellSize, chessBarSize }: MapPiratesProps) => {
    const piratesIds = useSelector(getPiratesIds);
    const { pirateSize } = useSelector(getGameSettings);

    return (
        <>
            {piratesIds &&
                piratesIds.map((girlId) => (
                    <MapPirate
                        key={`girl_${girlId}`}
                        id={girlId}
                        pirateSize={pirateSize}
                        mapSize={mapSize}
                        cellSize={cellSize}
                        chessBarSize={chessBarSize}
                    />
                ))}
        </>
    );
};

export default MapPirates;
