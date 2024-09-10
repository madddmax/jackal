import { useState } from 'react';
import { Button, Form } from 'react-bootstrap';
import Container from 'react-bootstrap/Container';
import Row from 'react-bootstrap/Row';
import classes from './newgame.module.less';
import { useDispatch, useSelector } from 'react-redux';
import { initGroups } from '/redux/gameSlice';
import { sagaActions } from '/redux/saga';
import { useNavigate } from 'react-router-dom';
import { uuidGen } from '/app/global';
import cn from 'classnames';
import { Constants } from '/app/constants';
import Player from './player';
import { ReduxState } from '/redux/types';

const convertGroups = (initial: string[]) =>
    initial.map((gr) => {
        return Constants.groups.findIndex((it) => it.id == gr) || 0;
    });

function Newgame() {
    const dispatch = useDispatch();
    const navigate = useNavigate();

    const initialGroups = useSelector<ReduxState, string[]>((state) => state.game.initialGroups);

    const [playersCount, setplayersCount] = useState(4);
    const [players, setPlayers] = useState(['human', 'robot2', 'robot', 'robot2']);
    const [groups, setGroups] = useState(convertGroups(initialGroups));

    const [randNumber, setRandNumber] = useState(() => crypto.getRandomValues(new Int32Array(1)));
    const [mapSize, setMapSize] = useState(11);

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

    const newStart = (event: React.FormEvent) => {
        event.preventDefault();
        navigate('/');
        dispatch(initGroups(groups));
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

    const getPlayers = (gamers: string[], count: number): string[] => {
        if (count == 1) return [gamers[0]];
        else if (count == 2) return [gamers[0], gamers[2]];
        else return gamers;
    };

    return (
        <Container>
            <Row className="justify-content-center">
                <Form className={classes.newgame} onSubmit={(event) => newStart(event)}>
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
                                setplayersCount((prev) => {
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
                                let clone = new Int32Array(1);
                                clone[0] = Number(event.target.value);
                                setRandNumber(clone);
                            }}
                        />
                    </Form.Group>
                    <Button variant="primary" type="submit">
                        Начать
                    </Button>
                </Form>
            </Row>
        </Container>
    );
}

export default Newgame;
