import Logo from '../../assets/images/logo.png';

const ChatMessaging = (props) => {
    return (
        <div className='messaging-wrapper'>
            {props.conversations.map((conversation, index) => (
                <span key={index} className={`messaging-wrapper-message ${(conversation.isMine ? 'my-message' : 'bot-message')}`}>
                    {!conversation.isMine &&
                        <img className='ai-message-logo' src={Logo} />
                    }
                    {conversation.message}
                </span>
            ))}
        </div>
    )
}

export default ChatMessaging;
