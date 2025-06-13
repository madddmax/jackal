import { Container, Row } from 'react-bootstrap';
import { useSelector } from 'react-redux';

import NetGameForm from './netGameForm';
import { getNetGame } from '/netgame/redux/lobbySlice';

const NetGameCreate = () => {
    const netGame = useSelector(getNetGame);

    return (
        <Container>
            <Row className="justify-content-center">{netGame && <NetGameForm netGame={netGame} />}</Row>
        </Container>
    );
};

export default NetGameCreate;
