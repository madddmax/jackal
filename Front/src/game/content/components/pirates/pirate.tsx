import cn from 'classnames';

import Image from 'react-bootstrap/Image';
import './pirates.less';
import { GamePirate } from '../../../../common/redux.types';

interface PirateProps {
    pirate: GamePirate;
    isActive?: boolean;
    onClick: () => void;
    onCoinClick: () => void;
}

const Pirate = ({ pirate, isActive, onClick, onCoinClick }: PirateProps) => {
    const isDisabled = pirate.isDrunk || pirate.isInTrap || pirate.isInHole;
    return (
        <div className="photo-position float-end">
            <Image
                src={`/pictures/${pirate.photo}`}
                roundedCircle
                className={cn('photo', {
                    'photo-active': isActive || false,
                })}
                style={{
                    filter: isDisabled ? 'grayscale(100%)' : undefined,
                    cursor: isDisabled ? 'default' : 'pointer',
                }}
                onClick={onClick}
            />
            {(pirate.withCoin !== undefined || pirate.isDrunk) && (
                <>
                    <Image
                        src={pirate.isDrunk ? '/pictures/rum.png' : '/pictures/ruble.png'}
                        roundedCircle
                        className={cn({
                            moneta: !pirate.isDrunk,
                            rum: pirate.isDrunk,
                        })}
                        onClick={onCoinClick}
                    />
                    {!pirate.withCoin && !pirate.isDrunk && (
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

export default Pirate;
