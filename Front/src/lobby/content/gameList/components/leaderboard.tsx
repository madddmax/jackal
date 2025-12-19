import { Table } from 'react-bootstrap';

import { LeaderBoardItemResponse } from '/lobby/types/lobbySaga';

interface LeaderboardProps {
    items: LeaderBoardItemResponse[] | undefined;
}

const Leaderboard = ({ items }: LeaderboardProps) => {
    let ratingNumber = 1;

    return (
        <Table striped>
            <thead>
                <tr>
                    <th>#</th>
                    <th style={{ width: '100px' }}>Логин</th>
                    <th style={{ width: '100px' }}>Ранг</th>
                    <th>Игры сегодня</th>
                    <th>Игры недели</th>
                    <th>Игры месяца</th>
                    <th>Победы&nbsp;- Игры</th>
                    <th>Монеты</th>
                </tr>
            </thead>
            <tbody>
                {items &&
                    items.map((it) => (
                        <tr key={`leader_${ratingNumber}`}>
                            <td>{ratingNumber++}</td>
                            <td>{it.playerName}</td>
                            <td>
                                <img src={`ranks/${it.rank}.webp`} alt={it.rank} />
                            </td>
                            <td>
                                {it.winCountToday} - {it.gamesCountToday}
                            </td>
                            <td>
                                {it.winCountThisWeek} - {it.gamesCountThisWeek}
                            </td>
                            <td>
                                {it.winCountThisMonth} - {it.gamesCountThisMonth}
                            </td>
                            <td>
                                {it.totalWin} - {it.gamesCountTotal}
                            </td>
                            <td>{it.totalCoins}</td>
                        </tr>
                    ))}
            </tbody>
        </Table>
    );
};

export default Leaderboard;
