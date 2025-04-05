import { useState } from 'react';
import { Button, Form, Modal } from 'react-bootstrap';
import { useDispatch } from 'react-redux';

import { sagaActions } from '/common/sagas';

const Logon = () => {
    const dispatch = useDispatch();

    const [name, setName] = useState<string>();

    const enterLogin = (login: string) => {
        dispatch({
            type: sagaActions.AUTH_LOGIN,
            payload: {
                login: login,
            },
        });
    };

    return (
        <Modal
            show={true}
            // onHide={handleClose}
            backdrop="static"
            keyboard={false}
        >
            <Modal.Header>
                <Modal.Title>Авторизоваться</Modal.Title>
            </Modal.Header>
            <Modal.Body>
                <Form>
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
                </Form>
            </Modal.Body>
            <Modal.Footer>
                <Button
                    className="float-end"
                    variant="outline-secondary"
                    type="submit"
                    onClick={() => {
                        if (name) enterLogin(name);
                    }}
                >
                    Войти
                </Button>
            </Modal.Footer>
        </Modal>
    );
};

export default Logon;
