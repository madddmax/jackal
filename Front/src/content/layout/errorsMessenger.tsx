import { Toast, ToastContainer } from 'react-bootstrap';
import { useDispatch, useSelector } from 'react-redux';
import { ReduxState } from '/redux/types';
import { ErrorInfo } from '../../redux/commonSlice.types';
import { hideError, processError } from '/redux/commonSlice';

const ErrorsMessenger = () => {
    const dispatch = useDispatch();

    const error = useSelector<ReduxState, ErrorInfo | undefined>((state) => state.common.error);

    return (
        <ToastContainer className="p-3" position="bottom-end" style={{ zIndex: 5 }}>
            <Toast
                className="d-inline-block m-1"
                bg="danger light"
                show={!!error}
                onClose={() => {
                    dispatch(hideError());
                    setTimeout(() => dispatch(processError()), 200);
                }}
                delay={3000}
                autohide
            >
                <Toast.Header>
                    <img src="holder.js/20x20?text=%20" className="rounded me-2" alt="" />
                    <strong className="me-auto">Bootstrap</strong>
                    <small>11 mins ago</small>
                </Toast.Header>
                <Toast.Body className="text-white">{error?.errorMessage || 'неизвестная ошибка'}</Toast.Body>
            </Toast>
        </ToastContainer>
    );
};

export default ErrorsMessenger;
