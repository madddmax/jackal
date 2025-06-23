import { useDispatch } from 'react-redux';

import { chooseHumanPirate } from '../../../redux/gameSlice';

interface CoinPhotoCalcs {
    imageClass: string;
    count?: number;
}
interface CoinPhotoProps {
    piratesWithCoins: number;
    freeCoinGirlId: string | undefined;
    pirateSize: number;
    coinCount: number;
    bigCoinCount: number;
}

const CoinPhoto = ({ coinCount, bigCoinCount, piratesWithCoins, freeCoinGirlId, pirateSize }: CoinPhotoProps) => {
    const dispatch = useDispatch();

    const data: CoinPhotoCalcs = {
        imageClass: 'treasure',
    };
    if (bigCoinCount === 0) {
        data.imageClass = 'coins';
        data.count = coinCount - piratesWithCoins;
    } else if (coinCount === piratesWithCoins) {
        data.imageClass = 'bigCoins';
        data.count = bigCoinCount;
    }

    const coinSize = pirateSize * 0.6;

    const onClick = (girlId: string) => {
        dispatch(chooseHumanPirate({ pirate: girlId, withCoinAction: true }));
    };

    return freeCoinGirlId ? (
        <div
            className={data.imageClass}
            style={{
                width: coinSize,
                height: coinSize,
                fontSize: Math.ceil(coinSize * 0.6),
                cursor: 'pointer',
            }}
            onClick={() => onClick(freeCoinGirlId)}
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
