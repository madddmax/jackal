import { useCallback } from 'react';
import { useDispatch, useSelector } from 'react-redux';

import { girlsMap } from '../../../logic/gameLogic';
import {
    chooseHumanPirate,
    getCurrentPlayerTeam,
    getPirateById,
    getUserSettings,
    takeOrPutCoin,
} from '../../../redux/gameSlice';
import AnimatePirate from './animatePirate';
import { GameState } from '/game/types';

interface MapPirateProps {
    id: string;
    pirateSize: number;
    getMarginTop: (girl: GamePiratePosition) => number;
    getMarginLeft: (girl: GamePiratePosition) => number;
    mapSize: number;
    cellSize: number;
    chessBarSize: number;
}

const MapPirate = ({
    id,
    getMarginTop,
    getMarginLeft,
    cellSize,
    mapSize,
    pirateSize,
    chessBarSize,
}: MapPirateProps) => {
    const pirate = useSelector<{ game: GameState }, GamePirate | undefined>((state) => getPirateById(state, id));
    const currentHumanTeam = useSelector<{ game: GameState }, TeamState | undefined>((state) =>
        getCurrentPlayerTeam(state),
    );

    const { gameSpeed: speed } = useSelector(getUserSettings);
    const dispatch = useDispatch();

    const onTeamPirateClick = useCallback(
        (girl: GamePirate, allowChoosing: boolean) => {
            const mapLevel = girlsMap.GetPosition(girl);
            if (!mapLevel || !mapLevel.girls) return;

            const willChangePirate = mapLevel.girls.length > 1 && girl.isActive && allowChoosing;
            if (willChangePirate) {
                girlsMap.ScrollGirls(mapLevel);
            }
            const curGirl = willChangePirate ? mapLevel.girls[mapLevel.girls.length - 1]?.id : girl.id;
            if (!girl.isActive || willChangePirate) {
                dispatch(chooseHumanPirate({ pirate: curGirl }));
            } else {
                dispatch(takeOrPutCoin({ pirate: curGirl }));
            }
        },
        [dispatch],
    );

    if (!pirate) return <></>;

    const leftOffset = pirate.position.x * (cellSize + 1) + getMarginLeft(pirate) + chessBarSize;
    const topOffset = (mapSize - 1 - pirate.position.y) * (cellSize + 1) + getMarginTop(pirate) + chessBarSize;

    return (
        <AnimatePirate
            pirate={pirate}
            pirateSize={pirateSize}
            speed={speed}
            left={leftOffset}
            top={topOffset}
            isCurrentPlayerGirl={pirate.teamId === currentHumanTeam?.id}
            onTeamPirateClick={onTeamPirateClick}
        />
    );
};
export default MapPirate;
