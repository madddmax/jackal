import cn from 'classnames';

import { Constants, ImagesPacksIds } from '/app/constants';
import { FieldState } from '/game/types';

interface CellProps {
    row: number;
    col: number;
    cellSize: number;
    tileType: string;
    imagesPackName: ImagesPacksIds;
}

const Cell = ({ row, col, cellSize, tileType, imagesPackName }: CellProps) => {
    const hasMove = false;

    const field: FieldState = {
        tileType,
        image: Constants.imagesPacks[imagesPackName] + tileType + '.png',
        levels: [
            {
                info: {
                    level: 0,
                    coins: 0,
                    bigCoins: 0,
                },
                pirates: {
                    coins: 0,
                    bigCoins: 0,
                },
            },
        ],
        availableMoves: [],
    };

    return (
        <div
            key="main_cell"
            id={`cell_${col}_${row}`}
            className={cn('cell', { 'cell-dark': field.dark === true }, { 'cell-active': field.highlight === true })}
            style={{
                width: cellSize,
                height: cellSize,
                backgroundImage: field.image ? `url(${field.image})` : '',
                transform: field.rotate && field.rotate > 0 ? `rotate(${field.rotate * 90}deg)` : 'none',
                opacity: hasMove ? '0.5' : '1',
                cursor: hasMove ? 'pointer' : 'default',
            }}
            // onClick={hasMove ? onClick : undefined}
        ></div>
    );
};

export default Cell;
