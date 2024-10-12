import { ListGroup } from 'react-bootstrap';
import classes from './newgame.module.less';
import { useSelector } from 'react-redux';
import { LobbyInfo, ReduxState } from '/redux/types';

const Lobbies = () => {
    const list = useSelector<ReduxState, LobbyInfo[]>((state) => state.lobby.lobbies);

    return (
        <div className={classes.lobbyList}>
            <ListGroup>
                {list &&
                    list.map((it) => {
                        return <ListGroup.Item>{it.id}</ListGroup.Item>;
                    })}
            </ListGroup>
        </div>
    );
};

export default Lobbies;
