import { useDispatch } from 'react-redux';

import { chooseHumanPirate } from '../../../redux/gameSlice';
import { GameLevel } from '/game/types/gameContent';

interface CoinPhotoCalcs {
    imageClass: string;
    count?: number;
}
interface CoinPhotoProps {
    level: GameLevel;
    pirateSize: number;
}

const CoinPhoto = ({ level, pirateSize }: CoinPhotoProps) => {
    const dispatch = useDispatch();

    const data: CoinPhotoCalcs = {
        imageClass: 'treasure',
    };
    if (level.info.bigCoins === level.pirates.bigCoins) {
        data.imageClass = 'coins';
        data.count = level.info.coins - level.pirates.coins;
    } else if (level.info.coins === level.pirates.coins) {
        data.imageClass = 'bigCoins';
        data.count = level.info.bigCoins - level.pirates.bigCoins;
    }

    const coinSize = pirateSize * 0.6;

    const onClick = (girlId: string) => {
        dispatch(chooseHumanPirate({ pirate: girlId, withCoinAction: true }));
    };

    return level.freeCoinGirlId ? (
        <div
            className={data.imageClass}
            style={{
                width: coinSize,
                height: coinSize,
                fontSize: Math.ceil(coinSize * 0.6),
                cursor: 'pointer',
            }}
            onClick={() => onClick(level.freeCoinGirlId!)}
        >
            {data.count}
        </div>
    ) : (
        <div
            className={data.imageClass}
            style={{
                width: coinSize,
                height: coinSize,
                fontSize: Math.ceil(coinSize * 0.6),
            }}
        >
            {data.count}
        </div>
    );
};

export default CoinPhoto;
