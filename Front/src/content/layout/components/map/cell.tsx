import cn from 'classnames'
import { useDispatch, useSelector } from 'react-redux'

import { sagaActions } from '/redux/saga';
import { FieldState, ReduxState } from '/redux/types'
import './cell.css'

interface CellProps {
    row: number,
    col: number
}

function Cell(props: CellProps) {
    const {row, col } = props;
    const field = useSelector<ReduxState, FieldState>((state) => state.game.fields[row][col])
    const dispatch = useDispatch()

    return (
        <div className={cn(
            'cell', {
            'cell-water': field.image == 0,
            'cell-ground': field.image == 1,
            })}
        style={{ 
            backgroundColor: field.backColor || 'transparent',
            opacity: field.moveNum ? '0.5' : '1'
        }}
        // onClick={() => dispatch(toggle({col, row}))}
        onClick={() => { 
            console.log('field', field);
            if (field.moveNum !== undefined) {
                dispatch({ type: sagaActions.GAME_TURN, payload: { 
                    gameName:	"afc9847e-dce9-497d-bac8-767c3d571b48",
                    turnNum:	field.moveNum
                  }});
            }
        }}
        ></div>
    );
  }
  
  export default Cell;