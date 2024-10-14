import { Button, Container, Form, Row } from 'react-bootstrap';
import cn from 'classnames';
import classes from './lobbyCard.module.less';
import { useDispatch, useSelector } from 'react-redux';
import { LobbyInfo, ReduxState } from '/redux/types';
import { useNavigate, useParams } from 'react-router-dom';
import { sagaActions } from '/sagas/constants';

const LobbyCard = () => {
    let { id } = useParams();
    const navigate = useNavigate();
    const dispatch = useDispatch();

    const lobby = useSelector<ReduxState, LobbyInfo | undefined>((state) =>
        state.lobby.lobbies.find((it) => it.id === id),
    );

    const joinLobby = () => {
        dispatch({
            type: sagaActions.LOBBY_JOIN,
            payload: {
                lobbyId: id,
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
                            <Form.Group className="mb-3">
                                <Form.Label>Номер:</Form.Label>
                                <Form.Control readOnly type="text" size="sm" value={lobby.id} />
                            </Form.Group>
                            <Form.Group className="mb-3">
                                <Form.Label>Владелец:</Form.Label>
                                <Form.Control readOnly type="text" size="sm" value={lobby.ownerId} />
                            </Form.Group>
                            <Form.Group className="mb-3">
                                <Form.Label>Участники:</Form.Label>
                                {lobby.lobbyMembers && Object.keys(lobby.lobbyMembers).map((key) => <div>{key}</div>)}
                            </Form.Group>
                        </>
                    )}
                    <Button variant="primary" type="submit" onClick={joinLobby}>
                        Присоединиться
                    </Button>
                    <Button className="float-end" variant="outline-primary" type="submit" onClick={exitLobby}>
                        Вернуться
                    </Button>
                </Form>
            </Row>
        </Container>
    );
};

export default LobbyCard;
