import { CellPirate, ReduxState, TeamState } from '/redux/types';
import Image from 'react-bootstrap/Image';
import cn from 'classnames';
import './cell.less';
import { useDispatch, useSelector } from 'react-redux';
import { chooseHumanPirate } from '/redux/gameSlice';

interface PiratePhotoProps {
    pirates: CellPirate[];
    pirateSize: number;
}

const PiratePhoto = ({ pirates, pirateSize }: PiratePhotoProps) => {
    const dispatch = useDispatch();
    const team = useSelector<ReduxState, TeamState>((state) => state.game.currentHumanTeam);

    let arr = [...pirates];
    arr.sort((a, b) => {
        if (a?.isTransparent) return 1;
        if (b?.isTransparent) return -1;
        return a.photoId - b.photoId;
    });
    const pirate = arr[0];

    const onClick = (girl: CellPirate) =>
        dispatch(
            chooseHumanPirate({
                pirate: girl.id,
                withCoin: girl.withCoin === undefined || team.activePirate !== girl.id ? girl.withCoin : !girl.withCoin,
            }),
        );

    return (
        <Image
            src={`/pictures/${pirate.photo}.png`}
            roundedCircle={!pirate.isTransparent}
            className={cn('pirates')}
            style={{
                border: pirate.isTransparent ? 'none' : `2px ${'DarkRed' || 'transparent'} solid`,
                width: pirateSize,
                height: pirateSize,
                cursor: 'pointer',
            }}
            onClick={() => onClick(pirate)}
        />
    );
};
export default PiratePhoto;
