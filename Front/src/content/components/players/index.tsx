import cn from 'classnames';
import classes from './players.module.less';
import Player from './player';
import { Constants } from '/app/constants';
import { PlayersInfo } from '../types';
import { useState } from 'react';

const convertGroups = (grps: string[]) => grps.map((gr) => Constants.groups.findIndex((it) => it.id == gr) || 0);
const deconvertGroups = (groups: number[]) => groups.map((num) => Constants.groups[num].id);

interface PlayersProps {
    players: PlayersInfo;
    setPlayers: (data: PlayersInfo) => void;
    mapInfo?: string[];
}

const Players = ({ players, setPlayers, mapInfo }: PlayersProps) => {
    const [grps, setGrps] = useState<number[]>(convertGroups(players.groups));

    const changePlayer = (pos: number) => {
        const clone = [...players.members];
        if (clone[pos] === 'human') clone[pos] = 'robot';
        else if (clone[pos] === 'robot') clone[pos] = 'robot2';
        else clone[pos] = 'human';
        setPlayers({
            ...players,
            members: clone,
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
                players.members &&
                players.members.map((_, index) => {
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
                            type={players.members[index]}
                            group={grps[index]}
                            posInfo={mapInfo && mapInfo[index]}
                            changePlayer={() => changePlayer(index)}
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
