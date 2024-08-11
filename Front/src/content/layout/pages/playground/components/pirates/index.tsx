import { useDispatch, useSelector } from 'react-redux';
import { choosePirate } from '/redux/gameSlice';
import Pirate from './pirate';
import './pirates.css';
import { GamePirate, ReduxState, TeamState } from '/redux/types';

function Pirates() {
    const dispatch = useDispatch();

    const pirates = useSelector<ReduxState, GamePirate[] | undefined>(
        (state) => state.game.pirates,
    );
    const team = useSelector<ReduxState, TeamState>(
        (state) =>
            state.game.teams.find((it) => it.id === state.game.currentTeamId!)!,
    );

    const onClick = (girl: GamePirate) => () =>
        dispatch(
            choosePirate({
                pirate: girl.id,
                withCoin:
                    team.activePirate !== girl.id
                        ? girl.withCoin
                        : !girl.withCoin,
            }),
        );

    return (
        <>
            {pirates &&
                pirates
                    .filter((it) => it.teamId == team.id)
                    .map((girl, index) => (
                        <Pirate
                            key={`pirate_${index}`}
                            photoId={girl.photoId || 0}
                            isActive={team.activePirate === girl.id}
                            withCoin={girl.withCoin}
                            onClick={onClick(girl)}
                        />
                    ))}
        </>
    );
}

export default Pirates;
