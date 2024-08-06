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
                pirate: girl.Id,
                withCoin:
                    team.activePirate !== girl.Id
                        ? girl.WithCoin
                        : !girl.WithCoin,
            }),
        );

    return (
        <>
            {pirates &&
                pirates
                    .filter((it) => it.TeamId == team.id)
                    .map((girl, index) => (
                        <Pirate
                            key={`pirate_${index}`}
                            photo={`/pictures/pirate_${index + 1}.png`}
                            isActive={team.activePirate === girl.Id}
                            withCoin={girl.WithCoin}
                            onClick={onClick(girl)}
                        />
                    ))}
        </>
    );
}

export default Pirates;
