import { Col, Container, Row } from 'react-bootstrap';

import classes from './gamedocuments.module.less';

const GameDocuments = () => {
    return (
        <Container>
            <Row className="justify-content-center">
                <Col lg className="g-lg-2">
                    <div className={classes.gamedocuments}>Раздел находится в разработке</div>
                </Col>
            </Row>
        </Container>
    );
};

export default GameDocuments;
