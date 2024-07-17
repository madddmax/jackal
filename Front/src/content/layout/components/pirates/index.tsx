import { useDispatch, useSelector } from 'react-redux';
import cn from 'classnames';

import Image from 'react-bootstrap/Image';
import { highlightMoves } from '/redux/gameSlice';
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

    return (
        <>
            <Image
                src="/pictures/smallet.jpg"
                roundedCircle
                className={cn('photo float-end', {
                    'photo-active': activePirate === 1,
                })}
                onClick={() => dispatch(highlightMoves({ pirate: 1 }))}
            />
            <Image
                src="/pictures/gokins.jpg"
                roundedCircle
                className={cn('photo float-end', {
                    'photo-active': activePirate === 2,
                })}
                onClick={() => dispatch(highlightMoves({ pirate: 2 }))}
            />
            <Image
                src="/pictures/livsi.jpg"
                roundedCircle
                className={cn('photo float-end', {
                    'photo-active': activePirate === 3,
                })}
                onClick={() => dispatch(highlightMoves({ pirate: 3 }))}
            />
            {withCoin !== undefined && (
                <div className="form-check float-end">
                    <input
                        className="form-check-input"
                        type="checkbox"
                        value=""
                        id="with-coin"
                        checked={withCoin}
                        onChange={() =>
                            dispatch(highlightMoves({ withCoin: !withCoin }))
                        }
                    />
                    <label className="form-check-label" htmlFor="with-coin">
                        с монетой
                    </label>
                </div>
            )}
        </>
    );
}

export default Pirates;
