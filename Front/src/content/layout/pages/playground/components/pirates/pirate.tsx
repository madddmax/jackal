import cn from 'classnames';

import Image from 'react-bootstrap/Image';
import './pirates.css';

interface PirateProps {
    photo: string;
    isActive: boolean;
    withCoin: boolean | undefined;
    onClick: () => void;
    onCoinClick: () => void;
}

const Pirate = ({ photo, isActive, withCoin, onClick, onCoinClick }: PirateProps) => {
    return (
        <div className="photo-position float-end">
            <Image
                src={`/pictures/${photo}`}
                roundedCircle
                className={cn('photo', {
                    'photo-active': isActive,
                })}
                onClick={onClick}
            />
            {withCoin !== undefined && (
                <>
                    <Image src="/pictures/ruble.png" roundedCircle className={cn('moneta')} onClick={onCoinClick} />
                    {!withCoin && (
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
