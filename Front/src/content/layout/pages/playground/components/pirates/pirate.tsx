import cn from 'classnames';

import Image from 'react-bootstrap/Image';
import './pirates.css';

interface PirateProps {
    photoId: number;
    isActive: boolean;
    withCoin: boolean | undefined;
    onClick: () => void;
}

const Pirate = ({ photoId, isActive, withCoin, onClick }: PirateProps) => {
    return (
        <div className="photo-position float-end">
            <Image
                src={`/pictures/pirate_${photoId}.png`}
                roundedCircle
                className={cn('photo', {
                    'photo-active': isActive,
                })}
                onClick={() => onClick()}
            />
            {withCoin !== undefined && (
                <>
                    <Image
                        src="/pictures/ruble.png"
                        roundedCircle
                        className={cn('moneta')}
                    />
                    {!withCoin && (
                        <Image
                            src="/pictures/cross-linear-icon.png"
                            roundedCircle
                            className={cn('moneta')}
                        />
                    )}
                </>
            )}
        </div>
    );
};

export default Pirate;
