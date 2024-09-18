import { useDispatch } from 'react-redux';
import { chooseHumanPirate } from '/redux/gameSlice';
import { CellPirate } from '/redux/types';

interface CoinPhotoProps {
    pirates: CellPirate[] | undefined;
    pirateSize: number;
    coinCount: number;
}

const CoinPhoto = ({ coinCount, pirates, pirateSize }: CoinPhotoProps) => {
    const dispatch = useDispatch();

    let piratesWithCoins = (pirates && pirates.filter((it) => it.withCoin).length) || 0;
    if (piratesWithCoins === coinCount) return <div />;

    let text = coinCount - piratesWithCoins;
    const coinSize = pirateSize * 0.6 > 20 ? pirateSize * 0.6 : 20;

    const onClick = (girl: CellPirate) => {
        if (girl.withCoin === false) {
            dispatch(chooseHumanPirate({ pirate: girl.id, withCoinAction: true }));
        }
    };

    return pirates && pirates.length == 1 ? (
        <div
            className="coins"
            style={{
                width: coinSize,
                height: coinSize,
                fontSize: Math.ceil(coinSize * 0.6),
                cursor: pirates[0].withCoin === false ? 'pointer' : 'default',
            }}
            onClick={() => onClick(pirates[0])}
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
