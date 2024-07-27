import { useSelector } from 'react-redux';
import Cell from './cell';
import './map.less';

import { GameState, ReduxState } from '/redux/types';

function Map() {
    const game = useSelector<ReduxState, GameState>((state) => state.game);

    const mapSize = 51 * game.fields.length - 1;

    return (
        <div
            className="map"
            style={{
                width: mapSize,
                height: mapSize,
            }}
        >
            {game.fields.map((row, rIndex) => (
                <div
                    className="map-row"
                    key={`map-row-${game.fields.length - 1 - rIndex}`}
                >
                    {row.map((_col, cIndex) => (
                        <div className="map-cell" key={`map-cell-${cIndex}`}>
                            <Cell
                                col={cIndex}
                                row={game.fields.length - 1 - rIndex}
                            />
                        </div>
                    ))}
                </div>
            ))}
        </div>
    );
}

export default Map;
