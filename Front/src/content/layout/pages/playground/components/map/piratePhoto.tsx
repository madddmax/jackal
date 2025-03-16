import { GamePirate, GameState } from '/redux/types';
import Image from 'react-bootstrap/Image';
import cn from 'classnames';
import './cell.less';
import { debugLog, girlsMap } from '/app/global';
import store from '/app/store';
import { memo } from 'react';

interface PiratePhotoProps {
    pirate: GamePirate;
    pirateSize: number;
    isCurrentPlayerGirl: boolean;
    onTeamPirateClick: (girl: GamePirate, allowChoosing: boolean) => void;
}

const PiratePhoto = ({ pirate, pirateSize, isCurrentPlayerGirl, onTeamPirateClick }: PiratePhotoProps) => {
    const mapLevel = girlsMap.GetPosition(pirate);

    debugLog('PiratePhoto', pirate.photo);

    const coinSize = pirateSize * 0.3 > 15 ? pirateSize * 0.3 : 15;
    const addSize = (pirateSize - coinSize - 20) / 10;
    const coinPos = pirateSize - coinSize - addSize;
    const isDisabled = pirate.isDrunk || pirate.isInTrap || pirate.isInHole;

    return (
        <>
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
                        const gameState = store.getState().game as GameState;
                        const level =
                            gameState.fields[pirate.position.y][pirate.position.x].levels[pirate.position.level];
                        const allowChoosingPirate =
                            (level.piratesWithCoinsCount || 0) === level.coins &&
                            (mapLevel?.girls?.length || 0) > level.coins;
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
        </>
    );
};
export default memo(PiratePhoto);
