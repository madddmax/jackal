import { useMemo, useState } from 'react';
import { Col, Form, Row } from 'react-bootstrap';
import { useDispatch, useSelector } from 'react-redux';

import Cell from './components/cell';
import MapPirate from './components/mapPirate';
import { Constants, ImagesPacksIds } from '/app/constants';
import { getStepOpacity, setPiratePosition, setStepOpacity } from '/docs/redux/docsSlice';

const TileTypes = [
    'airplane',
    'airplane_used',
    'back',
    'balloon',
    'bengunn',
    'bengunn_used',
    'cannabis',
    'cannabis_used',
    'cannibal',
    'cannon',
    'caramba',
    'chest',
    'croc',
    'desert',
    'empty',
    'forest',
    'fort',
    'four_arrows_diagonal',
    'four_arrows_perpendicular',
    'hole',
    'horse',
    'ice',
    'jungle',
    'lighthouse',
    'missioner',
    'mount',
    'native',
    'one_arrow_diagonal',
    'one_arrow_up',
    'quake',
    'respawn',
    'rum_1',
    'rum_1_used',
    'rum_2',
    'rum_2_used',
    'rum_3',
    'rum_3_used',
    'rumbar',
    'swamp',
    'three_arrows',
    'trap',
    'two_arrows_diagonal',
    'two_arrows_left_right',
];

//const BorderTileTypes = ['ship_1', 'ship_2', 'ship_3', 'ship_4', 'water'];

const OpacityForm = () => {
    const opacity = useSelector(getStepOpacity);
    const dispatch = useDispatch();

    return (
        <div>
            <Form.Label>Прозрачность хода: {opacity}%</Form.Label>
            <Form.Range
                value={opacity}
                min={0}
                max={1}
                step={0.1}
                name="opacity"
                onChange={(e) => dispatch(setStepOpacity(Number(e.target.value)))}
                className="custom-slider"
            />
        </div>
    );
};

const MapRenderer = () => {
    const [mapSize, setMapSize] = useState<number>(9);
    const [imagesPackName, setImagesPackName] = useState<ImagesPacksIds>(ImagesPacksIds.classic);

    const dispatch = useDispatch();

    const switchImagesPackName = (event: { target: { value: string } }) => {
        const val = Object.values(ImagesPacksIds).includes(event.target.value as ImagesPacksIds)
            ? (event.target.value as ImagesPacksIds)
            : ImagesPacksIds.classic;
        setImagesPackName(val);
    };

    const cellSize = 70;
    const mapWidth = (cellSize + 1) * (mapSize + 2) - 1;
    const tiles = useMemo(() => {
        let tt = [...TileTypes];
        const customTilesConfig: { [index: string]: number } = Constants.imagesPackTiles[imagesPackName];
        if (customTilesConfig) {
            Object.entries(customTilesConfig).forEach(([key, value], _) => {
                for (let i = 1; i <= value; i++) {
                    tt.push(`${key}_${i}`);
                }
            });
        }
        while (tt.length < mapSize * mapSize) {
            tt.push(TileTypes[Math.floor(Math.random() * TileTypes.length)]);
        }
        return tt;
    }, [imagesPackName, mapSize]);

    const calcTileType = (row: number, col: number, mapsize: number) => {
        if (row == 0 || row == mapsize + 1) {
            if (col == (mapsize - 1) / 2 + 1) return row == 0 ? 'ship_1' : 'ship_2';
            return 'water';
        }
        if (col == 0 || col == mapsize + 1) {
            if (row == (mapsize - 1) / 2 + 1) return col == 0 ? 'ship_3' : 'ship_4';
            return 'water';
        }
        return tiles[(row - 1) * mapSize + col - 1];
    };

    return (
        <Row className="justify-content-center">
            <Col xs={4}>
                <Form
                    // className={classes.newgame}
                    onSubmit={(event) => event.preventDefault()}
                >
                    <div className="mt-3">
                        <div>
                            <Form.Label>Размер карты: {mapSize}</Form.Label>
                            <Form.Range
                                value={mapSize}
                                min={7}
                                max={13}
                                step={2}
                                name="mapSize"
                                onChange={(e) => setMapSize(Number(e.target.value))}
                                className="custom-slider"
                            />
                        </div>
                    </div>
                    <Form.Group className="mb-3" controlId="images-pack-change-switch">
                        <Form.Label>Оформление карты:</Form.Label>
                        <Form.Select
                            id="images-pack-change-switch"
                            name="imagesPackName"
                            value={imagesPackName}
                            onChange={switchImagesPackName}
                        >
                            {Object.keys(Constants.imagesPacks).map((it) => (
                                <option value={it}>{it}</option>
                            ))}
                        </Form.Select>
                    </Form.Group>
                    <div className="mt-3">
                        <OpacityForm />
                    </div>
                </Form>
            </Col>
            <Col xs={8} className="justify-content-center">
                <div
                    className="map"
                    style={{
                        width: mapWidth,
                        height: mapWidth,
                    }}
                >
                    {Array(mapSize + 2)
                        .fill(0)
                        .map((_, rIndex) => (
                            <div className="map-row" key={`map-row-${mapSize - 1 - rIndex}`}>
                                {Array(mapSize + 2)
                                    .fill(0)
                                    .map((_, cIndex) => (
                                        <div className="map-cell" key={`map-cell-${cIndex}`}>
                                            <Cell
                                                col={cIndex}
                                                row={mapSize - rIndex + 1}
                                                cellSize={cellSize}
                                                tileType={calcTileType(rIndex, cIndex, mapSize)}
                                                imagesPackName={imagesPackName}
                                                onClick={(img: string | undefined) => {
                                                    dispatch(
                                                        setPiratePosition({
                                                            mapSize,
                                                            tiles,
                                                            img,
                                                            position: {
                                                                level: 0,
                                                                x: cIndex,
                                                                y: mapSize - rIndex + 1,
                                                            },
                                                        }),
                                                    );
                                                }}
                                            />
                                        </div>
                                    ))}
                            </div>
                        ))}
                </div>

                <div style={{ position: 'relative' }}>
                    <MapPirate mapSize={mapSize} cellSize={cellSize} />
                </div>
            </Col>
        </Row>
    );
};

export default MapRenderer;
