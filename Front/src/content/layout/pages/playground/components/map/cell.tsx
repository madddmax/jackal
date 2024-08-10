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
    const cellSize = useSelector<ReduxState, number>(
        (state) => state.game.cellSize,
    );
    const gamename = useSelector<ReduxState, string | undefined>(
        (state) => state.game.gameName,
    );
    const team = useSelector<ReduxState, TeamState>(
        (state) =>
            state.game.teams.find((it) => it.id === state.game.currentTeamId!)!,
    );

    const mul_x_times = cellSize / 50;
    const addSize = (mul_x_times - 1) * 10;
    const getMarginTop = (field: FieldState, level: number) => {
        if (field.levels?.length === 3) {
            if (level === 2) return cellSize * 0.7 + addSize;
            else if (level == 1) return cellSize * 0.3 + addSize;
        } else if (field.levels?.length === 2) {
            if (level === 1) return cellSize * 0.7 + addSize;
        } else if (field.levels?.length === 4) {
            if (level === 3) return cellSize * 0.7 + addSize;
            else if (level == 2) return cellSize * 0.5;
            else if (level == 1) return cellSize * 0.2;
        } else if (field.levels?.length === 5) {
            if (level === 4) return addSize;
            else if (level == 3) return addSize;
            else if (level == 2) return cellSize * 0.3;
            else if (level == 1) return cellSize * 0.7 - addSize;
            else if (level == 0) return cellSize * 0.7;
        }
        return 0;
    };

    const getMarginLeft = (field: FieldState, level: number) => {
        if (field.levels?.length === 3) {
            if (level === 2) return cellSize * 0.7 + addSize;
            else if (level == 1) return addSize * 3;
            else if (level == 0) return cellSize * 0.7 + addSize;
        } else if (field.levels?.length === 2) {
            if (level === 0) return cellSize * 0.7 + addSize;
        } else if (field.levels?.length === 4) {
            if (level === 3) return cellSize * 0.7 - addSize;
            else if (level == 2) return addSize * 2;
            else if (level == 1) return cellSize * 0.5 + addSize;
            else if (level == 0) return addSize * 2;
        } else if (field.levels?.length === 5) {
            if (level === 4) return cellSize * 0.7 + addSize;
            else if (level === 3) return cellSize * 0.3 + addSize;
            else if (level == 2) return addSize;
            else if (level == 1) return addSize * 3;
            else if (level == 0) return cellSize * 0.7;
        }

        return 0;
    };

    const getWidth = (field: FieldState): number | undefined => {
        if (field.levels?.length === 1) {
            return cellSize;
        }
        return undefined;
    };

    const dispatch = useDispatch();

    return (
        <>
            <div
                key="main_cell"
                className={cn('cell', {
                    'sell-active': field.highlight && field.highlight === true,
                })}
                style={{
                    width: cellSize,
                    height: cellSize,
                    backgroundImage: field.image ? `url(${field.image})` : '',
                    backgroundColor: field.backColor || 'transparent',
                    transform:
                        field.rotate && field.rotate > 0
                            ? `rotate(${field.rotate * 90}deg)`
                            : 'none',
                    opacity: field.moveNum !== undefined ? '0.5' : '1',
                    cursor: field.moveNum !== undefined ? 'pointer' : 'default',
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
                        style={{
                            marginTop: getMarginTop(field, it.Level),
                            marginLeft: getMarginLeft(field, it.Level),
                            width: getWidth(field),
                        }}
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
