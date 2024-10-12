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
import cn from 'classnames';
import { Constants } from '/app/constants';
import Player from './player';
import { ReduxState, StorageState } from '/redux/types';
import Lobbies from './lobbies';

const convertGroups = (initial: string[]) => initial.map((gr) => Constants.groups.findIndex((it) => it.id == gr) || 0);
const deconvertGroups = (groups: number[]) => groups.map((num) => Constants.groups[num].id);

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

    const [playersCount, setPlayersCount] = useState(userSettings.playersCount || 4);
    const [players, setPlayers] = useState(userSettings.players || ['human', 'robot2', 'robot', 'robot2']);
    const [groups, setGroups] = useState(convertGroups(userSettings.groups));
    const [isStoredMap, setIsStoredMap] = useState(userSettings.mapId != undefined);

    const [randNumber, setRandNumber] = useState(
        convertMapId(userSettings.mapId) || crypto.getRandomValues(new Int32Array(1)),
    );
    const [mapSize, setMapSize] = useState(userSettings.mapSize || 11);

    const changePlayer = (pos: number) => {
        const clone = [...players];
        if (clone[pos] === 'human') clone[pos] = 'robot';
        else if (clone[pos] === 'robot') clone[pos] = 'robot2';
        else clone[pos] = 'human';
        setPlayers(clone);
    };

    const changeGroup = (pos: number) => {
        const clone = [...groups];
        let current = clone[pos];
        while (clone.includes(current)) {
            if (current + 1 >= Constants.groups.length) {
                current = 0;
            } else {
                current += 1;
            }
        }
        clone[pos] = current;
        setGroups(clone);
    };

    const newStart = () => {
        navigate('/');
        dispatch(
            initMySettings({
                groups: deconvertGroups(groups),
                mapSize,
                players,
                playersCount,
                mapId: isStoredMap ? randNumber[0] : undefined,
            }),
        );
        dispatch({
            type: sagaActions.GAME_START,
            payload: {
                gameName: uuidGen(),
                settings: {
                    players: getPlayers(players, playersCount),
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
                groups: deconvertGroups(groups),
                mapSize,
                players,
                playersCount,
                mapId: isStoredMap ? randNumber[0] : undefined,
            }),
        );
        dispatch({
            type: sagaActions.LOBBY_CREATE,
            payload: {
                settings: {
                    players: getPlayers(players, playersCount),
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
                groups: deconvertGroups(groups),
                mapSize,
                players,
                playersCount,
                mapId: event.target.checked ? randNumber[0] : undefined,
            }),
        );
    };

    return (
        <Container>
            <Row className="justify-content-center">
                <Form className={classes.newgame} onSubmit={(event) => event.preventDefault()}>
                    {/* <h3>Новая игра</h3> */}
                    <div className={cn(classes.settings, 'mx-auto')}>
                        {players &&
                            players.map((_, index) => {
                                if (playersCount < 4 && (index == 1 || index == 3)) {
                                    return null;
                                }
                                if (playersCount < 2 && index == 2) {
                                    return null;
                                }

                                return (
                                    <Player
                                        key={`player-pos-${index}`}
                                        position={index}
                                        type={players[index]}
                                        group={groups[index]}
                                        changePlayer={() => changePlayer(index)}
                                        changeGroup={() => changeGroup(index)}
                                    />
                                );
                            })}
                        <div
                            className={classes.player}
                            onClick={() =>
                                setPlayersCount((prev) => {
                                    if (prev == 4) return 1;
                                    else if (prev == 1) return 2;
                                    return 4;
                                })
                            }
                            style={{
                                cursor: 'pointer',
                                top: '110px',
                                left: '130px',
                                fontSize: '48px',
                                lineHeight: '48px',
                                textAlign: 'center',
                            }}
                        >
                            {playersCount}
                        </div>
                    </div>
                    <Form.Control type="hidden" name="players" value={players} />
                    <Form.Control type="hidden" name="playersCount" value={playersCount} />
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
