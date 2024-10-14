import { useState } from 'react';
import { Button, Form } from 'react-bootstrap';
import Container from 'react-bootstrap/Container';
import Row from 'react-bootstrap/Row';
import classes from './newgame.module.less';
import { useDispatch, useSelector } from 'react-redux';
import { initMySettings } from '/redux/gameSlice';
import { sagaActions } from '/sagas/constants';
import { useNavigate } from 'react-router-dom';
import { uuidGen } from '/app/global';
import { ReduxState, StorageState } from '/redux/types';
import Lobbies from './lobbies';
import Players from '/content/components/players';
import { PlayersInfo } from '/content/components/types';

const getPlayers = (gamers: string[], count: number): string[] => {
    if (count == 1) return [gamers[0]];
    else if (count == 2) return [gamers[0], gamers[2]];
    else return gamers;
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

    const userSettings = useSelector<ReduxState, StorageState>((state) => state.game.userSettings);

    const [players, setPlayers] = useState<PlayersInfo>({
        count: userSettings.playersCount || 4,
        members: userSettings.players || ['human', 'robot2', 'robot', 'robot2'],
        groups: userSettings.groups,
    });

    const [isStoredMap, setIsStoredMap] = useState(userSettings.mapId != undefined);

    const [randNumber, setRandNumber] = useState(
        convertMapId(userSettings.mapId) || crypto.getRandomValues(new Int32Array(1)),
    );
    const [mapSize, setMapSize] = useState(userSettings.mapSize || 11);

    const newStart = () => {
        navigate('/');
        dispatch(
            initMySettings({
                groups: players.groups,
                mapSize,
                players: players.members,
                playersCount: players.count,
                mapId: isStoredMap ? randNumber[0] : undefined,
            }),
        );
        dispatch({
            type: sagaActions.GAME_START,
            payload: {
                gameName: uuidGen(),
                settings: {
                    players: getPlayers(players.members, players.count),
                    mapId: randNumber[0],
                    mapSize,
                },
            },
        });
    };

    const createLobby = () => {
        // navigate('/');
        dispatch(
            initMySettings({
                groups: players.groups,
                mapSize,
                players: players.members,
                playersCount: players.count,
                mapId: isStoredMap ? randNumber[0] : undefined,
            }),
        );
        dispatch({
            type: sagaActions.LOBBY_CREATE,
            payload: {
                settings: {
                    players: getPlayers(players.members, players.count),
                    mapId: randNumber[0],
                    mapSize,
                },
            },
        });
    };

    const storeMapId = (event: any) => {
        setIsStoredMap(event.target.checked);
        dispatch(
            initMySettings({
                groups: players.groups,
                mapSize,
                players: players.members,
                playersCount: players.count,
                mapId: event.target.checked ? randNumber[0] : undefined,
            }),
        );
    };

    return (
        <Container>
            <Row className="justify-content-center">
                <Form className={classes.newgame} onSubmit={(event) => event.preventDefault()}>
                    {/* <h3>Новая игра</h3> */}
                    <Players players={players} setPlayers={setPlayers} />
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
                    <Form.Group className="mb-3" controlId="formBasicEmail">
                        <Form.Label>Код карты</Form.Label>
                        <Form.Control
                            type="text"
                            name="mapcode"
                            placeholder="Введите код"
                            value={randNumber[0]}
                            onChange={(event) => {
                                setRandNumber(convertMapId(event.target.value)!);
                            }}
                        />
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
                <Lobbies />
            </Row>
        </Container>
    );
}

export default Newgame;
