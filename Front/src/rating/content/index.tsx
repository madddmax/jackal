import { Col, Container, Row, Tab, Tabs } from 'react-bootstrap';
import { useSelector } from 'react-redux';

import { getLeaders, getUsersOnline } from '../redux/ratingSlice';
import classes from './rating.module.less';
import Leaderboard from '/rating/content/components/leaderboard';

const Rating = () => {
    const leaders = useSelector(getLeaders);
    const usersOnline = useSelector(getUsersOnline);

    return (
        <Container>
            <Row className="justify-content-center">
                <Col className="g-lg-2">
                    <div className={classes.leaderboard}>
                        <Tabs defaultActiveKey="leaders" id="leaderboard-tab" className="mb-3">
                            <Tab eventKey="leaders" title="Одиночный" style={{ overflowX: 'auto' }}>
                                <Leaderboard items={leaders.localLeaders} usersOnline={usersOnline} />
                            </Tab>
                            <Tab eventKey="netleaders" title="Командный">
                                <Leaderboard items={leaders.netLeaders} usersOnline={usersOnline} />
                            </Tab>
                            <Tab eventKey="botleaders" title="Роботы">
                                <Leaderboard items={leaders.botLeaders} usersOnline={usersOnline} />
                            </Tab>
                        </Tabs>
                    </div>
                </Col>
            </Row>
        </Container>
    );
};

export default Rating;
