import Cell from './cell';
import './index.css'

function Map() {

    const cols = [0,1,2,3,4,5,6,7,8,9,10,11,12];
    const rows = [0,1,2,3,4,5,6,7,8,9,10,11,12];

    return (
      <div className='map'>
        {rows.map((row) => (
          <div className='map-row'>
            {cols.map((col) => <div className='map-cell'><Cell col={col} row={row} /></div>)}
          </div>
        ))}
      </div>  
    );
  }
  
  export default Map;