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
        dispatch(chooseHumanPirate({ pirate: girl.id, withCoinAction: true }));
    };

    const coinSize = pirateSize * 0.3 > 15 ? pirateSize * 0.3 : 15;
    const addSize = (pirateSize - coinSize - 20) / 10;
    const coinPos = pirateSize - coinSize - addSize;

    return (
        <>
            <Image
                src={`/pictures/${pirate.photo}`}
                roundedCircle={!pirate.isTransparent}
                className={cn('pirates')}
                style={{
                    border: pirate.isTransparent
                        ? 'none'
                        : `${pirateSize >= 50 ? 4 : 3}px ${pirate.backgroundColor || 'transparent'} solid`,
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
                        top: coinPos,
                        left: coinPos,
                        width: coinSize,
                        height: coinSize,
                    }}
                />
            )}
        </>
    );
};
export default PiratePhoto;
