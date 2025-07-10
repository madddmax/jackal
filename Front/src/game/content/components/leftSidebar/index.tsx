import cn from 'classnames';
import { useState } from 'react';
import { Button, Form, InputGroup } from 'react-bootstrap';
import Image from 'react-bootstrap/Image';
import { useDispatch, useSelector } from 'react-redux';

import {
    chooseHumanPirate,
    getCurrentPlayerPirates,
    getIncludeMovesWithRum,
    getPirateAutoChange,
    getRumBottles,
    getUserSettings,
    saveMySettings,
    setIncludeMovesWithRum,
    setPirateAutoChange,
} from '../../../redux/gameSlice';
import './leftSidebar.less';
import PirateIcon from './pirateIcon';

function LeftSidebar() {
    const dispatch = useDispatch();

    const currentPlayerPirates = useSelector(getCurrentPlayerPirates);
    const hasPirateAutoChange = useSelector(getPirateAutoChange);
    const includeMovesWithRum = useSelector(getIncludeMovesWithRum);
    const rumBottlesCount = useSelector(getRumBottles);
    const userSettings = useSelector(getUserSettings);

    const [gameSpeed, setGameSpeed] = useState<number>(userSettings.gameSpeed || 0);

    const onClick = (girl: GamePirate, withCoinAction: boolean) => () =>
        dispatch(chooseHumanPirate({ pirate: girl.id, withCoinAction }));

    const pirateAutoChangeToggle = (event: { target: { checked: boolean } }) =>
        dispatch(setPirateAutoChange(event.target.checked));
    const includeMovesWithRumToggle = (event: { target: { checked: boolean } }) =>
        dispatch(setIncludeMovesWithRum(event.target.checked));

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
            {currentPlayerPirates &&
                currentPlayerPirates.map((girl, index) => (
                    <PirateIcon
                        key={`pirate_${index}`}
                        pirate={girl}
                        onClick={onClick(girl, false)}
                        onCoinClick={onClick(girl, true)}
                    />
                ))}

            <Form.Group controlId="formBasicCheckbox">
                <Form.Check
                    className="float-end mb-3"
                    style={{ marginLeft: 0 }}
                    type="switch"
                    label={
                        <>
                            <Image
                                src="/pictures/rum.png"
                                className={cn('rum-bottle', { 'rum-bottle-disabled': !includeMovesWithRum })}
                            />
                            x <span className="bottles-count">{rumBottlesCount}</span>
                        </>
                    }
                    checked={includeMovesWithRum}
                    onChange={includeMovesWithRumToggle}
                />
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

export default LeftSidebar;
