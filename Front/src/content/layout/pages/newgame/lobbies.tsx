import { Button, ListGroup } from 'react-bootstrap';
import classes from './newgame.module.less';
import { useSelector } from 'react-redux';
import { LobbyInfo, ReduxState } from '/redux/types';
import { useNavigate } from 'react-router-dom';

const Lobbies = () => {
    const navigate = useNavigate();
    const list = useSelector<ReduxState, LobbyInfo[]>((state) => state.lobby.lobbies);

    const enterLobby = (id: string) => {
        navigate(`/lobby/${id}`);
    };

    return (
        <div className={classes.lobbyList}>
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
                                    onClick={() => enterLobby(it.id)}
                                >
                                    Войти
                                </Button>
                            </ListGroup.Item>
                        );
                    })}
            </ListGroup>
        </div>
    );
};

export default Lobbies;
