import Logo from '../../assets/images/logo.png';
import { formatMessageForHTML } from '../../utils/Utils';
import MessagingCodeBlock from '../Utils/MessagingCodeBlock';

const ChatMessaging = (props) => {
    return (
        <div className='messaging-wrapper'>
            {props.conversations.map((conversation, index) => (
                <span key={index} className={`messaging-wrapper-message ${(conversation.IsMine ? 'my-message' : 'bot-message')}`}>
                    {!conversation.IsMine &&
                        <img className='ai-message-logo' src={Logo} />
                    }

                    <MessagingCodeBlock message={conversation.Message} />
                </span>
            ))}

            {(props.incommingMessage || props.waitingResponse) &&
                <span className={`messaging-wrapper-message bot-message ${(props.isGeneratingResponse ? 'glow-border' : '')}`}>
                    <img className='ai-message-logo' src={Logo} />

                    {props.waitingResponse &&
                        <span className='loading-wrapper'>
                            <span className="loading"></span>
                            <span className="loading"></span>
                            <span className="loading"></span>
                        </span>
                    }

                    <MessagingCodeBlock message={props.incommingMessage} />
                </span>  
            }

        </div>
    )
}

export default ChatMessaging;
