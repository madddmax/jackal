import { useEffect } from 'react';
import { Button, Form, InputGroup } from 'react-bootstrap';
import { useDispatch, useSelector } from 'react-redux';

import classes from '../newgame.module.less';
import Players from './players';
import { PlayersInfo } from './types';
import { sagaActions } from '/common/sagas';
import { getGameSettings, getMapForecasts, setMapForecasts } from '/game/redux/gameSlice';
import { GameSettingsFormData } from '/game/types/hubContracts';

export interface GameSettingsFormProps {
    isPublic?: boolean;
    gameSettingsData: GameSettingsFormData;
    setGameSettingsData: (data: GameSettingsFormData) => void;
    children: React.ReactElement;
}

const GameSettingsForm = ({ isPublic, gameSettingsData, setGameSettingsData, children }: GameSettingsFormProps) => {
    const dispatch = useDispatch();

    const { tilesPackNames } = useSelector(getGameSettings);
    const mapForecasts = useSelector(getMapForecasts);

    if (!gameSettingsData.mapId) gameSettingsData.mapId = crypto.getRandomValues(new Int32Array(1))[0];

    useEffect(() => {
        dispatch({
            type: sagaActions.CHECK_MAP,
            payload: {
                mapId: gameSettingsData.mapId,
                mapSize: gameSettingsData.mapSize,
                tilesPackName: gameSettingsData.tilesPackName,
            },
        });

        return () => {
            dispatch(setMapForecasts());
        };
    }, [dispatch, gameSettingsData.mapId, gameSettingsData.mapSize, gameSettingsData.tilesPackName]);

    const setPlayers = (data: PlayersInfo) => {
        setGameSettingsData({
            ...gameSettingsData,
            players: data,
        });
    };

    const setMapSize = (data: number) => {
        setGameSettingsData({
            ...gameSettingsData,
            mapSize: data,
        });
    };

    const setTilesPackName = (data: string) => {
        setGameSettingsData({
            ...gameSettingsData,
            tilesPackName: data,
        });
    };

    const setMapId = (data: number) => {
        setGameSettingsData({
            ...gameSettingsData,
            mapId: data,
        });
        dispatch({
            type: sagaActions.CHECK_MAP,
            payload: {
                mapId: data,
                mapSize: gameSettingsData.mapSize,
                tilesPackName: gameSettingsData.tilesPackName,
            },
        });
    };

    const changeMapId = () => {
        const newId = crypto.getRandomValues(new Int32Array(1));
        setMapId(newId[0]);
    };

    const storeMapId = (event: { target: { checked: boolean } }) => {
        setGameSettingsData({
            ...gameSettingsData,
            isStoredMap: event.target.checked,
        });
    };

    return (
        <Form className={classes.newgame} onSubmit={(event) => event.preventDefault()}>
            <Players
                players={gameSettingsData.players}
                gamers={gameSettingsData.gamers}
                setPlayers={setPlayers}
                mapInfo={mapForecasts}
            />
            <div className="mt-3">
                {isPublic ? (
                    <div className="badge rounded-pill bg-warning text-dark">Публичная игра</div>
                ) : (
                    <div className="badge rounded-pill bg-success">Частная игра</div>
                )}
            </div>
            <div className="mt-3">
                <div>
                    <Form.Label>Размер карты: {gameSettingsData.mapSize}</Form.Label>
                    <Form.Range
                        value={gameSettingsData.mapSize}
                        min={5}
                        max={13}
                        step={2}
                        name="mapSize"
                        onChange={(e) => setMapSize(Number(e.target.value))}
                        className="custom-slider"
                    />
                </div>
            </div>
            {tilesPackNames && tilesPackNames.length > 0 && (
                <Form.Group className="mb-3" controlId="formBasicSelect">
                    <Form.Label>Игровой набор</Form.Label>
                    <Form.Select
                        name="tilesPackName"
                        value={gameSettingsData.tilesPackName}
                        onChange={(event) => {
                            setTilesPackName(event.target.value);
                        }}
                    >
                        {tilesPackNames.map((it) => (
                            <option value={it}>{it}</option>
                        ))}
                    </Form.Select>
                </Form.Group>
            )}
            <Form.Group className="mb-3" controlId="formBasicEmail">
                <Form.Label>Код карты</Form.Label>
                <InputGroup>
                    <Form.Control
                        type="text"
                        name="mapcode"
                        placeholder="Введите код"
                        value={gameSettingsData.mapId}
                        onChange={(event) => {
                            if (event.target.value) setMapId(Number(event.target.value));
                        }}
                    />
                    <Button variant="outline-secondary" onClick={changeMapId}>
                        Поменять
                    </Button>
                </InputGroup>
            </Form.Group>
            <Form.Group className="mb-3" controlId="formBasicCheckbox">
                <Form.Check
                    type="checkbox"
                    label="Запоминать код карты"
                    checked={gameSettingsData.isStoredMap}
                    onChange={storeMapId}
                />
            </Form.Group>
            {children}
        </Form>
    );
};

export default GameSettingsForm;
