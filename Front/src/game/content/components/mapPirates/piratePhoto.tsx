import cn from 'classnames';
import Image from 'react-bootstrap/Image';

import { girlsMap } from '../../../logic/gameLogic';
import './piratePhoto.less';
import store from '/app/store';
import { GameState } from '/game/types';

interface PiratePhotoProps {
    pirate: GamePirate;
    pirateSize: number;
    isCurrentPlayerGirl: boolean;
    onTeamPirateClick: (girl: GamePirate, allowChoosing: boolean) => void;
}

const PiratePhoto = ({ pirate, pirateSize, isCurrentPlayerGirl, onTeamPirateClick }: PiratePhotoProps) => {
    const mapLevel = girlsMap.GetPosition(pirate);

    const coinSize = pirateSize * 0.3 > 15 ? pirateSize * 0.3 : 15;
    const addSize = (pirateSize - coinSize - 20) / 10;
    const coinPos = pirateSize - coinSize - addSize;
    const isDisabled = pirate.isDrunk || pirate.isInTrap || pirate.isInHole;

    let img = '';
    if (pirate.isDrunk) img = '/pictures/rum.png';
    if (pirate.withCoin) img = '/pictures/ruble.png';
    if (pirate.withBigCoin) img = '/pictures/gold_ruble.png';

    return (
        <>
            <Image
                src={`/pictures/${pirate.photo}`}
                roundedCircle={!isCurrentPlayerGirl || !pirate.isActive}
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
                        const gameState = store.getState().game as GameState;
                        const level =
                            gameState.fields[pirate.position.y][pirate.position.x].levels[pirate.position.level];
                        const allowChoosingPirate =
                            level.pirates.coins === level.info.coins &&
                            level.pirates.bigCoins === level.info.bigCoins &&
                            (mapLevel?.girls?.filter((x) => x.teamId === pirate.teamId).length || 0) >
                                level.info.coins + level.info.bigCoins;
                        onTeamPirateClick(pirate, allowChoosingPirate);
                    }
                }}
            />
            {(pirate.withCoin || pirate.withBigCoin || pirate.isDrunk) && (
                <Image
                    src={img}
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
