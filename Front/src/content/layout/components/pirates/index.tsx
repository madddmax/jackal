import Image from 'react-bootstrap/Image';
import './pirates.css'

function Pirates() {

    return (
      <>
        <Image src="/pictures/smallet.jpg" roundedCircle className='photo float-end' />
        <Image src="/pictures/gokins.jpg" roundedCircle className='photo float-end' />
        <Image src="/pictures/livsi.jpg" roundedCircle className='photo float-end' />
      </>  
    );
  }
  
  export default Pirates;