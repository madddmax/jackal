import { useDispatch, useSelector } from 'react-redux';
import { chooseHumanPirate, getCurrentTeam, setPirateAutoChange } from '/redux/gameSlice';
import Pirate from './pirate';
import './pirates.less';
import { GamePirate, ReduxState } from '/redux/types';
import { Form } from 'react-bootstrap';

function Pirates() {
    const dispatch = useDispatch();

    const pirates = useSelector<ReduxState, GamePirate[] | undefined>((state) => state.game.pirates);
    const hasPirateAutoChange = useSelector<ReduxState, boolean>((state) => state.game.hasPirateAutoChange);
    const team = useSelector(getCurrentTeam);

    const onClick = (girl: GamePirate, withCoinAction: boolean) => () =>
        dispatch(chooseHumanPirate({ pirate: girl.id, withCoinAction }));

    const pirateAutoChangeToggle = (event: any) => dispatch(setPirateAutoChange(event.target.checked));

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

            <Form.Group className="mb-3" controlId="formBasicCheckbox">
                <Form.Check
                    className="photo-position float-end"
                    style={{ marginLeft: 0 }}
                    type="switch"
                    label="Автовыбор пиратки"
                    checked={hasPirateAutoChange}
                    onChange={pirateAutoChangeToggle}
                />
            </Form.Group>
        </>
    );
}

export default Pirates;
