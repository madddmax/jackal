import Container from 'react-bootstrap/Container';
import Nav from 'react-bootstrap/Nav';
import Navbar from 'react-bootstrap/Navbar';

import { useDispatch } from 'react-redux';
import { sagaActions } from '/redux/saga';

function Header() {
  const dispatch = useDispatch();

  const startGame = () => dispatch({ type: sagaActions.GAME_START, payload: { 
    gameName:	"afc9847e-dce9-497d-bac8-767c3d571b48",
    settings:	'{"players":["human","robot2","robot2","robot2"]}'
  }})

    return (
    //   <Navbar expand="lg" className="bg-body-tertiary">
      <Navbar bg="light" data-bs-theme="light">
        <Container>
          <Navbar.Brand href="#home">React-Jackal</Navbar.Brand>
          <Navbar.Toggle aria-controls="basic-navbar-nav" />
          <Navbar.Collapse id="basic-navbar-nav" className="ms-3">
            <Nav className="me-auto">
              <Nav.Link onClick={startGame} href="#home">Новая игра</Nav.Link>
              <Nav.Link href="#link">Настройки</Nav.Link>
            </Nav>
          </Navbar.Collapse>
        </Container>
      </Navbar>
    );
  }
  
  export default Header;