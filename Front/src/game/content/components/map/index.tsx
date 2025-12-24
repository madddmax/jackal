import { useRef } from 'react';
import { useSelector } from 'react-redux';
import { Tooltip, TooltipRefProps } from 'react-tooltip';

import MapPirates from '../mapPirates';
import Cell from './cell';
import './map.less';
import { getUserSettings } from '/game/redux/gameSlice';

interface MapProps {
    mapSize: number;
    cellSize: number;
}

function Map({ mapSize, cellSize }: MapProps) {
    const mapWidth = (cellSize + 1) * mapSize - 1;
    const actionsTooltip = useRef<TooltipRefProps>(null);
    const userSettings = useSelector(getUserSettings);
    const chessBarSize = userSettings.hasChessBar || true ? 24 : 0;

    return (
        <>
            <div className="map-container">
                <div
                    className="map"
                    style={{
                        width: mapWidth + chessBarSize,
                        height: mapWidth + chessBarSize,
                    }}
                >
                    {chessBarSize > 0 && (
                        <div className="map-row" key={`map-row-xnote`} style={{ height: chessBarSize }}>
                            <div className="map-cell" key={`map-xnote`}>
                                <div style={{ width: chessBarSize }} />
                            </div>
                            {Array(mapSize)
                                .fill(0)
                                .map((_, cIndex) => (
                                    <div className="map-cell" key={`map-xnote-${cIndex}`}>
                                        <div style={{ width: cellSize }}>{String.fromCharCode(65 + cIndex)}</div>
                                    </div>
                                ))}
                        </div>
                    )}
                    {Array(mapSize)
                        .fill(0)
                        .map((_, rIndex) => (
                            <div className="map-row" key={`map-row-${mapSize - 1 - rIndex}`}>
                                {chessBarSize > 0 && (
                                    <div
                                        className="map-cell"
                                        key={`map-ynote-${rIndex}`}
                                        style={{ width: chessBarSize, verticalAlign: 'middle' }}
                                    >
                                        <div style={{ width: chessBarSize }}>{rIndex}</div>
                                    </div>
                                )}
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
                <MapPirates mapSize={mapSize} cellSize={cellSize} chessBarSize={chessBarSize} />
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
