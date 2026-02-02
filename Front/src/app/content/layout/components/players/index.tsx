import cn from 'classnames';
import { useRef, useState } from 'react';
import Image from 'react-bootstrap/Image';
import { PlacesType, Tooltip, TooltipRefProps } from 'react-tooltip';

import { PlayerInfo, PlayersInfo } from '../types';
import Player from './player';
import classes from './players.module.less';
import { Constants } from '/app/constants';

const convertGroups = (grps: string[]) => grps.map((gr) => Constants.groups.findIndex((it) => it.id == gr) || 0);
const deconvertGroups = (groups: number[]) => groups.map((num) => Constants.groups[num].id);
const tooltipPositions: PlacesType[] = ['bottom', 'left', 'top', 'right'];
const tooltipAnchors: string[] = ['#players_mode_cntrl', '#player_pos_3', '#player_pos_0', '#player_pos_1'];

export interface PlayersProps {
    players: PlayersInfo;
    allowedGamers: PlayerInfo[];
    setPlayers: (data: PlayersInfo) => void;
    mapInfo?: string[];
}

const Players = ({ players, allowedGamers, setPlayers, mapInfo }: PlayersProps) => {
    const [grps, setGrps] = useState<number[]>(convertGroups(players.groups));

    const actionsTooltip = useRef<TooltipRefProps>(null);

    const changeGamer = (pos: number) => {
        const clone = [...players.gamers];
        if (clone[pos].id + 1 >= allowedGamers.length) clone[pos] = allowedGamers[0];
        else clone[pos] = allowedGamers.find((it) => it.id === clone[pos].id + 1) ?? allowedGamers[0];
        setPlayers({
            ...players,
            gamers: clone,
        });
    };

    const changeGroup = (pos: number) => {
        actionsTooltip.current?.open({
            anchorSelect: tooltipAnchors[pos],
            place: tooltipPositions[pos],
            content: (
                <div className={classes.content}>
                    {Constants.groups.map((it, index) =>
                        grps.includes(index) ? (
                            <></>
                        ) : (
                            <Image
                                className={classes.icon}
                                roundedCircle
                                src={`/pictures/${it.id}/logo.png`}
                                onClick={() => {
                                    actionsTooltip.current?.close();
                                    const clone = [...grps];
                                    clone[pos] = index;
                                    setGrps(clone);
                                    setPlayers({
                                        ...players,
                                        groups: deconvertGroups(clone),
                                    });
                                }}
                            />
                        ),
                    )}
                </div>
            ),
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
                            changePlayer={() => changeGamer(index)}
                            changeGroup={() => changeGroup(index)}
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
            <Tooltip
                ref={actionsTooltip}
                className={classes.groupsTooltip}
                classNameArrow={classes.groupsTooltipArrow}
                style={{ backgroundColor: 'white', zIndex: 1000 }}
                clickable
                openEvents={{}}
                closeEvents={{ blur: true }}
                globalCloseEvents={{ clickOutsideAnchor: true, escape: true }}
            />
        </div>
    );
};

export default Players;
