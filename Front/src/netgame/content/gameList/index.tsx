import { Button, Container, ListGroup, Row } from 'react-bootstrap';
import { useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';

import classes from './gamelist.module.less';
import { getGames } from '/netgame/redux/lobbySlice';
import { debugLog, hubConnection } from '/app/global';

const GameList = () => {
    const navigate = useNavigate();
    const list = useSelector(getGames);

    const loadGame = (gameId: number) => {
        navigate('/');

        hubConnection
            .invoke('load', {
                gameId: gameId,
            })
            .catch((err) => {
                debugLog(err);
            });
    };

    return (
        <Container>
            <Row className="justify-content-center">
                <div className={classes.gameList}>
                    <ListGroup>
                        {list &&
                            list.map((it) => {
                                return (
                                    <ListGroup.Item key={`lobby-${it.id}`}>
                                        {it.id}
                                        <Button
                                            className="float-end"
                                            variant="outline-primary"
                                            type="submit"
                                            onClick={() => loadGame(it.id)}
                                        >
                                            Войти
                                        </Button>
                                    </ListGroup.Item>
                                );
                            })}
                    </ListGroup>
                </div>
            </Row>
        </Container>
    );
};

export default GameList;
