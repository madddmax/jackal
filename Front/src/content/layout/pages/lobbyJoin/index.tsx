import { useState } from 'react';
import { Button, Form } from 'react-bootstrap';
import Container from 'react-bootstrap/Container';
import Row from 'react-bootstrap/Row';
import classes from './lobbyJoin.module.less';
import { useDispatch } from 'react-redux';
import { sagaActions } from '/sagas/constants';

function LobbyJoin() {
    const dispatch = useDispatch();

    const [lobbyId, setLobbyId] = useState<string>();

    const joinLobby = (id: string) => {
        dispatch({
            type: sagaActions.LOBBY_JOIN,
            payload: {
                lobbyId: id,
            },
        });
    };

    return (
        <Container>
            <Row className="justify-content-center">
                <Form className={classes.lobbyJoin} onSubmit={(event) => event.preventDefault()}>
                    <Form.Group className="mb-3" controlId="formLobbyId">
                        <Form.Label>Номер лобби</Form.Label>
                        <Form.Control
                            type="text"
                            name="lobbyId"
                            placeholder="Введите номер лобби"
                            value={lobbyId}
                            onChange={(event) => setLobbyId(event.target.value)}
                        />
                    </Form.Group>
                    <Button
                        className="float-end"
                        variant="outline-primary"
                        type="submit"
                        onClick={() => {
                            if (lobbyId) joinLobby(lobbyId);
                        }}
                    >
                        Войти
                    </Button>
                </Form>
            </Row>
        </Container>
    );
}

export default LobbyJoin;
