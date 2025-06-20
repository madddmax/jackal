import { Container, Row } from 'react-bootstrap';
import { useSelector } from 'react-redux';

import { getNetGame } from '../../redux/lobbySlice';
import NetGameForm from './netGameForm';

const NetGameCreate = () => {
    const netGame = useSelector(getNetGame);

    return (
        <Container>
            <Row className="justify-content-center">{netGame && <NetGameForm netGame={netGame} />}</Row>
        </Container>
    );
};

export default NetGameCreate;
