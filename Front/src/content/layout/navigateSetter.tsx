import { useNavigate } from 'react-router-dom';
import { history } from '/app/global';

const NavigateSetter = () => {
    history.navigate = useNavigate();

    return null;
};

export default NavigateSetter;
