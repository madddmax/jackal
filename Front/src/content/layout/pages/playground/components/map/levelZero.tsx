import CoinPhoto from './coinPhoto';
import FeaturePhoto from './featurePhoto';
import { GameLevel } from '/redux/types';

interface LevelZeroProps {
    cellSize: number;
    pirateSize: number;
    data: GameLevel;
    onClick?: () => void;
}

const LevelZero = ({ cellSize, pirateSize, data, onClick }: LevelZeroProps) => {
    const addSize = data.features && data.features.length > 3 ? cellSize / 10 : 0;
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
            {data.coins > 0 && (
                <div
                    key={`cell_level_${data.level}_coin`}
                    style={{
                        position: 'absolute',
                        zIndex: 0,
                        width: cellSize,
                        cursor: onClick ? 'pointer' : 'default',
                    }}
                    onClick={onClick}
                >
                    <CoinPhoto
                        coinCount={data.coins}
                        piratesWithCoins={data.piratesWithCoinsCount}
                        freeCoinGirlId={data.freeCoinGirlId}
                        pirateSize={pirateSize}
                    />
                </div>
            )}
            {data.features?.map((feature, idx) => (
                <div
                    key={`cell_level_${data.level}_feature_${idx}`}
                    className="feature"
                    style={{
                        marginTop: getMarginTop(idx),
                        marginLeft: getMarginLeft(idx),
                    }}
                    onClick={onClick}
                >
                    <FeaturePhoto feature={feature} featureSize={pirateSize} hasClick={!!onClick} />
                </div>
            ))}
        </>
    );
};

export default LevelZero;
