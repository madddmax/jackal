import { GameLevel, GamePirate, GameState, ReduxState } from '/redux/types';
import Image from 'react-bootstrap/Image';
import cn from 'classnames';
import './cell.less';
import { useDispatch, useSelector } from 'react-redux';
import { chooseHumanPirate } from '/redux/gameSlice';
import store from '/app/store';
import { girlsMap } from '/app/global';

interface PiratePhotoProps {
    pirate: GamePirate;
    pirateSize: number;
    getMarginTop: (girl: GamePirate) => number;
    getMarginLeft: (girl: GamePirate) => number;
    mapSize: number;
    cellSize: number;
}

const PiratePhoto = ({ pirate, pirateSize, getMarginTop, getMarginLeft, mapSize, cellSize }: PiratePhotoProps) => {
    const level = useSelector<ReduxState, GameLevel>(
        (state) => state.game.fields[pirate.position.y][pirate.position.x].levels[pirate.position.level],
    );
    const dispatch = useDispatch();

    const mapLevel = girlsMap.GetPosition(pirate);

    let allowChoosingPirate =
        (level.piratesWithCoinsCount || 0) === level.coins && (mapLevel?.girls?.length || 0) > level.coins;

    const isCurrentPlayerPirate = (girl: GamePirate): boolean => {
        let gameState = store.getState().game as GameState;
        let team = gameState.teams.find((it) => it.id == gameState.currentHumanTeamId);
        return girl.teamId === team?.id;
    };

    const onTeamPirateClick = (girl: GamePirate, allowChoosing: boolean) => {
        const mapLevel = girlsMap.GetPosition(girl);
        if (!mapLevel || !mapLevel.girls) return;

        let willChangePirate = mapLevel.girls.length > 1 && girl.isActive && allowChoosing;
        if (willChangePirate) {
            girlsMap.ScrollGirls(mapLevel);
        }
        dispatch(
            chooseHumanPirate({
                pirate: willChangePirate ? mapLevel.girls[mapLevel.girls.length - 1] : girl.id,
                withCoinAction: true,
            }),
        );
    };

    const coinSize = pirateSize * 0.3 > 15 ? pirateSize * 0.3 : 15;
    const addSize = (pirateSize - coinSize - 20) / 10;
    const coinPos = pirateSize - coinSize - addSize;
    const isDisabled = pirate.isDrunk || pirate.isInTrap || pirate.isInHole;
    const isCurrentPlayerGirl = isCurrentPlayerPirate(pirate);

    return (
        <div
            key={`pirate_${pirate.id}`}
            className="level"
            style={{
                top: (mapSize - 1 - pirate.position.y) * (cellSize + 1) + getMarginTop(pirate),
                left: pirate.position.x * (cellSize + 1) + getMarginLeft(pirate),
                zIndex: pirate.isActive ? 10 : mapLevel?.girls?.indexOf(pirate.id)! + 3,
                pointerEvents: isCurrentPlayerGirl ? 'auto' : 'none',
            }}
        >
            <Image
                src={`/pictures/${pirate.photo}`}
                roundedCircle={!pirate.isActive}
                className={cn('pirates')}
                style={{
                    border: `${pirateSize >= 50 ? 4 : 3}px ${pirate.backgroundColor || 'transparent'} solid`,
                    // -webkit-filter: grayscale(100%); /* Safari 6.0 - 9.0 */
                    filter: isDisabled ? 'grayscale(100%)' : undefined,
                    width: pirateSize,
                    height: pirateSize,
                    cursor: isDisabled && isCurrentPlayerGirl ? 'default' : 'pointer',
                }}
                onClick={(event) => {
                    if (isCurrentPlayerGirl) {
                        event.stopPropagation();
                        onTeamPirateClick(pirate, allowChoosingPirate);
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
        </div>
    );
};
export default PiratePhoto;
