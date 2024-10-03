import { useDispatch, useSelector } from 'react-redux';
import { chooseHumanPirate, getCurrentTeam } from '/redux/gameSlice';
import Pirate from './pirate';
import './pirates.less';
import { GamePirate, ReduxState } from '/redux/types';

function Pirates() {
    const dispatch = useDispatch();

    const pirates = useSelector<ReduxState, GamePirate[] | undefined>((state) => state.game.pirates);
    const team = useSelector(getCurrentTeam);

    const onClick = (girl: GamePirate, withCoinAction: boolean) => () =>
        dispatch(chooseHumanPirate({ pirate: girl.id, withCoinAction }));

    return (
        <>
            {pirates &&
                pirates
                    .filter((it) => it.teamId == team?.id)
                    .map((girl, index) => (
                        <Pirate
                            key={`pirate_${index}`}
                            pirate={girl}
                            isActive={team?.activePirate === girl.id}
                            onClick={onClick(girl, false)}
                            onCoinClick={onClick(girl, true)}
                        />
                    ))}
        </>
    );
}

export default Pirates;
