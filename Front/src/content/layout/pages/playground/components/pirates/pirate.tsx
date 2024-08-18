import cn from 'classnames';

import Image from 'react-bootstrap/Image';
import './pirates.css';

interface PirateProps {
    photo: string;
    isActive: boolean;
    withCoin: boolean | undefined;
    onClick: () => void;
}

const Pirate = ({ photo, isActive, withCoin, onClick }: PirateProps) => {
    return (
        <div className="photo-position float-end">
            <Image
                src={`/pictures/${photo}.png`}
                roundedCircle
                className={cn('photo', {
                    'photo-active': isActive,
                })}
                onClick={() => onClick()}
            />
            {withCoin !== undefined && (
                <>
                    <Image src="/pictures/ruble.png" roundedCircle className={cn('moneta')} />
                    {!withCoin && (
                        <Image src="/pictures/cross-linear-icon.png" roundedCircle className={cn('moneta')} />
                    )}
                </>
            )}
        </div>
    );
};

export default Pirate;
