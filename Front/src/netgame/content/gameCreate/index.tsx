import { Button, Container, Row } from 'react-bootstrap';
import { useDispatch, useSelector } from 'react-redux';

import { Constants } from '/app/constants';
import GameSettingsForm from '/app/content/layout/components/gameSettingsForm';
import { getUserSettings, saveMySettings } from '/game/redux/gameSlice';
import { GameSettingsExt } from '/game/types/hubContracts';

const NetGameCreate = () => {
    const dispatch = useDispatch();
    // const navigate = useNavigate();

    const userSettings = useSelector(getUserSettings);

    const newStart = () => {
        // navigate('/');
        // if (formData) {
        //     gameHub.startGame(formData);
        // }
    };

    // const createNetGame = () => {
    //     navigate('/netcreate');

    //     if (formData) {
    //         gameHub.netCreate(formData);
    //     }
    // };

    const saveToLocalStorage = () => {
        if (formData) {
            dispatch(
                saveMySettings({
                    ...userSettings,
                    groups: formData.groups,
                    mapSize: formData.mapSize,
                    players: formData.players.map((it) => it.type),
                    playersMode:
                        formData.gameMode === Constants.gameModeTypes.TwoPlayersInTeam ? 8 : formData.players.length,
                    mapId: formData.isStoredMap ? formData.mapId : undefined,
                    tilesPackName: formData.tilesPackName,
                }),
            );
        }
    };

    let formData: GameSettingsExt | undefined;
    const getFormData = (data: GameSettingsExt) => {
        if (formData?.isStoredMap != data.isStoredMap) {
            saveToLocalStorage();
        }
        formData = data;
    };

    return (
        <Container>
            <Row className="justify-content-center">
                <GameSettingsForm isNet onChange={getFormData}>
                    <Button variant="primary" type="submit" onClick={newStart}>
                        Начать
                    </Button>
                </GameSettingsForm>
            </Row>
        </Container>
    );
};

export default NetGameCreate;
