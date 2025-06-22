import { useDispatch } from 'react-redux';

import { chooseHumanPirate } from '../../../redux/gameSlice';

interface CoinPhotoProps {
    piratesWithCoins: number | undefined;
    freeCoinGirlId: string | undefined;
    pirateSize: number;
    coinCount: number;
    bigCoinCount: number;
}

const CoinPhoto = ({ coinCount, bigCoinCount, piratesWithCoins, freeCoinGirlId, pirateSize }: CoinPhotoProps) => {
    const dispatch = useDispatch();

    if (piratesWithCoins === coinCount && bigCoinCount === 0) return <div />;

    const text = bigCoinCount > 0 ? bigCoinCount : coinCount - (piratesWithCoins || 0);
    const coinSize = pirateSize * 0.6;

    const onClick = (girlId: string) => {
        dispatch(chooseHumanPirate({ pirate: girlId, withCoinAction: true }));
    };

    return freeCoinGirlId ? (
        <div
            className={bigCoinCount > 0 ? 'bigCoins' : 'coins'}
            style={{
                width: coinSize,
                height: coinSize,
                fontSize: Math.ceil(coinSize * 0.6),
                cursor: 'pointer',
            }}
            onClick={() => onClick(freeCoinGirlId)}
        >
            {text}
        </div>
    ) : (
        <div
            className={bigCoinCount > 0 ? 'bigCoins' : 'coins'}
            style={{
                width: coinSize,
                height: coinSize,
                fontSize: Math.ceil(coinSize * 0.6),
            }}
        >
            {text}
        </div>
    );
};

export default CoinPhoto;
