import { useState } from 'react';
import { Button, Container, Row } from 'react-bootstrap';
import { useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';

import { Constants } from '/app/constants';
import GameSettingsForm from '/app/content/layout/components/gameSettingsForm';
import { PlayerInfo } from '/app/content/layout/components/types';
import { convertToMembers, convertToSettings } from '/app/global';
import { getAuth } from '/auth/redux/authSlice';
import gameHub from '/game/hub/gameHub';
import { getUserSettings } from '/game/redux/gameSlice';
import { GameSettingsFormData } from '/game/types/hubContracts';
import { getNetGame, getNetGames } from '/netgame/redux/lobbySlice';

const NetGameCreate = () => {
    const navigate = useNavigate();

    const authInfo = useSelector(getAuth);
    const userSettings = useSelector(getUserSettings);
    const netGame = useSelector(getNetGame);
    const netGames = useSelector(getNetGames);

    const [groups, setGroups] = useState<string[]>(userSettings.groups);

    const newNetStart = () => {
        navigate('/');
        if (formData) {
            gameHub.startGame(convertToSettings(formData));
        }
    };

    // const createNetGame = () => {
    //     navigate('/netcreate');

    //     if (formData) {
    //         gameHub.netCreate(formData);
    //     }
    // };

    let formData: GameSettingsFormData | undefined;
    if (netGame) {
        let counter = 0;
        const gamers: PlayerInfo[] = netGame.viewers
            .map((it) => ({ id: counter++, type: 'human', userId: it }))
            .concat([
                { id: counter++, type: 'robot', userId: 0 },
                { id: counter++, type: 'robot2', userId: 0 },
                { id: counter++, type: 'robot3', userId: 0 },
            ]);

        formData = {
            players: {
                mode:
                    netGame.settings.gameMode === Constants.gameModeTypes.TwoPlayersInTeam
                        ? 8
                        : netGame.settings.players.length,
                members: convertToMembers(
                    netGame.settings.players,
                    userSettings.players || ['human', 'robot2', 'robot', 'robot2'],
                ),
                users: netGame.settings.players.map((it) => it.userId),
                gamers: netGame.settings.players.map((it) =>
                    it.userId > 0
                        ? (gamers.find((gm) => gm.userId === it.userId) ?? gamers[0])
                        : (gamers.find((gm) => gm.type === it.type.toLocaleLowerCase()) ?? gamers[0]),
                ),
                groups: groups,
            },
            gamers,
            mapId: netGame.settings.mapId,
            mapSize: netGame.settings.mapSize,
            tilesPackName: netGame.settings.tilesPackName,
            isStoredMap: true,
        };
    } else {
        let counter = 0;
        const gamers = [
            { id: counter++, type: 'human', userId: authInfo.user?.id ?? 0 },
            { id: counter++, type: 'robot', userId: 0 },
            { id: counter++, type: 'robot2', userId: 0 },
            { id: counter++, type: 'robot3', userId: 0 },
        ];

        formData = {
            players: {
                mode: userSettings.playersMode || 4,
                members: userSettings.players || ['human', 'robot2', 'robot', 'robot2'],
                users: [authInfo.user?.id ?? 0, authInfo.user?.id ?? 0, authInfo.user?.id ?? 0, authInfo.user?.id ?? 0],
                gamers: (userSettings.players || ['human', 'robot2', 'robot', 'robot2']).map(
                    (it) => gamers.find((gm) => gm.type === it) ?? gamers[0],
                ),
                groups: groups,
            },
            gamers,
            mapId: userSettings.mapId,
            mapSize: userSettings.mapSize || 11,
            tilesPackName: userSettings.tilesPackName,
            isStoredMap: userSettings.mapId != undefined,
        };
    }

    const setFormData = (data: GameSettingsFormData) => {
        setGroups(data.players.groups);
        if (netGame) {
            const curGame = netGames.find((it) => it.id === netGame?.id);
            if (authInfo.user?.id === curGame?.creator.id) {
                gameHub.netChange(netGame?.id, convertToSettings(data));
            }
        }
    };

    return (
        <Container>
            <Row className="justify-content-center">
                <GameSettingsForm isNetStyle gameSettingsData={formData} setGameSettingsData={setFormData}>
                    <Button variant="primary" type="submit" onClick={newNetStart}>
                        Начать
                    </Button>
                </GameSettingsForm>
            </Row>
        </Container>
    );
};

export default NetGameCreate;
