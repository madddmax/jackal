import cn from 'classnames';
import { useState } from 'react';

import { PlayerInfo, PlayersInfo } from '../types';
import Player from './player';
import classes from './players.module.less';
import { Constants } from '/app/constants';

const convertGroups = (grps: string[]) => grps.map((gr) => Constants.groups.findIndex((it) => it.id == gr) || 0);
const deconvertGroups = (groups: number[]) => groups.map((num) => Constants.groups[num].id);

export interface PlayersProps {
    players: PlayersInfo;
    allowedGamers: PlayerInfo[];
    setPlayers: (data: PlayersInfo) => void;
    mapInfo?: string[];
    isPublic: boolean;
}

const Players = ({ players, allowedGamers, setPlayers, mapInfo, isPublic }: PlayersProps) => {
    const [grps, setGrps] = useState<number[]>(convertGroups(players.groups));

    const changeGamer = (pos: number, playerId: number) => {
        const clone = [...players.gamers];
        clone[pos] = allowedGamers[playerId];
        setPlayers({
            ...players,
            gamers: clone,
        });
    };

    const isIgnoredGroup = (pos: number): boolean => grps.includes(pos);

    const changeGroup = (pos: number, grpId: number) => {
        const clone = [...grps];
        clone[pos] = grpId;
        setGrps(clone);
        setPlayers({
            ...players,
            groups: deconvertGroups(clone),
        });
    };

    const calcNextMode = (prev: number) => {
        if (prev == 8) return 1;
        else if (prev == 1) return 2;
        else if (prev == 2) return 4;
        return 8;
    };

    return (
        <div className={cn(classes.settings, 'mx-auto')}>
            {players &&
                players.gamers &&
                players.gamers.map((gamer, index) => {
                    if (players.mode < 4 && (index == 1 || index == 3)) {
                        return null;
                    }
                    if (players.mode < 2 && index == 2) {
                        return null;
                    }

                    return (
                        <Player
                            id={`player_pos_${index}`}
                            key={`player-pos-${index}`}
                            position={index}
                            type={gamer.type}
                            userName={gamer.userId > 0 ? gamer.userName : undefined}
                            group={grps[index]}
                            posInfo={mapInfo && mapInfo[index]}
                            changePlayer={(id) => changeGamer(index, id)}
                            changeGroup={(id) => changeGroup(index, id)}
                            isIgnoredGroup={isIgnoredGroup}
                            allowedGamers={allowedGamers}
                            isPublic={isPublic}
                        />
                    );
                })}
            <div
                id="players_mode_cntrl"
                className={classes.player}
                onClick={() =>
                    setPlayers({
                        ...players,
                        mode: calcNextMode(players.mode),
                    })
                }
                style={{
                    cursor: 'pointer',
                    top: '110px',
                    left: players.mode == 8 ? '110px' : '130px',
                    fontSize: '48px',
                    lineHeight: '48px',
                    textAlign: 'center',
                }}
            >
                {players.mode == 8 ? '2x2' : players.mode}
            </div>
        </div>
    );
};

export default Players;
