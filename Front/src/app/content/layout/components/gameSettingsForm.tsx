import { useEffect, useState } from 'react';
import { Button, Form, InputGroup } from 'react-bootstrap';
import { useDispatch, useSelector } from 'react-redux';

import classes from '../newgame.module.less';
import Players from './players';
import { PlayersInfo } from './types';
import { Constants } from '/app/constants';
import { getAuth } from '/auth/redux/authSlice';
import { sagaActions } from '/common/sagas';
import { getGameSettings, getMapForecasts, getUserSettings, setMapForecasts } from '/game/redux/gameSlice';
import { GamePlayer, GameSettingsExt } from '/game/types/hubContracts';

const convertPlayers = (data: PlayersInfo): GamePlayer[] => {
    const { users, members, mode } = data;
    if (mode == 1)
        return [{ userId: members[0] === 'human' ? users[0] : 0, type: members[0], position: Constants.positions[0] }];
    else if (mode == 2)
        return [
            { userId: members[0] === 'human' ? users[0] : 0, type: members[0], position: Constants.positions[0] },
            { userId: members[2] === 'human' ? users[0] : 0, type: members[2], position: Constants.positions[2] },
        ];
    else
        return members.map((it, index) => ({
            userId: it === 'human' ? users[index] : 0,
            type: it,
            position: Constants.positions[index],
        }));
};

const convertMapId = (val: string | number | undefined) => {
    if (val === undefined) return undefined;
    const clone = new Int32Array(1);
    clone[0] = typeof val == 'string' ? Number(val) : val;
    return clone;
};

interface GameSettingsFormProps {
    isNet?: boolean;
    onChange: (data: GameSettingsExt) => void;
    saveToLocalStorage: (hasStoredMapCode: boolean) => void;
    children: React.ReactElement;
}

const GameSettingsForm = ({ isNet, onChange, saveToLocalStorage, children }: GameSettingsFormProps) => {
    const dispatch = useDispatch();

    const userSettings = useSelector(getUserSettings);
    const { tilesPackNames } = useSelector(getGameSettings);
    const mapForecasts = useSelector(getMapForecasts);
    const authInfo = useSelector(getAuth);

    const [players, setPlayers] = useState<PlayersInfo>({
        mode: userSettings.playersMode || 4,
        users: [authInfo.user?.id ?? 0, authInfo.user?.id ?? 0, authInfo.user?.id ?? 0, authInfo.user?.id ?? 0],
        members: userSettings.players || ['human', 'robot2', 'robot', 'robot2'],
        groups: userSettings.groups,
    });

    const [mapSize, setMapSize] = useState(userSettings.mapSize || 11);
    const [tilesPackName, setTilesPackName] = useState<string | undefined>(userSettings.tilesPackName);
    const [isStoredMap, setIsStoredMap] = useState(userSettings.mapId != undefined);

    const [randNumber, setRandNumber] = useState(
        convertMapId(userSettings.mapId) || crypto.getRandomValues(new Int32Array(1)),
    );

    useEffect(() => {
        dispatch({
            type: sagaActions.CHECK_MAP,
            payload: {
                mapId: randNumber[0],
                mapSize,
                tilesPackName,
            },
        });

        return () => {
            dispatch(setMapForecasts());
        };
    }, [dispatch, mapSize, randNumber, tilesPackName]);

    onChange({
        groups: players.groups,
        mapSize,
        players: convertPlayers(players),
        members: players.members,
        isStoredMap: isStoredMap,
        mapId: randNumber[0],
        tilesPackName,
        gameMode: players.mode == 8 ? Constants.gameModeTypes.TwoPlayersInTeam : Constants.gameModeTypes.FreeForAll,
    });

    const changeMapId = () => {
        const newId = crypto.getRandomValues(new Int32Array(1));
        setRandNumber(newId);
        dispatch({
            type: sagaActions.CHECK_MAP,
            payload: {
                mapId: newId[0],
                mapSize,
                tilesPackName,
            },
        });
    };

    const storeMapId = (event: { target: { checked: boolean } }) => {
        setIsStoredMap(event.target.checked);
        saveToLocalStorage(event.target.checked);
    };

    return (
        <Form className={isNet ? classes.netgame : classes.newgame} onSubmit={(event) => event.preventDefault()}>
            <Players players={players} setPlayers={setPlayers} mapInfo={mapForecasts} />
            <div className="mt-3">
                <div>
                    <Form.Label>Размер карты: {mapSize}</Form.Label>
                    <Form.Range
                        value={mapSize}
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
                        value={tilesPackName}
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
                        value={randNumber[0]}
                        onChange={(event) => {
                            setRandNumber(convertMapId(event.target.value)!);
                        }}
                    />
                    <Button variant="outline-secondary" onClick={changeMapId}>
                        Поменять
                    </Button>
                </InputGroup>
            </Form.Group>
            <Form.Group className="mb-3" controlId="formBasicCheckbox">
                <Form.Check type="checkbox" label="Запоминать код карты" checked={isStoredMap} onChange={storeMapId} />
            </Form.Group>
            {children}
        </Form>
    );
};

export default GameSettingsForm;
