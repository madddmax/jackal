import cn from 'classnames';
import { useSelector } from 'react-redux';

import { getStepOpacity, hasAvailableMove } from '/docs/redux/docsSlice';
import { DocsState } from '/docs/types/types';
import TurnIcon from '/game/content/components/map/cell/turnIcon';
import { FieldState } from '/game/types';

interface CellPhotoProps {
    row: number;
    col: number;
    cellSize: number;
    field: FieldState;
    onClick: (img: string | undefined) => void;
}

const CellPhoto = ({ row, col, cellSize, field, onClick }: CellPhotoProps) => {
    const hasMove = useSelector<{ docs: DocsState }, boolean>((state) => hasAvailableMove(state, col, row));
    const opacity = useSelector(getStepOpacity);

    return (
        <>
            {hasMove && <TurnIcon moves={field.availableMoves} />}
            <div
                key="main_cell"
                id={`cell_${col}_${row}`}
                className={cn(
                    'cell',
                    { 'cell-dark': field.dark === true },
                    { 'cell-active': field.highlight === true },
                )}
                style={{
                    width: cellSize,
                    height: cellSize,
                    backgroundImage: field.image ? `url(${field.image})` : '',
                    transform: field.rotate && field.rotate > 0 ? `rotate(${field.rotate * 90}deg)` : 'none',
                    opacity: hasMove ? opacity : '1',
                    cursor: hasMove ? 'pointer' : 'default',
                }}
                onClick={() => {
                    onClick(field.image);
                }}
            ></div>
        </>
    );
};

export default CellPhoto;
