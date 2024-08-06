import { useDispatch, useSelector } from 'react-redux';

import { sagaActions } from '/redux/saga';
import { FieldState, ReduxState, TeamState } from '/redux/types';
import cn from 'classnames';
import './cell.less';

interface CellProps {
    row: number;
    col: number;
}

function Cell(props: CellProps) {
    const { row, col } = props;
    const field = useSelector<ReduxState, FieldState>(
        (state) => state.game.fields[row][col],
    );
    const gamename = useSelector<ReduxState, string | undefined>(
        (state) => state.game.gameName,
    );
    const team = useSelector<ReduxState, TeamState>(
        (state) =>
            state.game.teams.find((it) => it.id === state.game.currentTeamId!)!,
    );

    const dispatch = useDispatch();

    return (
        <>
            <div
                key="main_cell"
                className={cn('cell', {
                    'sell-active': field.highlight && field.highlight === true,
                })}
                style={{
                    backgroundImage: field.image ? `url(${field.image})` : '',
                    backgroundColor: field.backColor || 'transparent',
                    transform:
                        field.rotate && field.rotate > 0
                            ? `rotate(${field.rotate * 90}deg)`
                            : 'none',
                    opacity: field.moveNum !== undefined ? '0.5' : '1',
                }}
                onClick={() => {
                    if (field.moveNum !== undefined) {
                        dispatch({
                            type: sagaActions.GAME_TURN,
                            payload: {
                                gameName: gamename,
                                turnNum: field.moveNum,
                                pirateId: team.activePirate,
                            },
                        });
                    }
                }}
            ></div>
            {field.levels &&
                field.levels.map((it) => (
                    <div
                        key={`cell_level_${it.Level}`}
                        className={`level-${field.levels?.length}${it.Level}`}
                    >
                        {it.Coin && (
                            <div
                                className="coins"
                                style={{
                                    backgroundColor:
                                        it.Coin.BackColor || 'transparent',
                                }}
                            >
                                {it.Coin.Text}
                            </div>
                        )}
                        {it.Pirate && (
                            <div
                                className="pirates"
                                style={{
                                    backgroundColor:
                                        it.Pirate.BackColor || 'transparent',
                                }}
                            >
                                {it.Pirate.Text}
                            </div>
                        )}
                    </div>
                ))}
        </>
    );
}

export default Cell;
