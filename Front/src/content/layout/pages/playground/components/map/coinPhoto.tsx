import { useDispatch } from 'react-redux';
import { chooseHumanPirate } from '/redux/gameSlice';
import { CellPirate } from '/redux/types';

interface CoinPhotoProps {
    pirate: CellPirate | undefined;
    pirateSize: number;
    coinCount: number;
}

const CoinPhoto = ({ coinCount, pirate, pirateSize }: CoinPhotoProps) => {
    const dispatch = useDispatch();

    if (pirate && pirate.withCoin && coinCount === 1) return <div />;

    let text = pirate && pirate.withCoin ? coinCount - 1 : coinCount;
    const coinSize = pirateSize * 0.6 > 20 ? pirateSize * 0.6 : 20;

    const onClick = (girl: CellPirate | undefined) => {
        if (girl?.withCoin === false) {
            dispatch(chooseHumanPirate({ pirate: girl.id }));
        }
    };

    return (
        <div
            className="coins"
            style={{
                width: coinSize,
                height: coinSize,
                fontSize: Math.ceil(coinSize * 0.6),
                cursor: pirate?.withCoin === false ? 'pointer' : 'default',
            }}
            onClick={() => onClick(pirate)}
        >
            {text}
        </div>
    );
};

export default CoinPhoto;
