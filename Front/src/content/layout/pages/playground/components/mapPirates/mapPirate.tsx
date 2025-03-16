import { useDispatch, useSelector } from 'react-redux';
import { GamePirate, GameState, PiratePosition, ReduxState } from '/redux/types';
import { chooseHumanPirate, getPirateById } from '/redux/gameSlice';
import { girlsMap } from '/app/global';
import store from '/app/store';
import AnimatePirate from './animatePirate';
import { useCallback } from 'react';

interface MapPirateProps {
    id: string;
    pirateSize: number;
    getMarginTop: (girl: PiratePosition) => number;
    getMarginLeft: (girl: PiratePosition) => number;
    mapSize: number;
    cellSize: number;
}

const MapPirate = ({ id, getMarginTop, getMarginLeft, cellSize, mapSize, pirateSize }: MapPirateProps) => {
    const pirate = useSelector<ReduxState, GamePirate | undefined>((state) => getPirateById(state, id));
    const speed = useSelector<ReduxState, number>((state) => state.game.userSettings.gameSpeed);
    const dispatch = useDispatch();

    if (!pirate) return <></>;

    const leftOffset = pirate.position.x * (cellSize + 1) + getMarginLeft(pirate);
    const topOffset = (mapSize - 1 - pirate.position.y) * (cellSize + 1) + getMarginTop(pirate);

    // TODO: optimize
    const isCurrentPlayerPirate = (girl: GamePirate): boolean => {
        let gameState = store.getState().game as GameState;
        let team = gameState.teams.find((it) => it.id == gameState.currentHumanTeamId);
        return girl.teamId === team?.id;
    };

    const onTeamPirateClick = useCallback(
        (girl: GamePirate, allowChoosing: boolean) => {
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
        },
        [dispatch, chooseHumanPirate],
    );

    return (
        <AnimatePirate
            pirate={pirate}
            pirateSize={pirateSize}
            speed={speed}
            left={leftOffset}
            top={topOffset}
            isCurrentPlayerGirl={isCurrentPlayerPirate(pirate)}
            onTeamPirateClick={onTeamPirateClick}
        />
    );
};
export default MapPirate;
