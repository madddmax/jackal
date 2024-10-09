import { LevelFeature } from '/redux/types';
import Image from 'react-bootstrap/Image';
import cn from 'classnames';
import './cell.less';

interface FeaturePhotoProps {
    feature: LevelFeature;
    featureSize: number;
}

const FeaturePhoto = ({ feature, featureSize }: FeaturePhotoProps) => {
    return (
        <>
            <Image
                src={`/pictures/${feature.photo}`}
                className={cn('features')}
                style={{
                    width: featureSize,
                    height: featureSize,
                    cursor: 'default',
                }}
            />
        </>
    );
};
export default FeaturePhoto;
