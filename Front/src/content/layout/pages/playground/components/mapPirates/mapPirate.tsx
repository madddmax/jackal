import { useSelector } from 'react-redux';
import PiratePhoto from '../map/piratePhoto';
import { GamePirate, ReduxState } from '/redux/types';
import { getPirateById } from '/redux/gameSlice';

interface MapPirateProps {
    id: string;
    pirateSize: number;
    getMarginTop: (girl: GamePirate) => number;
    getMarginLeft: (girl: GamePirate) => number;
    mapSize: number;
    cellSize: number;
}

const MapPirate = ({ id, ...others }: MapPirateProps) => {
    const pirate = useSelector<ReduxState, GamePirate | undefined>((state) => getPirateById(state, id));

    if (!pirate) return <></>;

    return <PiratePhoto pirate={pirate} {...others} />;
};
export default MapPirate;
