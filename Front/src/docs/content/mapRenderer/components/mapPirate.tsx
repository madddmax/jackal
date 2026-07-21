import { useSelector } from 'react-redux';

import docsLogic from '../../../logic/docsLogic';
import { getPirate } from '/docs/redux/docsSlice';
import { PiratePhotoMemoized } from '/game/content/components/mapPirates/piratePhotoMemoized';

interface MapPirateProps {
    mapSize: number;
    cellSize: number;
}

const MapPirate = ({ mapSize, cellSize }: MapPirateProps) => {
    const pirate = useSelector(getPirate);

    const pirateSize = cellSize * 0.55;

    return (
        <div
            className="level"
            style={{
                top: docsLogic.CalcTopOffset(pirate, mapSize, cellSize) - mapSize * (cellSize + 1),
                left: docsLogic.CalcLeftOffset(pirate, cellSize),
                zIndex: 10,
                pointerEvents: 'auto',
            }}
        >
            <PiratePhotoMemoized
                pirate={pirate}
                pirateSize={pirateSize}
                isCurrentPlayerGirl
                onTeamPirateClick={() => {}}
            />
        </div>
    );
};

export default MapPirate;
