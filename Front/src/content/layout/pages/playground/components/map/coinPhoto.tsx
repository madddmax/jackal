import { CellPirate, GameThing } from '/redux/types';

interface CoinPhotoProps {
    pirate: CellPirate | undefined;
    coin: GameThing;
}

const CoinPhoto = ({ coin, pirate }: CoinPhotoProps) => {
    if (pirate && pirate.withCoin && coin.text === '1') return <div />;

    let text = pirate && pirate.withCoin ? Number(coin.text) - 1 : coin.text;

    return (
        <div
            className="coins"
            style={{
                backgroundColor: coin.backColor || 'transparent',
            }}
        >
            {text}
        </div>
    );
};

export default CoinPhoto;
