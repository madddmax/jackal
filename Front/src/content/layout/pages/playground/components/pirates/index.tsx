import { useDispatch, useSelector } from 'react-redux';
import { highlightMoves } from '/redux/gameSlice';
import Pirate from './pirate';
import './pirates.css';
import { GamePirate, ReduxState } from '/redux/types';

function Pirates() {
    const dispatch = useDispatch();

    const pirates = useSelector<ReduxState, GamePirate[] | undefined>(
        (state) => state.game.pirates,
    );
    const activePirate = useSelector<ReduxState, string>(
        (state) => state.game.activePirate,
    );
    const withCoin = useSelector<ReduxState, boolean | undefined>(
        (state) => state.game.withCoin,
    );

    const onClick = (id: string) => () =>
        dispatch(
            highlightMoves({
                pirate: id,
                withCoin:
                    activePirate !== id || withCoin === undefined
                        ? undefined
                        : !withCoin,
            }),
        );

    return (
        <>
            {pirates &&
                pirates.map((girl, index) => (
                    <Pirate
                        key={`pirate_${index}`}
                        photo={`/pictures/pirate_${index + 1}.png`}
                        isActive={activePirate === girl.Id}
                        withCoin={
                            activePirate === girl.Id ? withCoin : undefined
                        }
                        onClick={onClick(girl.Id)}
                    />
                ))}
        </>
    );
}

export default Pirates;
