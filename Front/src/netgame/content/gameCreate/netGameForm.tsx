import { useState } from 'react';
import { Button } from 'react-bootstrap';
import { useDispatch, useSelector } from 'react-redux';

import { Constants } from '/app/constants';
import GameSettingsForm from '/app/content/layout/components/gameSettingsForm';
import { PlayerInfo } from '/app/content/layout/components/types';
import { convertToGamers, convertToMembers, convertToSettings, convertToUsers } from '/app/global';
import { getAuth } from '/auth/redux/authSlice';
import gameHub from '/game/hub/gameHub';
import { getUserSettings, saveMySettings } from '/game/redux/gameSlice';
import { GameSettingsFormData } from '/game/types/hubContracts';
import { getNetGames } from '/netgame/redux/lobbySlice';
import { NetGameInfo } from '/netgame/redux/lobbySlice.types';

const isEqualsLists = (sList: string[], rList: string[]): boolean => {
    if (sList.length !== rList.length) return false;
    for (let i = 0; i < sList.length; i++) {
        if (sList[i] !== rList[i]) return false;
    }
    return true;
};

export interface NetGameFormProps {
    netGame: NetGameInfo;
}

const NetGameForm = ({ netGame }: NetGameFormProps) => {
    const dispatch = useDispatch();

    const authInfo = useSelector(getAuth);
    const userSettings = useSelector(getUserSettings);
    const netGames = useSelector(getNetGames);

    const [groups, setGroups] = useState<string[]>(userSettings.groups);

    const newNetStart = () => {
        gameHub.startPublicGame(netGame.id, convertToSettings(formData));
    };

    let counter = 0;
    const gamers: PlayerInfo[] = netGame.users
        .map((it) => ({ id: counter++, type: 'human', userId: it.id, userName: it.login }) as PlayerInfo)
        .concat([
            { id: counter++, type: 'robot', userId: 0 } as PlayerInfo,
            { id: counter++, type: 'robot2', userId: 0 } as PlayerInfo,
            { id: counter++, type: 'robot3', userId: 0 } as PlayerInfo,
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
            users: convertToUsers(netGame.settings.players, [
                authInfo.user?.id ?? 0,
                authInfo.user?.id ?? 0,
                authInfo.user?.id ?? 0,
                authInfo.user?.id ?? 0,
            ]),
            gamers: convertToGamers(
                netGame.settings.players,
                gamers,
                (userSettings.players || ['human', 'robot2', 'robot', 'robot2']).map(
                    (it) => gamers.find((gm) => gm.type === it) ?? gamers[0],
                ),
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
        if (!isEqualsLists(groups, data.players.groups)) {
            saveToLocalStorage(data.players.groups);
            setGroups(data.players.groups);
        }
        const curGame = netGames.find((it) => it.id === netGame?.id);
        if (curGame?.isCreator) {
            gameHub.netChange(netGame?.id, convertToSettings(data));
        }
    };

    const saveToLocalStorage = (grps: string[]) => {
        dispatch(
            saveMySettings({
                ...userSettings,
                groups: grps,
            }),
        );
    };

    return (
        <GameSettingsForm
            id={netGame.id}
            viewers={netGame.users}
            isPublic
            isEditGroupsOnly={!netGame.isCreator}
            gameSettingsData={formData}
            setGameSettingsData={setFormData}
        >
            <>
                {netGame.isCreator && (
                    <Button variant="primary" type="submit" onClick={newNetStart}>
                        Начать
                    </Button>
                )}
            </>
        </GameSettingsForm>
    );
};

export default NetGameForm;
