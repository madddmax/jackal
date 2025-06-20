import { useEffect, useState } from 'react';
import { Button, Container, Form, Row } from 'react-bootstrap';
import { useDispatch, useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';

import classes from './lobbyCard.module.less';
import Players from '/app/content/layout/components/players';
import { PlayersInfo } from '/app/content/layout/components/types';
import { getAuth } from '/auth/redux/authSlice';
import { sagaActions } from '/common/sagas';
import { getUserSettings } from '/game/redux/gameSlice';
import { getLobby } from '/netgame/redux/lobbySlice';

const LobbyCard = () => {
    const navigate = useNavigate();
    const dispatch = useDispatch();

    const lobby = useSelector(getLobby);
    const authInfo = useSelector(getAuth);
    const userSettings = useSelector(getUserSettings);

    const [players, setPlayers] = useState<PlayersInfo>({
        mode: 4,
        users: [authInfo.user?.id ?? 0, authInfo.user?.id ?? 0, authInfo.user?.id ?? 0, authInfo.user?.id ?? 0],
        members: ['human', 'human', 'human', 'human'],
        gamers: [{ id: 0, type: 'human', userId: 0 }],
        groups: userSettings.groups,
    });

    useEffect(() => {
        dispatch({ type: sagaActions.LOBBY_DO_POLLING });

        return () => {
            dispatch({ type: sagaActions.LOBBY_STOP_POLLING });
        };
    }, [dispatch]);

    const isInLobby = authInfo.user && !!lobby?.lobbyMembers[authInfo.user.id];

    const joinLobby = () => {
        if (!lobby) return;

        dispatch({
            type: sagaActions.LOBBY_JOIN,
            payload: {
                lobbyId: lobby.id,
            },
        });
    };

    const exitLobby = () => {
        navigate('/newgame');
    };

    return (
        <Container>
            <Row className="justify-content-center">
                <Form className={classes.lobbyCard} onSubmit={(event) => event.preventDefault()}>
                    {lobby && (
                        <>
                            <Form.Group as={Row} className="mb-2">
                                <Form.Label column xs="4">
                                    Номер:
                                </Form.Label>
                                <Form.Label column xs="8">
                                    {lobby.id}
                                </Form.Label>
                            </Form.Group>
                            <Form.Group as={Row} className="mb-2">
                                <Form.Label column xs="4">
                                    Владелец:
                                </Form.Label>
                                <Form.Label column xs="8">
                                    {lobby.ownerId}
                                </Form.Label>
                            </Form.Group>
                            <Form.Group className="mb-3">
                                <Form.Label>Участники:</Form.Label>
                                {lobby.lobbyMembers && Object.keys(lobby.lobbyMembers).map((key) => <div>{key}</div>)}
                            </Form.Group>
                            <Players players={players} gamers={players.gamers} setPlayers={setPlayers} />
                        </>
                    )}
                    {!isInLobby && (
                        <Button variant="primary" type="submit" onClick={joinLobby}>
                            Присоединиться
                        </Button>
                    )}
                    <Button className="float-end" variant="outline-primary" type="submit" onClick={exitLobby}>
                        Вернуться
                    </Button>
                </Form>
            </Row>
        </Container>
    );
};

export default LobbyCard;
