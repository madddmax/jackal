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
    const isDisabled = pirate.isDrunk || pirate.isInTrap || pirate.isInHole;

    return (
        <>
            <Image
                src={`/pictures/${pirate.photo}`}
                roundedCircle={!pirate.isActive && pirate.backgroundColor != 'transparent'}
                className={cn('pirates')}
                style={{
                    border:
                        pirate.backgroundColor == 'transparent'
                            ? 'none'
                            : `${pirateSize >= 50 ? 4 : 3}px ${pirate.backgroundColor || 'transparent'} solid`,
                    // -webkit-filter: grayscale(100%); /* Safari 6.0 - 9.0 */
                    filter: isDisabled ? 'grayscale(100%)' : undefined,
                    width: pirateSize,
                    height: pirateSize,
                    cursor: isDisabled ? 'default' : 'pointer',
                }}
                onClick={() => onClick(pirate)}
            />
            {(pirate.withCoin || pirate.isDrunk) && (
                <Image
                    src={pirate.isDrunk ? '/pictures/rum.png' : '/pictures/ruble.png'}
                    roundedCircle
                    className={cn({
                        'cell-moneta': !pirate.isDrunk,
                        'cell-rum': pirate.isDrunk,
                    })}
                    style={{
                        top: pirate.isDrunk ? coinPos / 4 : coinPos,
                        left: pirate.isDrunk ? (coinPos * 2) / 3 : coinPos,
                        width: pirate.isDrunk ? (coinSize * 6) / 3 : coinSize,
                        height: pirate.isDrunk ? (coinSize * 6) / 3 : coinSize,
                    }}
                />
            )}
        </>
    );
};
export default PiratePhoto;
