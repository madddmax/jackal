import { useState } from 'react';
import { Button, Form } from 'react-bootstrap';
import Container from 'react-bootstrap/Container';
import Row from 'react-bootstrap/Row';
import { useDispatch } from 'react-redux';

import classes from './login.module.less';
import { AuthLoginRequest } from '/auth/types/authSaga';
import { sagaActions } from '/common/sagas';

function Login() {
    const dispatch = useDispatch();

    const [name, setName] = useState<string>();

    const enterLogin = (login: string) => {
        dispatch({
            type: sagaActions.AUTH_LOGIN,
            payload: {
                login: login,
            } as AuthLoginRequest,
        });
    };

    return (
        <Container>
            <Row className="justify-content-center">
                <Form className={classes.login} onSubmit={(event) => event.preventDefault()}>
                    <Form.Group className="mb-3" controlId="formLobbyId">
                        <Form.Label>Ваше имя</Form.Label>
                        <Form.Control
                            type="text"
                            name="name"
                            placeholder="Введите ваше имя"
                            value={name}
                            onChange={(event) => setName(event.target.value)}
                        />
                    </Form.Group>
                    <Button
                        className="float-end"
                        variant="outline-primary"
                        type="submit"
                        onClick={() => {
                            if (name) enterLogin(name);
                        }}
                    >
                        Войти
                    </Button>
                </Form>
            </Row>
        </Container>
    );
}

export default Login;
