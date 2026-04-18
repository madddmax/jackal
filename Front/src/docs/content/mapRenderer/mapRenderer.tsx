import { useState } from 'react';
import { Col, Form, Row } from 'react-bootstrap';

import Cell from './components/cell';
import { Constants, ImagesPacksIds } from '/app/constants';

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
    'empty_1',
    'empty_2',
    'empty_3',
    'empty_4',
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
    'ship_1',
    'ship_2',
    'ship_3',
    'ship_4',
    'swamp',
    'three_arrows',
    'trap',
    'two_arrows_diagonal',
    'two_arrows_left_right',
    'water',
];

const MapRenderer = () => {
    const [mapSize, setMapSize] = useState<number>(9);
    const [imagesPackName, setImagesPackName] = useState<ImagesPacksIds>(ImagesPacksIds.classic);

    const switchImagesPackName = (event: { target: { value: string } }) => {
        const val = Object.values(ImagesPacksIds).includes(event.target.value as ImagesPacksIds)
            ? (event.target.value as ImagesPacksIds)
            : ImagesPacksIds.classic;
        setImagesPackName(val);
    };

    const cellSize = 70;
    const mapWidth = (cellSize + 1) * mapSize - 1;

    const tiles = [...TileTypes];
    while (tiles.length < mapSize * mapSize) {
        tiles.push(TileTypes[Math.floor(Math.random() * TileTypes.length)]);
    }

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
                    {Array(mapSize)
                        .fill(0)
                        .map((_, rIndex) => (
                            <div className="map-row" key={`map-row-${mapSize - 1 - rIndex}`}>
                                {Array(mapSize)
                                    .fill(0)
                                    .map((_, cIndex) => (
                                        <div className="map-cell" key={`map-cell-${cIndex}`}>
                                            <Cell
                                                col={cIndex}
                                                row={mapSize - 1 - rIndex}
                                                cellSize={cellSize}
                                                tileType={tiles[rIndex * mapSize + cIndex]}
                                                imagesPackName={imagesPackName}
                                            />
                                        </div>
                                    ))}
                            </div>
                        ))}
                </div>
            </Col>
        </Row>
    );
};

export default MapRenderer;
