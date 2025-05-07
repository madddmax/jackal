import dayjs from 'dayjs';
import relativeTime from 'dayjs/plugin/relativeTime';
import { Button, Container, ListGroup, Row } from 'react-bootstrap';
import { useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';

import classes from './gamelist.module.less';
import { debugLog, hubConnection } from '/app/global';
import { getGames } from '/netgame/redux/lobbySlice';

dayjs.extend(relativeTime);

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
                                        <span style={{ paddingRight: 10 }}>{it.id}</span>
                                        {dayjs(it.timeStamp * 1000).fromNow()}
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
