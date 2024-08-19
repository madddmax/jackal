import { CellPirate } from '/redux/types';
import Image from 'react-bootstrap/Image';
import cn from 'classnames';
import './cell.less';

interface PiratePhotoProps {
    pirates: CellPirate[];
    pirateSize: number;
}

const PiratePhoto = ({ pirates, pirateSize }: PiratePhotoProps) => {
    let arr = [...pirates];
    arr.sort((a, b) => {
        if (a?.isTransparent) return 1;
        if (b?.isTransparent) return -1;
        return a.photoId - b.photoId;
    });
    const pirate = arr[0];

    return (
        <Image
            src={`/pictures/${pirate.photo}.png`}
            roundedCircle={!pirate.isTransparent}
            className={cn('pirates')}
            style={{
                border: pirate.isTransparent ? 'none' : `2px ${'DarkRed' || 'transparent'} solid`,
                width: pirateSize,
                height: pirateSize,
            }}
        />
    );
};
export default PiratePhoto;
