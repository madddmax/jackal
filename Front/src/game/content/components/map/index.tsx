import { Tooltip, TooltipRefProps } from 'react-tooltip';
import Cell from './cell';
import './map.less';
import { useRef } from 'react';
import MapPirates from '../mapPirates';

interface MapProps {
    mapSize: number;
    cellSize: number;
}

function Map({ mapSize, cellSize }: MapProps) {
    const mapWidth = (cellSize + 1) * mapSize - 1;
    const actionsTooltip = useRef<TooltipRefProps>(null);

    return (
        <>
            <div style={{ position: 'relative' }}>
                <div
                    className="map"
                    style={{
                        width: mapWidth,
                        height: mapWidth,
                    }}
                >
                    {Array(mapSize)
                        .fill(0)
                        .map((_, rIndex) => (
                            <div className="map-row" key={`map-row-${mapSize - 1 - rIndex}`}>
                                {Array(mapSize)
                                    .fill(0)
                                    .map((_, cIndex) => (
                                        <div className="map-cell" key={`map-cell-${cIndex}`}>
                                            <Cell col={cIndex} row={mapSize - 1 - rIndex} tooltipRef={actionsTooltip} />
                                        </div>
                                    ))}
                            </div>
                        ))}
                </div>
                <MapPirates mapSize={mapSize} cellSize={cellSize} />
            </div>
            <Tooltip
                ref={actionsTooltip}
                style={{ backgroundColor: 'white', zIndex: 1000 }}
                place="right"
                clickable
                openEvents={{}}
                closeEvents={{ blur: true }}
                globalCloseEvents={{ clickOutsideAnchor: true, escape: true }}
            />
        </>
    );
}

export default Map;
