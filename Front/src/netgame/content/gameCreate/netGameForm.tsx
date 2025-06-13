import { useState } from 'react';
import { Button } from 'react-bootstrap';
import { useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';

import { Constants } from '/app/constants';
import GameSettingsForm from '/app/content/layout/components/gameSettingsForm';
import { PlayerInfo } from '/app/content/layout/components/types';
import { convertToMembers, convertToSettings } from '/app/global';
import { getAuth } from '/auth/redux/authSlice';
import { NetGameInfo } from '/common/redux.types';
import gameHub from '/game/hub/gameHub';
import { getUserSettings } from '/game/redux/gameSlice';
import { GameSettingsFormData } from '/game/types/hubContracts';
import { getNetGames } from '/netgame/redux/lobbySlice';

export interface NetGameFormProps {
    netGame: NetGameInfo;
}

const NetGameForm = ({ netGame }: NetGameFormProps) => {
    const navigate = useNavigate();

    const authInfo = useSelector(getAuth);
    const userSettings = useSelector(getUserSettings);
    const netGames = useSelector(getNetGames);

    const [groups, setGroups] = useState<string[]>(userSettings.groups);

    const newNetStart = () => {
        navigate('/');
        if (formData) {
            gameHub.startGame(convertToSettings(formData));
        }
    };

    // const createNetGame = () => {
    //     navigate('/newpublic');

    //     if (formData) {
    //         gameHub.netCreate(formData);
    //     }
    // };

    let counter = 0;
    const gamers: PlayerInfo[] = netGame.viewers
        .map((it) => ({ id: counter++, type: 'human', userId: it }))
        .concat([
            { id: counter++, type: 'robot', userId: 0 },
            { id: counter++, type: 'robot2', userId: 0 },
            { id: counter++, type: 'robot3', userId: 0 },
        ]);

    const formData: GameSettingsFormData = {
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

    const setFormData = (data: GameSettingsFormData) => {
        setGroups(data.players.groups);
        const curGame = netGames.find((it) => it.id === netGame?.id);
        if (authInfo.user?.id === curGame?.creator.id) {
            gameHub.netChange(netGame?.id, convertToSettings(data));
        }
    };

    return (
        <GameSettingsForm isPublic gameSettingsData={formData} setGameSettingsData={setFormData}>
            <Button variant="primary" type="submit" onClick={newNetStart}>
                Начать
            </Button>
        </GameSettingsForm>
    );
};

export default NetGameForm;
