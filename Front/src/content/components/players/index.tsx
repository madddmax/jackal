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
}

const Players = ({ players, setPlayers }: PlayersProps) => {
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

    const calcCount = (prev: number) => {
        if (prev == 4) return 1;
        else if (prev == 1) return 2;
        return 4;
    };

    return (
        <div className={cn(classes.settings, 'mx-auto')}>
            {players &&
                players.members &&
                players.members.map((_, index) => {
                    if (players.count < 4 && (index == 1 || index == 3)) {
                        return null;
                    }
                    if (players.count < 2 && index == 2) {
                        return null;
                    }

                    return (
                        <Player
                            key={`player-pos-${index}`}
                            position={index}
                            type={players.members[index]}
                            group={grps[index]}
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
                        count: calcCount(players.count),
                    })
                }
                style={{
                    cursor: 'pointer',
                    top: '110px',
                    left: '130px',
                    fontSize: '48px',
                    lineHeight: '48px',
                    textAlign: 'center',
                }}
            >
                {players.count}
            </div>
        </div>
    );
};

export default Players;
