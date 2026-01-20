import { Table } from 'react-bootstrap';
import { PiDotFill } from 'react-icons/pi';

import { LeaderBoardItemResponse } from '/lobby/types/lobbySaga';

interface LeaderboardProps {
    items: LeaderBoardItemResponse[] | undefined;
    usersOnline: number[] | undefined;
}

const Leaderboard = ({ items }: LeaderboardProps) => {
    let ratingNumber = 1;

    return (
        <Table striped>
            <thead>
                <tr>
                    <th>#</th>
                    <th>On</th>
                    <th style={{ width: '100px' }}>Логин</th>
                    <th style={{ width: '100px' }}>Ранг</th>
                    <th>Игры&nbsp;сегодня</th>
                    <th>Игры&nbsp;недели</th>
                    <th>Игры&nbsp;месяца</th>
                    <th>Игры&nbsp;всего</th>
                    <th>Побед</th>
                    <th>Монет</th>
                </tr>
            </thead>
            <tbody>
                {items &&
                    items.map((it) => (
                        <tr key={`leader_${ratingNumber}`}>
                            <td>{ratingNumber++}</td>
                            <td>
                                <PiDotFill color="green" />
                            </td>
                            <td>{it.playerName}</td>
                            <td>
                                <img src={`ranks/${it.rank}.webp`} alt={it.rank} />
                            </td>
                            <td>
                                <span style={{ color: 'green' }}>{it.winCountToday}</span>&nbsp;-&nbsp;
                                <span style={{ color: 'red' }}>{it.loseCountToday}</span>
                            </td>
                            <td>
                                <span style={{ color: 'green' }}>{it.winCountThisWeek}</span>&nbsp;-&nbsp;
                                <span style={{ color: 'red' }}>{it.loseCountThisWeek}</span>
                            </td>
                            <td>
                                <span style={{ color: 'green' }}>{it.winCountThisMonth}</span>&nbsp;-&nbsp;
                                <span style={{ color: 'red' }}>{it.loseCountThisMonth}</span>
                            </td>
                            <td>
                                <span style={{ color: 'green' }}>{it.totalWin}</span>&nbsp;-&nbsp;
                                <span style={{ color: 'red' }}>{it.totalLose}</span>
                            </td>
                            <td>{it.winPercent.toFixed(2)}%</td>
                            <td>{it.totalCoins}</td>
                        </tr>
                    ))}
            </tbody>
        </Table>
    );
};

export default Leaderboard;
