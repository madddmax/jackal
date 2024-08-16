import Cell from './cell';
import './map.less';

interface MapProps {
    mapSize: number;
    cellSize: number;
}

function Map({ mapSize, cellSize }: MapProps) {
    const mapWidth = (cellSize + 1) * mapSize - 1;

    return (
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
                                    <Cell col={cIndex} row={mapSize - 1 - rIndex} />
                                </div>
                            ))}
                    </div>
                ))}
        </div>
    );
}

export default Map;
