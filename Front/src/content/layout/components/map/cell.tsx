import cn from 'classnames'
import { useDispatch, useSelector } from 'react-redux'
import { toggle } from '/redux/gameSlice'

import { ReduxState } from '/redux/types'
import './cell.css'

interface CellProps {
    row: number,
    col: number
}

function Cell(props: CellProps) {
    const {row, col} = props;
    const num = useSelector<ReduxState, number>((state) => state.game.fields[row][col])
    const dispatch = useDispatch()

    return (
        <div className={cn(
            'cell', {
            'cell-water': num == 0,
            'cell-ground': num == 1,
        })}
        onClick={() => dispatch(toggle({col, row}))}></div>
    );
  }
  
  export default Cell;