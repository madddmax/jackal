import { CellPirate } from '/redux/types';

interface CoinPhotoProps {
    pirate: CellPirate | undefined;
    pirateSize: number;
    coinCount: number;
}

const CoinPhoto = ({ coinCount, pirate, pirateSize }: CoinPhotoProps) => {
    if (pirate && pirate.withCoin && coinCount === 1) return <div />;

    let text = pirate && pirate.withCoin ? coinCount - 1 : coinCount;
    const coinSize = pirateSize * 0.6 > 20 ? pirateSize * 0.6 : 20;

    return (
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
