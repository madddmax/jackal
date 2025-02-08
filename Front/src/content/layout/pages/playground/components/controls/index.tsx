import { useSelector } from 'react-redux';
import cn from 'classnames';
import classes from './controls.module.less';
import { GameState, ReduxState } from '/redux/types';
import { Alert } from 'react-bootstrap';

function Controls() {
    const game = useSelector<ReduxState, GameState | undefined>((state) => state.game);

    return (
        <>
            <div className={classes.statistic}>
                <div>
                    Код игры: <span>{game?.gameName}</span>
                </div>
                <div>
                    Код карты: <span>{game?.mapId}</span>
                </div>
                <div>
                    Игровой набор: <span>{game?.tilesPackName}</span>
                </div>
                <div>
                    Режим игры: <span>{game?.gameMode == 1 ? '2x2' : 'каждый сам за себя'}</span>
                </div>
                <div>
                    Размер карты: <span>{game?.mapSize}</span>
                </div>
                <div>
                    Номер хода: <span>{game?.stat?.turnNo}</span>
                </div>
                <div className={cn(classes.teams, 'container')}>
                    {game?.stat?.teams.map((it) => (
                        <div key={`ctrl_${it.id}`} className="row" style={{ backgroundColor: it.backcolor }}>
                            <div className="col-md-8">{it.name}</div>
                            <div className="col-md-4">{it.gold}</div>
                        </div>
                    ))}
                </div>
            </div>

            {game?.stat?.gameMessage != undefined && (
                <Alert variant={game?.stat?.isGameOver ? 'success' : 'primary'} className="my-2">
                    {game?.stat?.gameMessage}
                </Alert>
            )}
        </>
    );
}

export default Controls;
