import cn from 'classnames';

import Image from 'react-bootstrap/Image';
import './pirates.less';

interface PirateProps {
    photo: string;
    isActive: boolean;
    withCoin: boolean | undefined;
    withRum: boolean | undefined;
    isInTrap: boolean | undefined;
    isInHole: boolean | undefined;
    onClick: () => void;
    onCoinClick: () => void;
}

const Pirate = ({ photo, isActive, withCoin, withRum, isInTrap, isInHole, onClick, onCoinClick }: PirateProps) => {
    const isDisabled = withRum || isInTrap || isInHole;
    return (
        <div className="photo-position float-end">
            <Image
                src={`/pictures/${photo}`}
                roundedCircle
                className={cn('photo', {
                    'photo-active': isActive,
                })}
                style={{
                    filter: isDisabled ? 'grayscale(100%)' : undefined,
                    cursor: isDisabled ? 'default' : 'pointer',
                }}
                onClick={onClick}
            />
            {(withCoin !== undefined || withRum) && (
                <>
                    <Image
                        src={withRum ? '/pictures/rum.png' : '/pictures/ruble.png'}
                        roundedCircle
                        className={cn({
                            moneta: !withRum,
                            rum: withRum,
                        })}
                        onClick={onCoinClick}
                    />
                    {!withCoin && !withRum && (
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
