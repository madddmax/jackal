import CoinPhoto from './coinPhoto';
import FeaturePhoto from './featurePhoto';
import { hasFreeMoney } from '/game/logic/gameLogic';
import { GameLevel } from '/game/types/gameContent';

interface LevelZeroProps {
    cellSize: number;
    pirateSize: number;
    data: GameLevel;
    onClick?: () => void;
}

/// Для отображение монет и черепов на одноуровневых клетках
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
            {hasFreeMoney(data) && (
                <div
                    key={`cell_level_${data.info.level}_coin`}
                    style={{
                        position: 'absolute',
                        zIndex: 0,
                        width: cellSize,
                        cursor: onClick ? 'pointer' : 'default',
                    }}
                    onClick={onClick}
                >
                    <CoinPhoto level={data} pirateSize={pirateSize} />
                </div>
            )}
            {data.features?.map((feature, idx) => (
                <div
                    key={`cell_level_${data.info.level}_feature_${idx}`}
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
