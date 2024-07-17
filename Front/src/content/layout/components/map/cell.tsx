import { useDispatch, useSelector } from 'react-redux';

import { sagaActions } from '/redux/saga';
import { FieldState, ReduxState } from '/redux/types';
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
    const dispatch = useDispatch();

    return (
        <>
            <div
                className="cell"
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
                                gameName:
                                    'afc9847e-dce9-497d-bac8-767c3d571b48',
                                turnNum: field.moveNum,
                            },
                        });
                    }
                }}
            ></div>
            {field.levels &&
                field.levels.map((it) => (
                    <div className={`level-${field.levels?.length}${it.Level}`}>
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
