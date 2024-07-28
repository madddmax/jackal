import { useDispatch, useSelector } from 'react-redux';
import { highlightMoves } from '/redux/gameSlice';
import Pirate from './pirate';
import './pirates.css';
import { ReduxState } from '/redux/types';

function Pirates() {
    const dispatch = useDispatch();

    const activePirate = useSelector<ReduxState, number>(
        (state) => state.game.activePirate,
    );
    const withCoin = useSelector<ReduxState, boolean | undefined>(
        (state) => state.game.withCoin,
    );

    const onClick = (num: number) => () =>
        dispatch(
            highlightMoves({
                pirate: num,
                withCoin:
                    activePirate !== num || withCoin === undefined
                        ? undefined
                        : !withCoin,
            }),
        );

    return (
        <>
            <Pirate
                photo="/pictures/pirate_1.png"
                isActive={activePirate === 1}
                withCoin={activePirate === 1 ? withCoin : undefined}
                onClick={onClick(1)}
            />
            <Pirate
                photo="/pictures/pirate_2.png"
                isActive={activePirate === 2}
                withCoin={activePirate === 2 ? withCoin : undefined}
                onClick={onClick(2)}
            />
            <Pirate
                photo="/pictures/pirate_3.png"
                isActive={activePirate === 3}
                withCoin={activePirate === 3 ? withCoin : undefined}
                onClick={onClick(3)}
            />
        </>
    );
}

export default Pirates;
