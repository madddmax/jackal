import { useState } from 'react';
import { Button } from 'react-bootstrap';
import Container from 'react-bootstrap/Container';
import Row from 'react-bootstrap/Row';
import { useDispatch, useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';

import { getUserSettings, saveMySettings } from '../../../game/redux/gameSlice';
import GameSettingsForm from './components/gameSettingsForm';
import { convertToSettings } from '/app/global';
import { getAuth } from '/auth/redux/authSlice';
import { PlayerTypes } from '/common/constants';
import gameHub from '/game/hub/gameHub';
import { GameSettingsFormData } from '/game/types/hubContracts';

const Newgame = () => {
    const dispatch = useDispatch();
    const navigate = useNavigate();

    const authInfo = useSelector(getAuth);
    const userSettings = useSelector(getUserSettings);

    let counter = 0;
    const allowedGamers = Object.values(PlayerTypes).map((it) => ({
        id: counter++,
        type: it,
        userId: it == PlayerTypes.Human ? (authInfo.user?.id ?? 0) : 0,
    }));

    const [formData, setFormData] = useState<GameSettingsFormData>({
        players: {
            mode: userSettings.playersMode || 4,
            users: [authInfo.user?.id ?? 0, authInfo.user?.id ?? 0, authInfo.user?.id ?? 0, authInfo.user?.id ?? 0],
            gamers: (
                userSettings.players || [PlayerTypes.Human, PlayerTypes.Robot2, PlayerTypes.Robot, PlayerTypes.Robot2]
            ).map((it) => allowedGamers.find((gm) => gm.type === it) ?? allowedGamers[0]),
            groups: userSettings.groups,
        },
        gamers: allowedGamers,
        mapId: userSettings.mapId,
        mapSize: userSettings.mapSize || 11,
        tilesPackName: userSettings.tilesPackName,
        isStoredMap: userSettings.mapId != undefined,
    });

    const newStart = () => {
        saveToLocalStorage();
        gameHub.startGame(convertToSettings(formData));
    };

    const createNetGame = () => {
        navigate('/newpublic');

        gameHub.netCreate(convertToSettings(formData));
    };

    const saveToLocalStorage = () => {
        dispatch(
            saveMySettings({
                ...userSettings,
                groups: formData.players.groups,
                mapSize: formData.mapSize,
                players: formData.players.gamers.map((it) => it.type),
                playersMode: formData.players.mode,
                mapId: formData.isStoredMap ? formData.mapId : undefined,
                tilesPackName: formData.tilesPackName,
            }),
        );
    };

    const setGameFormData = (data: GameSettingsFormData) => {
        if (formData.isStoredMap != data.isStoredMap) {
            saveToLocalStorage();
        }
        setFormData(data);
    };

    return (
        <Container>
            <Row className="justify-content-center">
                <GameSettingsForm gameSettingsData={formData} setGameSettingsData={setGameFormData}>
                    <>
                        <Button variant="primary" type="submit" onClick={newStart}>
                            Начать
                        </Button>
                        <Button className="float-end" variant="outline-primary" type="submit" onClick={createNetGame}>
                            Пригласить участников
                        </Button>
                    </>
                </GameSettingsForm>
            </Row>
        </Container>
    );
};

export default Newgame;
