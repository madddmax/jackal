import Container from 'react-bootstrap/Container';
import Nav from 'react-bootstrap/Nav';
import Navbar from 'react-bootstrap/Navbar';

import { useDispatch } from 'react-redux';
import { sagaActions } from '/redux/saga';
import './header.css';
import { Link } from 'react-router-dom';

function Header() {
    const dispatch = useDispatch();

    const quickStart = () =>
        dispatch({
            type: sagaActions.GAME_START,
            payload: {
                gameName: 'afc9847e-dce9-497d-bac8-767c3d571b48',
                settings: '{"players":["human","robot2","robot2","robot2"]}',
            },
        });

    return (
        <Navbar bg="light" data-bs-theme="light" className="header">
            <Container>
                <Navbar.Brand>
                    <Nav.Link as={Link} to="/">
                        React-Jackal
                    </Nav.Link>
                </Navbar.Brand>
                <Navbar.Toggle aria-controls="basic-navbar-nav" />
                <Navbar.Collapse id="basic-navbar-nav" className="ms-3">
                    <Nav className="me-auto">
                        <Nav.Link as={Link} to="/" onClick={quickStart}>
                            Быстрый старт
                        </Nav.Link>
                        <Nav.Link as={Link} to="/newgame">
                            Новая игра
                        </Nav.Link>
                    </Nav>
                </Navbar.Collapse>
            </Container>
        </Navbar>
    );
}

export default Header;
