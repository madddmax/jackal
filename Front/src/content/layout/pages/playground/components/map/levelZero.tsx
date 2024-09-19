import CoinPhoto from './coinPhoto';
import PiratePhoto from './piratePhoto';
import { GameLevel } from '/redux/types';

interface LevelZeroProps {
    cellSize: number;
    pirateSize: number;
    data: GameLevel;
}

const LevelZero = ({ cellSize, pirateSize, data }: LevelZeroProps) => {
    const addSize = data.pirates && data.pirates.length > 3 ? cellSize / 10 : 0;
    const unitSize = cellSize - pirateSize;

    const getMarginTop = (idx: number) => {
        if (idx === 0) return -addSize;
        if (idx === 1) return unitSize + addSize;
        if (idx === 2) return unitSize + addSize;
        if (idx === 3) return unitSize / 2;
        return -addSize;
    };

    const getMarginLeft = (idx: number) => {
        if (idx === 0) return -addSize;
        if (idx === 1) return unitSize + addSize;
        if (idx === 2) return -addSize;
        if (idx === 3) return unitSize / 2;
        return -addSize;
    };

    return (
        <>
            {data.coin && (
                <div
                    key={`cell_level_${data.level}_coin`}
                    className="level"
                    style={{
                        width: cellSize,
                    }}
                >
                    <CoinPhoto coinCount={Number(data.coin.text)} pirates={data.pirates} pirateSize={pirateSize} />
                </div>
            )}
            {data.pirates?.map((pirate, idx) => (
                <div
                    key={`cell_level_${data.level}_pirate_${pirate.id}`}
                    className="level"
                    style={{
                        marginTop: getMarginTop(idx),
                        marginLeft: getMarginLeft(idx),
                    }}
                >
                    <PiratePhoto pirate={pirate} pirateSize={pirateSize} />
                </div>
            ))}
        </>
    );
};

export default LevelZero;
