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
    gamers: PlayerInfo[];
    setPlayers: (data: PlayersInfo) => void;
    mapInfo?: string[];
}

const Players = ({ players, gamers, setPlayers, mapInfo }: PlayersProps) => {
    const [grps, setGrps] = useState<number[]>(convertGroups(players.groups));

    const changeGamer = (pos: number) => {
        const clone = [...players.gamers];
        if (clone[pos].id + 1 >= gamers.length) clone[pos] = gamers[0];
        else clone[pos] = gamers.find((it) => it.id === clone[pos].id + 1) ?? gamers[0];
        setPlayers({
            ...players,
            gamers: clone,
        });
    };

    const changeGroup = (pos: number) => {
        const clone = [...grps];
        let current = clone[pos];
        while (clone.includes(current)) {
            if (current + 1 >= Constants.groups.length) {
                current = 0;
            } else {
                current += 1;
            }
        }
        clone[pos] = current;
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
                            key={`player-pos-${index}`}
                            position={index}
                            type={gamer.type}
                            userName={gamer.userId > 0 ? gamer.userName : undefined}
                            group={grps[index]}
                            posInfo={mapInfo && mapInfo[index]}
                            changePlayer={() => changeGamer(index)}
                            changeGroup={() => changeGroup(index)}
                        />
                    );
                })}
            <div
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
