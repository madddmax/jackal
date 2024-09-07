import { CellPirate } from '/redux/types';
import Image from 'react-bootstrap/Image';
import cn from 'classnames';
import './cell.less';
import { useDispatch } from 'react-redux';
import { chooseHumanPirate } from '/redux/gameSlice';

interface PiratePhotoProps {
    pirate: CellPirate;
    pirateSize: number;
}

const PiratePhoto = ({ pirate, pirateSize }: PiratePhotoProps) => {
    const dispatch = useDispatch();

    const onClick = (girl: CellPirate) => {
        dispatch(chooseHumanPirate({ pirate: girl.id }));
    };

    const coinSize = pirateSize * 0.3 > 15 ? pirateSize * 0.3 : 15;

    return (
        <>
            <Image
                src={`/pictures/${pirate.photo}`}
                roundedCircle={!pirate.isTransparent}
                className={cn('pirates')}
                style={{
                    border: pirate.isTransparent ? 'none' : `2px ${'DarkRed' || 'transparent'} solid`,
                    width: pirateSize,
                    height: pirateSize,
                    cursor: 'pointer',
                }}
                onClick={() => onClick(pirate)}
            />
            {pirate.withCoin && (
                <Image
                    src="/pictures/ruble.png"
                    roundedCircle
                    className={cn('cell-moneta')}
                    style={{
                        top: pirateSize * 0.6,
                        left: pirateSize * 0.6,
                        width: coinSize,
                        height: coinSize,
                    }}
                />
            )}
        </>
    );
};
export default PiratePhoto;
