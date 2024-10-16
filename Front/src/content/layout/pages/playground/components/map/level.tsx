import CoinPhoto from './coinPhoto';
import PiratePhoto from './piratePhoto';
import FeaturePhoto from './featurePhoto';
import { FieldState, GameLevel } from '/redux/types';

interface LevelProps {
    cellSize: number;
    pirateSize: number;
    field: FieldState;
    data: GameLevel;
    hasFeaturesOnly?: boolean;
    onClick?: () => void;
}

const Level = ({ cellSize, pirateSize, field, data, hasFeaturesOnly, onClick }: LevelProps) => {
    const mul_x_times = cellSize / 50;
    const addSize = (mul_x_times - 1) * 5;
    const unitSize = cellSize - pirateSize / 2;

    const getMarginTop = (field: FieldState, level: number) => {
        if (field.levels?.length === 3) {
            if (level === 2) return unitSize * 0.7 + addSize;
            else if (level == 1) return unitSize * 0.3 + addSize;
        } else if (field.levels?.length === 2) {
            if (level === 1) return unitSize * 0.7 + addSize;
        } else if (field.levels?.length === 4) {
            if (level === 3) return unitSize * 0.7 + addSize;
            else if (level == 2) return unitSize * 0.5;
            else if (level == 1) return unitSize * 0.2;
        } else if (field.levels?.length === 5) {
            if (level === 4) return addSize;
            else if (level == 3) return addSize;
            else if (level == 2) return unitSize * 0.3;
            else if (level == 1) return unitSize * 0.7 - addSize;
            else if (level == 0) return unitSize * 0.7;
        }
        return 0;
    };

    const getMarginLeft = (field: FieldState, level: number) => {
        if (field.levels?.length === 3) {
            if (level === 2) return unitSize * 0.7 + addSize;
            else if (level == 1) return addSize * 3;
            else if (level == 0) return unitSize * 0.7 + addSize;
        } else if (field.levels?.length === 2) {
            if (level === 0) return unitSize * 0.7 + addSize;
        } else if (field.levels?.length === 4) {
            if (level === 3) return unitSize * 0.7 - addSize;
            else if (level == 2) return addSize * 2;
            else if (level == 1) return unitSize * 0.5 + addSize;
            else if (level == 0) return addSize * 2;
        } else if (field.levels?.length === 5) {
            if (level === 4) return unitSize * 0.7 + addSize;
            else if (level === 3) return unitSize * 0.3 + addSize;
            else if (level == 2) return addSize;
            else if (level == 1) return addSize * 3;
            else if (level == 0) return unitSize * 0.7;
        }

        return 0;
    };

    const getWidth = (field: FieldState): number | undefined => {
        if (field.levels?.length === 1) {
            return cellSize;
        }
        return undefined;
    };

    if (hasFeaturesOnly) {
        return (
            <div
                key={`cell-level-${data.level}-features`}
                className="feature"
                style={{
                    marginTop: getMarginTop(field, data.level),
                    marginLeft: getMarginLeft(field, data.level),
                    width: getWidth(field),
                }}
                onClick={onClick}
            >
                {data.features && data.features.length > 0 && (
                    <FeaturePhoto feature={data.features[0]} featureSize={pirateSize} hasClick={!!onClick} />
                )}
            </div>
        );
    }

    return (
        <div
            key={`cell-level-${data.level}-pirates`}
            className="level"
            style={{
                marginTop: getMarginTop(field, data.level),
                marginLeft: getMarginLeft(field, data.level),
                width: getWidth(field),
            }}
            onClick={onClick}
        >
            {data.coin && (
                <CoinPhoto coinCount={Number(data.coin.text)} pirates={data.pirates} pirateSize={pirateSize} />
            )}
            {data.pirates && data.pirates.length > 0 && (
                <PiratePhoto
                    pirates={data.pirates}
                    pirateSize={pirateSize}
                    coins={(data.coin && Number(data.coin.text)) || 0}
                />
            )}
        </div>
    );
};

export default Level;
