import cn from 'classnames'
// import classes from './cell.module.less'
import { Fields } from './constants';
import './cell.css'

interface CellProps {
    row: number,
    col: number
}

function Cell(props: CellProps) {

    const {row, col} = props;

    return (
        <div className={cn(
            'cell', {
            'cell-water': Fields[col][row] == 0,
            'cell-ground': Fields[col][row] == 1,
        })}></div>
    );
  }
  
  export default Cell;