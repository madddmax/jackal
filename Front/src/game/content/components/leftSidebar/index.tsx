import cn from 'classnames';
import { useState } from 'react';
import { Button, Form, InputGroup, Modal } from 'react-bootstrap';
import Image from 'react-bootstrap/Image';
import { useDispatch, useSelector } from 'react-redux';

import {
    chooseHumanPirate,
    getCurrentPlayerPirates,
    getIncludeMovesWithRum,
    getPirateAutoChange,
    getRumBottles,
    getUserSettings,
    refreshMap,
    saveMySettings,
    setIncludeMovesWithRum,
    setPirateAutoChange,
} from '../../../redux/gameSlice';
import './leftSidebar.less';
import PirateIcon from './pirateIcon';
import { Constants, ImagesPacksIds } from '/app/constants';

function LeftSidebar() {
    const dispatch = useDispatch();

    const currentPlayerPirates = useSelector(getCurrentPlayerPirates);
    const hasPirateAutoChange = useSelector(getPirateAutoChange);
    const includeMovesWithRum = useSelector(getIncludeMovesWithRum);
    const rumBottlesCount = useSelector(getRumBottles);
    const userSettings = useSelector(getUserSettings);

    const [isOpenSettings, setIsOpenSettings] = useState<boolean>(false);
    const [gameSpeed, setGameSpeed] = useState<number>(userSettings.gameSpeed || 0);
    const [hasChessBar, setHasChessBar] = useState<boolean>(userSettings.hasChessBar);

    const onClick = (girl: GamePirate, withCoinAction: boolean) => () =>
        dispatch(chooseHumanPirate({ pirate: girl.id, withCoinAction }));

    const toggleSettings = () => setIsOpenSettings((prev) => !prev);

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

    const switchChessBar = () => {
        dispatch(
            saveMySettings({
                ...userSettings,
                hasChessBar: !hasChessBar,
            }),
        );
        setHasChessBar((prev) => !prev);
    };

    const switchImagesPackName = (event: { target: { value: string } }) => {
        const val = Object.values(ImagesPacksIds).includes(event.target.value as ImagesPacksIds)
            ? (event.target.value as ImagesPacksIds)
            : ImagesPacksIds.classic;
        dispatch(
            saveMySettings({
                ...userSettings,
                imagesPackName: val,
            }),
        );
        dispatch(refreshMap());
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
                    id="rum-bottle-switch"
                    className="photo-position float-end mb-3"
                    style={{ marginLeft: 0 }}
                    type="switch"
                    label={
                        <div style={{ width: '83px' }}>
                            <Image
                                src="/pictures/rum-slim.png"
                                className={cn('rum-bottle', 'me-1', 'mt-2', {
                                    'rum-bottle-disabled': !includeMovesWithRum,
                                })}
                            />
                            x <span className="bottles-count">{rumBottlesCount}</span>
                        </div>
                    }
                    checked={includeMovesWithRum}
                    onChange={includeMovesWithRumToggle}
                />
                <Button variant="outline-secondary" size="sm" onClick={toggleSettings}>
                    Настройки
                </Button>
            </Form.Group>
            <Modal show={isOpenSettings} onHide={() => setIsOpenSettings(false)} keyboard={false}>
                <Modal.Header>
                    <Modal.Title>Локальные настройки</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <Form>
                        <Form.Group className="mb-3" controlId="images-pack-change-switch">
                            <Form.Label>Оформление карты</Form.Label>
                            <Form.Select
                                id="images-pack-change-switch"
                                name="imagesPackName"
                                value={userSettings.imagesPackName ?? ImagesPacksIds.classic}
                                onChange={switchImagesPackName}
                            >
                                {Object.keys(Constants.imagesPacks).map((it) => (
                                    <option value={it}>{it}</option>
                                ))}
                            </Form.Select>
                        </Form.Group>
                        <Form.Group className="mb-3" controlId="pirate-auto-change-switch">
                            <Form.Check
                                id="pirate-auto-change-switch"
                                type="switch"
                                label="Автовыбор пиратки"
                                checked={hasPirateAutoChange}
                                onChange={pirateAutoChangeToggle}
                            />
                        </Form.Group>
                        <Form.Group className="mb-3" controlId="chess-bar-change-switch">
                            <Form.Check
                                id="chess-bar-change-switch"
                                type="switch"
                                label="Шахматная нотация"
                                checked={hasChessBar}
                                onChange={switchChessBar}
                            />
                        </Form.Group>
                        <Form.Group className="mb-3" controlId="images-pack-change-switch">
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
                    </Form>
                </Modal.Body>
            </Modal>
        </>
    );
}

export default LeftSidebar;
