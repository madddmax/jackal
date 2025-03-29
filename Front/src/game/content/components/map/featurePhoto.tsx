import cn from 'classnames';
import Image from 'react-bootstrap/Image';

import './cell.less';

interface FeaturePhotoProps {
    feature: LevelFeature;
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
