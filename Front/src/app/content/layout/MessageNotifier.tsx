import { Toast, ToastContainer } from 'react-bootstrap';
import { useDispatch, useSelector } from 'react-redux';
import { ReduxState } from '../../../common/redux.types';
import { MessageInfo } from '../../../common/redux/commonSlice.types';
import { hideError, processError } from '../../../common/redux/commonSlice';

const MessageNotifier = () => {
    const dispatch = useDispatch();

    const error = useSelector<ReduxState, MessageInfo | undefined>((state) => state.common.message);

    return (
        <ToastContainer className="p-3" position={error?.isError ? 'bottom-end' : 'top-end'} style={{ zIndex: 5 }}>
            <Toast
                className="d-inline-block m-1"
                bg={error?.isError ? 'danger light' : 'info light'}
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
                <Toast.Body className="text-white">{error?.messageText || 'неизвестная ошибка'}</Toast.Body>
            </Toast>
        </ToastContainer>
    );
};

export default MessageNotifier;
