import { useSelector } from 'react-redux';
import cn from 'classnames';
import classes from './controls.module.less';
import { GameStat, GameState, GameTeamStat, ReduxState } from '/redux/types';
import { Alert } from 'react-bootstrap';

function Controls() {
    const game = useSelector<ReduxState, GameState | undefined>(
        (state) => state.game,
    );

    const getWinner = (stats: GameStat) => {
        var maxgold = 0;
        let winner = '';
        stats?.teams.forEach((team: GameTeamStat) => {
            if (team.gold > maxgold) {
                maxgold = team.gold;
                winner = team.name;
            } else if (team.gold == maxgold) {
                winner += ' и ' + team.name;
            }
        });
        return winner;
    };

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
                    Размер карты: <span>{game?.mapSize}</span>
                </div>
                <div>
                    Номер хода: <span>{game?.stat?.turnNo}</span>
                </div>
                <div className={cn(classes.teams, 'container')}>
                    {game?.stat?.teams.map((it) => (
                        <div
                            key={`ctrl_${it.id}`}
                            className="row"
                            style={{ backgroundColor: it.backcolor }}
                        >
                            <div className="col-md-8">{it.name}</div>
                            <div className="col-md-4">{it.gold}</div>
                        </div>
                    ))}
                </div>
            </div>

            {game?.stat?.isGameOver && (
                <Alert variant={'danger'} className="my-2">
                    Игра закончена. Победил {getWinner(game.stat)}'
                </Alert>
            )}
        </>
    );
}

export default Controls;
