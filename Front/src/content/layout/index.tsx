import Container from 'react-bootstrap/Container';
import Row from 'react-bootstrap/Row';
import Col from 'react-bootstrap/Col';

import Pirates from './components/pirates';
import Map from './components/map';
import Controls from './components/controls';
import classes from  './layout.module.less';

function Layout() {

    return (
      <Container>
        <Row className='justify-content-center gap-1'>
          <Col xs={1} className={classes.pirates}><Pirates /></Col>
          <Col xs={7}><Map /></Col>
          <Col xs={3}><Controls /></Col>
        </Row>
      </Container>
    );
  }
  
  export default Layout;