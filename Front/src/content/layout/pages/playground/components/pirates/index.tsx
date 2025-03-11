import { useDispatch, useSelector } from 'react-redux';
import { chooseHumanPirate, getCurrentPlayerTeam, saveMySettings, setPirateAutoChange } from '/redux/gameSlice';
import Pirate from './pirate';
import './pirates.less';
import { GamePirate, ReduxState, StorageState } from '/redux/types';
import { Button, Form, InputGroup } from 'react-bootstrap';
import { useState } from 'react';

function Pirates() {
    const dispatch = useDispatch();

    const pirates = useSelector<ReduxState, GamePirate[] | undefined>((state) => state.game.pirates);
    const hasPirateAutoChange = useSelector<ReduxState, boolean>((state) => state.game.hasPirateAutoChange);
    const userSettings = useSelector<ReduxState, StorageState>((state) => state.game.userSettings);
    const currentPlayerTeam = useSelector(getCurrentPlayerTeam);

    const [gameSpeed, setGameSpeed] = useState<number>(userSettings.gameSpeed || 0);

    const onClick = (girl: GamePirate, withCoinAction: boolean) => () =>
        dispatch(chooseHumanPirate({ pirate: girl.id, withCoinAction }));

    const pirateAutoChangeToggle = (event: any) => dispatch(setPirateAutoChange(event.target.checked));

    const increaseSpeed = () => {
        if (gameSpeed >= 10) return;
        dispatch(
            saveMySettings({
                ...userSettings,
                gameSpeed: gameSpeed + 1,
            }),
        );
        setGameSpeed((prev) => prev + 1);
    };

    const decreaseSpeed = () => {
        if (gameSpeed <= 0) return;
        dispatch(
            saveMySettings({
                ...userSettings,
                gameSpeed: gameSpeed - 1,
            }),
        );
        setGameSpeed((prev) => prev - 1);
    };

    return (
        <>
            {pirates &&
                pirates
                    .filter((it) => it.teamId == currentPlayerTeam?.id)
                    .map((girl, index) => (
                        <Pirate
                            key={`pirate_${index}`}
                            pirate={girl}
                            isActive={girl.isActive}
                            onClick={onClick(girl, false)}
                            onCoinClick={onClick(girl, true)}
                        />
                    ))}

            <Form.Group controlId="formBasicCheckbox">
                <Form.Check
                    className="photo-position float-end mb-3"
                    style={{ marginLeft: 0 }}
                    type="switch"
                    label="Автовыбор пиратки"
                    checked={hasPirateAutoChange}
                    onChange={pirateAutoChangeToggle}
                />
                <Form.Label>Задержка хода</Form.Label>
                <InputGroup className="mb-3" size="sm">
                    <Button variant="outline-secondary" onClick={decreaseSpeed} disabled={gameSpeed <= 0}>
                        -
                    </Button>
                    <Form.Control value={gameSpeed / 10} />
                    <Button variant="outline-secondary" onClick={increaseSpeed} disabled={gameSpeed >= 10}>
                        +
                    </Button>
                </InputGroup>
            </Form.Group>
        </>
    );
}

export default Pirates;
