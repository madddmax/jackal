import { useMemo } from 'react';
import { Button, Form } from 'react-bootstrap';
import Container from 'react-bootstrap/Container';
import Row from 'react-bootstrap/Row';
import './newgame.css';

function Newgame() {
    const human = '/pictures/human.png';
    const robot = '/pictures/robot.png';
    const robot2 = '/pictures/robot2.png';

    const randomNumber = useMemo(
        () => crypto.getRandomValues(new Uint32Array(1)),
        [],
    );

    return (
        <Container>
            <Row className="justify-content-center">
                <Form
                    style={{
                        width: '100%',
                        maxWidth: '500px',
                        textAlign: 'left',
                        backgroundColor: 'white',
                        padding: '15px',
                    }}
                >
                    <h3>Новая игра</h3>
                    <div className="settings">
                        <div
                            className="player"
                            style={{
                                top: '200px',
                                left: '100px',
                                backgroundImage: `url(${human})`,
                            }}
                        ></div>
                        <div
                            className="player"
                            style={{
                                top: '100px',
                                left: '0px',
                                backgroundImage: `url(${robot2})`,
                            }}
                        ></div>
                        <div
                            className="player"
                            style={{
                                top: '0px',
                                left: '100px',
                                backgroundImage: `url(${robot})`,
                            }}
                        ></div>
                        <div
                            className="player"
                            style={{
                                top: '100px',
                                left: '200px',
                                backgroundImage: `url(${robot2})`,
                            }}
                        ></div>
                    </div>
                    <Form.Group className="mb-3" controlId="formBasicEmail">
                        <Form.Label>Код карты</Form.Label>
                        <Form.Control
                            type="text"
                            placeholder="Введите код"
                            value={randomNumber[0]}
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
