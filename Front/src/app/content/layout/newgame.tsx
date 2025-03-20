import { useEffect, useState } from 'react';
import { Button, Form, InputGroup } from 'react-bootstrap';
import Container from 'react-bootstrap/Container';
import Row from 'react-bootstrap/Row';
import classes from './newgame.module.less';
import { useDispatch, useSelector } from 'react-redux';
import { saveMySettings, setMapInfo } from '../../../game/redux/gameSlice';
import { sagaActions } from '/common/sagas';
import { useNavigate } from 'react-router-dom';
import { debugLog, hubConnection, uuidGen } from '/app/global';
import { ReduxState, StorageState } from '../../../common/redux.types';
import Players from './components/players';
import { PlayersInfo } from './components/types';
import { GamePlayer, GameSettings } from '../../../game/redux/gameSlice.types';
import { Constants } from '/app/constants';

const getPlayers = (gamers: string[], mode: number): GamePlayer[] => {
    if (mode == 1) return [{ id: 0, type: gamers[0], position: Constants.positions[0] }];
    else if (mode == 2)
        return [
            { id: 0, type: gamers[0], position: Constants.positions[0] },
            { id: 0, type: gamers[2], position: Constants.positions[2] },
        ];
    else return gamers.map((it, index) => ({ id: 0, type: it, position: Constants.positions[index] }));
};

const convertMapId = (val: string | number | undefined) => {
    if (val === undefined) return undefined;
    let clone = new Int32Array(1);
    clone[0] = typeof val == 'string' ? Number(val) : val;
    return clone;
};

function Newgame() {
    const dispatch = useDispatch();
    const navigate = useNavigate();

    const tilesPackNames = useSelector<ReduxState, string[]>((state) => state.game.tilesPackNames);
    const userSettings = useSelector<ReduxState, StorageState>((state) => state.game.userSettings);
    const mapInfo = useSelector<ReduxState, string[] | undefined>((state) => state.game.mapInfo);

    const [players, setPlayers] = useState<PlayersInfo>({
        mode: userSettings.playersMode || 4,
        members: userSettings.players || ['human', 'robot2', 'robot', 'robot2'],
        groups: userSettings.groups,
    });

    const [isStoredMap, setIsStoredMap] = useState(userSettings.mapId != undefined);

    const [randNumber, setRandNumber] = useState(
        convertMapId(userSettings.mapId) || crypto.getRandomValues(new Int32Array(1)),
    );
    const [mapSize, setMapSize] = useState(userSettings.mapSize || 11);
    const [tilesPackName, setTilesPackName] = useState<string | undefined>(userSettings.tilesPackName); // useState(userSettings.mapSize || 11);

    useEffect(() => {
        dispatch({
            type: sagaActions.CHECK_MAP,
            payload: {
                mapId: randNumber[0],
                mapSize,
                tilesPackName,
            },
        });

        return () => {
            dispatch(setMapInfo());
        };
    }, []);

    const newStart = () => {
        navigate('/');
        saveToLocalStorage(isStoredMap);

        hubConnection
            .invoke('start', {
                gameName: uuidGen(),
                settings: {
                    players: getPlayers(players.members, players.mode),
                    mapId: randNumber[0],
                    mapSize,
                    tilesPackName,
                    gameMode:
                        players.mode == 8
                            ? Constants.gameModeTypes.TwoPlayersInTeam
                            : Constants.gameModeTypes.FreeForAll,
                },
            })
            .catch((err) => {
                debugLog(err);
            });
    };

    const changeMapId = () => {
        let newId = crypto.getRandomValues(new Int32Array(1));
        setRandNumber(newId);
        dispatch({
            type: sagaActions.CHECK_MAP,
            payload: {
                mapId: newId[0],
                mapSize,
                tilesPackName,
            },
        });
    };

    const createLobby = () => {
        dispatch({
            type: sagaActions.LOBBY_CREATE,
            payload: {
                settings: {
                    players: getPlayers(players.members, players.mode),
                    mapId: randNumber[0],
                    mapSize,
                    gameMode:
                        players.mode == 8
                            ? Constants.gameModeTypes.TwoPlayersInTeam
                            : Constants.gameModeTypes.FreeForAll,
                } as GameSettings,
            },
        });
    };

    const storeMapId = (event: any) => {
        setIsStoredMap(event.target.checked);
        saveToLocalStorage(event.target.checked);
    };

    const saveToLocalStorage = (hasStoredMapCode: boolean) => {
        dispatch(
            saveMySettings({
                ...userSettings,
                groups: players.groups,
                mapSize,
                players: players.members,
                playersMode: players.mode,
                mapId: hasStoredMapCode ? randNumber[0] : undefined,
                tilesPackName,
            }),
        );
    };

    return (
        <Container>
            <Row className="justify-content-center">
                <Form className={classes.newgame} onSubmit={(event) => event.preventDefault()}>
                    {/* <h3>Новая игра</h3> */}
                    <Players players={players} setPlayers={setPlayers} mapInfo={mapInfo} />
                    <div className="mt-3">
                        <div>
                            <Form.Label>Размер карты: {mapSize}</Form.Label>
                            <Form.Range
                                value={mapSize}
                                min={5}
                                max={13}
                                step={2}
                                name="mapSize"
                                onChange={(e) => setMapSize(Number(e.target.value))}
                                className="custom-slider"
                            />
                        </div>
                    </div>
                    {tilesPackNames && tilesPackNames.length > 0 && (
                        <Form.Group className="mb-3" controlId="formBasicSelect">
                            <Form.Label>Игровой набор</Form.Label>
                            <Form.Select
                                name="tilesPackName"
                                value={tilesPackName}
                                onChange={(event) => {
                                    setTilesPackName(event.target.value);
                                }}
                            >
                                {tilesPackNames.map((it) => (
                                    <option value={it}>{it}</option>
                                ))}
                            </Form.Select>
                        </Form.Group>
                    )}
                    <Form.Group className="mb-3" controlId="formBasicEmail">
                        <Form.Label>Код карты</Form.Label>
                        <InputGroup>
                            <Form.Control
                                type="text"
                                name="mapcode"
                                placeholder="Введите код"
                                value={randNumber[0]}
                                onChange={(event) => {
                                    setRandNumber(convertMapId(event.target.value)!);
                                }}
                            />
                            <Button variant="outline-secondary" onClick={changeMapId}>
                                Поменять
                            </Button>
                        </InputGroup>
                    </Form.Group>
                    <Form.Group className="mb-3" controlId="formBasicCheckbox">
                        <Form.Check
                            type="checkbox"
                            label="Запоминать код карты"
                            checked={isStoredMap}
                            onChange={storeMapId}
                        />
                    </Form.Group>
                    <Button variant="primary" type="submit" onClick={newStart}>
                        Начать
                    </Button>
                    <Button className="float-end" variant="outline-primary" type="submit" onClick={createLobby}>
                        Создать лобби
                    </Button>
                </Form>
            </Row>
        </Container>
    );
}

export default Newgame;
