import { GameLevel, GamePirate, GameState, ReduxState } from '/redux/types';
import Image from 'react-bootstrap/Image';
import cn from 'classnames';
import './cell.less';
import { useDispatch, useSelector } from 'react-redux';
import { chooseHumanPirate } from '/redux/gameSlice';
import { useRef } from 'react';
import store from '/app/store';

interface PiratePhotoProps {
    pirates: GamePirate[];
    pirateSize: number;
}

const PiratePhoto = ({ pirates, pirateSize }: PiratePhotoProps) => {
    const level = useSelector<ReduxState, GameLevel>(
        (state) => state.game.fields[pirates[0].position.y][pirates[0].position.x].levels[pirates[0].position.level],
    );
    const dispatch = useDispatch();
    const topGirl = useRef<number>(0);

    let sorted = [...pirates];
    const coins = Number(level.coin?.text);
    let allowChoosingPirate = (level.piratesWithCoinsCount || 0) === coins && pirates.length > coins;
    sorted.sort((a, b) => {
        if (a.backgroundColor == 'transparent') return 1;
        if (b.backgroundColor == 'transparent') return -1;
        if (a.isActive) return -1;
        if (b.isActive) return 1;
        if (a.photoId > topGirl.current && b.photoId <= topGirl.current) return -1;
        if (a.photoId <= topGirl.current && b.photoId > topGirl.current) return 1;
        return a.photoId - b.photoId;
    });
    let pirate = sorted[0];

    const isCurrentTeam = (girls: GamePirate[]): boolean => {
        let gameState = store.getState().game as GameState;
        let team = gameState.teams.find((it) => it.id == gameState.currentHumanTeamId);
        return girls[0].teamId === team?.id;
    };

    const onTeamPirateClick = (girls: GamePirate[], allowChoosing: boolean) => {
        let willChangePirate = girls.length > 1 && girls[0].isActive && allowChoosing;
        if (willChangePirate) {
            topGirl.current = girls[0].photoId;
        }
        dispatch(
            chooseHumanPirate({
                pirate: willChangePirate ? girls[1].id : girls[0].id,
                withCoinAction: true,
            }),
        );
    };

    const coinSize = pirateSize * 0.3 > 15 ? pirateSize * 0.3 : 15;
    const addSize = (pirateSize - coinSize - 20) / 10;
    const coinPos = pirateSize - coinSize - addSize;
    const isDisabled = pirate.isDrunk || pirate.isInTrap || pirate.isInHole;

    return (
        <>
            <Image
                src={`/pictures/${pirate.photo}`}
                roundedCircle={!pirate.isActive && pirate.backgroundColor != 'transparent'}
                className={cn('pirates')}
                style={{
                    border:
                        pirate.backgroundColor == 'transparent'
                            ? 'none'
                            : `${pirateSize >= 50 ? 4 : 3}px ${pirate.backgroundColor || 'transparent'} solid`,
                    // -webkit-filter: grayscale(100%); /* Safari 6.0 - 9.0 */
                    filter: isDisabled ? 'grayscale(100%)' : undefined,
                    width: pirateSize,
                    height: pirateSize,
                    cursor: isDisabled ? 'default' : 'pointer',
                }}
                onClick={(event) => {
                    if (isCurrentTeam(sorted)) {
                        event.stopPropagation();
                        onTeamPirateClick(sorted, allowChoosingPirate);
                    }
                }}
            />
            {(pirate.withCoin || pirate.isDrunk) && (
                <Image
                    src={pirate.isDrunk ? '/pictures/rum.png' : '/pictures/ruble.png'}
                    roundedCircle
                    className={cn({
                        'cell-moneta': !pirate.isDrunk,
                        'cell-rum': pirate.isDrunk,
                    })}
                    style={{
                        top: pirate.isDrunk ? coinPos / 4 : coinPos,
                        left: pirate.isDrunk ? (coinPos * 2) / 3 : coinPos,
                        width: pirate.isDrunk ? (coinSize * 6) / 3 : coinSize,
                        height: pirate.isDrunk ? (coinSize * 6) / 3 : coinSize,
                    }}
                />
            )}
        </>
    );
};
export default PiratePhoto;
