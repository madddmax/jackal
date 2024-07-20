import { useSelector } from 'react-redux';
import cn from 'classnames';
import classes from './controls.module.less';
import { GameStat, GameTeamStat, ReduxState } from '/redux/types';
import { Alert } from 'react-bootstrap';

function Controls() {
    const stat = useSelector<ReduxState, GameStat | undefined>(
        (state) => state.game.stat,
    );
    const gamename = useSelector<ReduxState, string | undefined>(
        (state) => state.game.gameName,
    );
    const gamecode = useSelector<ReduxState, number | undefined>(
        (state) => state.game.mapId,
    );

    const getWinner = (stats: GameStat) => {
        var maxgold = 0;
        let winner = '';
        stats?.Teams.forEach((team: GameTeamStat) => {
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
                    Код игры: <span>{gamename}</span>
                </div>
                <div>
                    Код карты: <span>{gamecode}</span>
                </div>
                <div>
                    Номер хода: <span>{stat && stat.TurnNo}</span>
                </div>
                <div className={cn(classes.teams, 'container')}>
                    {stat &&
                        stat.Teams &&
                        stat.Teams.map((it) => (
                            <div
                                className="row"
                                style={{ backgroundColor: it.backcolor }}
                            >
                                <div className="col-md-8">{it.name}</div>
                                <div className="col-md-4">{it.gold}</div>
                            </div>
                        ))}
                </div>
            </div>

            {stat?.IsGameOver && (
                <Alert variant={'danger'} className="my-2">
                    Игра закончена. Победил {getWinner(stat)}'
                </Alert>
            )}
        </>
    );
}

export default Controls;
