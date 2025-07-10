import cn from 'classnames';
import Image from 'react-bootstrap/Image';

import './pirateIcon.less';

interface PirateProps {
    pirate: GamePirate;
    onClick: () => void;
    onCoinClick: () => void;
}

const PirateIcon = ({ pirate, onClick, onCoinClick }: PirateProps) => {
    const isDisabled = pirate.isDrunk || pirate.isInTrap || pirate.isInHole;
    let coinImg = '/pictures/ruble.png';
    if (pirate.isDrunk) coinImg = '/pictures/rum.png';
    if (pirate.withBigCoin) coinImg = '/pictures/gold_ruble.png';

    return (
        <div className="photo-position float-end">
            <Image
                src={`/pictures/${pirate.photo}`}
                roundedCircle
                className={cn('photo', {
                    'photo-active': pirate.isActive || false,
                })}
                style={{
                    filter: isDisabled ? 'grayscale(100%)' : undefined,
                    cursor: isDisabled ? 'default' : 'pointer',
                }}
                onClick={onClick}
            />
            {(pirate.withCoin !== undefined || pirate.withBigCoin !== undefined || pirate.isDrunk) && (
                <>
                    <Image
                        src={coinImg}
                        roundedCircle
                        className={cn({
                            moneta: !pirate.isDrunk,
                            rum: pirate.isDrunk,
                        })}
                        onClick={onCoinClick}
                    />
                    {!pirate.withCoin && !pirate.withBigCoin && !pirate.isDrunk && (
                        <Image
                            src="/pictures/cross-linear-icon.png"
                            roundedCircle
                            className={cn('moneta')}
                            onClick={onCoinClick}
                        />
                    )}
                </>
            )}
        </div>
    );
};

export default PirateIcon;
