import cn from 'classnames';
import Image from 'react-bootstrap/Image';

import './cell.less';
import { GameLevelFeature } from '/game/types/gameContent';

interface FeaturePhotoProps {
    feature: GameLevelFeature;
    featureSize: number;
    hasClick?: boolean;
}

const FeaturePhoto = ({ feature, featureSize, hasClick }: FeaturePhotoProps) => {
    return (
        <>
            <Image
                src={`/pictures/${feature.photo}`}
                className={cn('features')}
                style={{
                    width: featureSize,
                    height: featureSize,
                    cursor: hasClick ? 'pointer' : 'default',
                }}
            />
        </>
    );
};
export default FeaturePhoto;
