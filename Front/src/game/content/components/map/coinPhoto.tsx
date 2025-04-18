import { useDispatch } from 'react-redux';

import { chooseHumanPirate } from '../../../redux/gameSlice';

interface CoinPhotoProps {
    piratesWithCoins: number | undefined;
    freeCoinGirlId: string | undefined;
    pirateSize: number;
    coinCount: number;
}

const CoinPhoto = ({ coinCount, piratesWithCoins, freeCoinGirlId, pirateSize }: CoinPhotoProps) => {
    const dispatch = useDispatch();

    if (piratesWithCoins === coinCount) return <div />;

    const text = coinCount - (piratesWithCoins || 0);
    const coinSize = pirateSize * 0.6;

    const onClick = (girlId: string) => {
        dispatch(chooseHumanPirate({ pirate: girlId, withCoinAction: true }));
    };

    return freeCoinGirlId ? (
        <div
            className="coins"
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
            className="coins"
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
