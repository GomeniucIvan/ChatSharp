import { useEffect, useRef, useState } from "react";
import ChatDefault from "./Chat/ChatDefault";
import ChatMessaging from "./Chat/ChatMessaging";

const Chat = () => {
    const [footerHeight, setFooterHeight] = useState(0);
    const [isDefaultPage, setIsDefaultPage] = useState(true);
    const [conversations, setConversations] = useState([]);
    const [message, setMessage] = useState('');
    const footerRef = useRef(null);

    useEffect(() => {
        if (footerRef.current) {
            setFooterHeight(footerRef.current.offsetHeight);
        }
    }, []);

    const handleKeyDown = (event) => {
        if (event.keyCode === 13 && !event.shiftKey) {
            event.preventDefault(); 
            handleTextareaSubmit(event);
        }
    };

    const handleTextareaSubmit = (event) => {
        event.preventDefault();  // Prevent form submission and page reload

        if (footerRef.current) {
            setIsDefaultPage(false);

            let conversationMessage = {
                isMine: true,
                message: message
            };

            conversations.push(conversationMessage)
            setConversations(conversations);

            conversationMessage = {
                isMine: false,
                message: message
            };

            conversations.push(conversationMessage)
            setConversations(conversations);
        }

        setMessage('');
    };

    const selectExample = (example) => {

        let conversationMessage = {
            isMine: true,
            message: example
        };

        conversations.push(conversationMessage)
        setConversations(conversations);

        setIsDefaultPage(false);
    }

    return (
        <div className='chat-wrapper'>
            <div className={`chat-wrapper-body default-${(isDefaultPage ? 'true' : 'false')}`}>
                {isDefaultPage &&
                    <ChatDefault selectExample={selectExample} />
                }

                {!isDefaultPage &&
                    <ChatMessaging conversations={conversations} />
                }

                <div className='chat-wrapper-footer-fake' style={{ height: `${footerHeight}px` }}>

                </div>
            </div>

            <div className='chat-wrapper-footer' ref={footerRef}>
                <form onSubmit={handleTextareaSubmit}>
                    <textarea placeholder='Send a message'
                        value={message}
                        onChange={(e) => setMessage(e.target.value)}
                        onKeyDown={handleKeyDown}>
                    </textarea>

                    <button type="submit" style={{ display: "none" }}>Submit</button>
                </form>
            </div>
        </div>
    )
}

export default Chat;
