import CellPhoto from './cellPhoto';
import { Constants, ImagesPacksIds } from '/app/constants';
import { getVersionsImage } from '/app/global';
import { FieldState } from '/game/types';

interface CellProps {
    row: number;
    col: number;
    cellSize: number;
    tileType: string;
    imagesPackName: ImagesPacksIds;
    onClick: (img: string | undefined) => void;
}

const Cell = ({ row, col, onClick, cellSize, tileType, imagesPackName }: CellProps) => {
    const customTilesConfig: { [index: string]: number } = Constants.imagesPackTiles[imagesPackName];
    const field: FieldState = {
        tileType,
        image: Constants.imagesPacks[imagesPackName] + getVersionsImage(customTilesConfig, tileType) + '.png',
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

    return <CellPhoto row={row} col={col} cellSize={cellSize} field={field} onClick={onClick} />;
};

export default Cell;
