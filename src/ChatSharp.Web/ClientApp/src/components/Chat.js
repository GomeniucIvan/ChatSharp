import { useEffect, useRef, useState } from "react";
import ChatDefault from "./Chat/ChatDefault";
import ChatMessaging from "./Chat/ChatMessaging";
import { post } from "../utils/HttpClient";
import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr';

const Chat = () => {
    const [footerHeight, setFooterHeight] = useState(0);
    const [isDefaultPage, setIsDefaultPage] = useState(true);
    const [conversations, setConversations] = useState([]);
    const [enteredMessage, setEnteredMessage] = useState('');
    const [incommingMessage, setIncommingMessage] = useState('');
    const [isGeneratingResponse, setIsGeneratingResponse] = useState(false);
    const [signalRConnection, setSignalRConnection] = useState(null);
    const [waitingResponse, setWaitingResponse] = useState(false);
    const footerRef = useRef(null);

    useEffect(() => {
        if (footerRef.current) {
            setFooterHeight(footerRef.current.offsetHeight);
        }     

        const connection = new HubConnectionBuilder()
            .withUrl("/chatSharpHub")
            .withAutomaticReconnect()
            .configureLogging(LogLevel.Trace)
            .build();

        setSignalRConnection(connection);

        connection.on(`OnMessageReceive`, (message) => {
            console.log(message);

            const incMsg = message;
            setWaitingResponse(false);
            setIncommingMessage(prevMessage => prevMessage + incMsg);
        });

        connection.start().then(function () {
            //handleSocketOnOpen();
        }).catch(function (err) {
            //handleDisconnect();
            return console.error(err.toString());
        });
    }, []);

    const handleKeyDown = (event) => {
        if (event.keyCode === 13 && !event.shiftKey) {
            event.preventDefault(); 
            handleTextareaSubmit(event);
        }
    };

    const handleTextareaSubmit = async (event) => {
        event.preventDefault();
        let enteredMessageToSubmit = enteredMessage;

        setEnteredMessage('');

        if (footerRef.current) {
            setIsDefaultPage(false);

            let conversationMessage = {
                isMine: true,
                message: enteredMessageToSubmit
            };

            setConversations(prevConversations => [...prevConversations, conversationMessage]);
            await sendMessage(enteredMessageToSubmit);
        }
    };

    const selectExample = async (selectedExampleMessage) => {
        let conversationMessage = {
            isMine: true,
            message: selectedExampleMessage
        };

        conversations.push(conversationMessage)
        setConversations(conversations);

        setIsDefaultPage(false);
        await sendMessage(enteredMessage);
    }

    const sendMessage = async (enteredMessage) => {
        setIsGeneratingResponse(true);
        setWaitingResponse(true);

        const postModel = {
            Message: enteredMessage
        };

        var result = await post('ConversationSendMessage', postModel);
        if (result.IsValid) {
            let conversationMessage = {
                isMine: false,
                message: result.Data
            };

            setConversations(prevConversations => [...prevConversations, conversationMessage]);
            setIsGeneratingResponse(false);
            setWaitingResponse(false);
            setIncommingMessage('');
        } else {
            setIsGeneratingResponse(false);
            setWaitingResponse(false);
            setIncommingMessage('');

            //pNotifyError todo
        }
    }

    return (
        <div className='chat-wrapper'>
            <div className={`chat-wrapper-body default-${(isDefaultPage ? 'true' : 'false')}`}>
                {isDefaultPage &&
                    <ChatDefault selectExample={selectExample} />
                }

                {!isDefaultPage &&
                    <ChatMessaging conversations={conversations}
                                   isGeneratingResponse={isGeneratingResponse}
                                   waitingResponse={waitingResponse}
                                   incommingMessage={incommingMessage} />
                }

                <div className='chat-wrapper-footer-fake' style={{ height: `${footerHeight}px` }}>

                </div>
            </div>

            <div className='chat-wrapper-footer' ref={footerRef}>
                <form onSubmit={handleTextareaSubmit}>
                    <textarea placeholder='Send a message'
                        value={enteredMessage}
                        onChange={(e) => setEnteredMessage(e.target.value)}
                        onKeyDown={handleKeyDown}>
                    </textarea>

                    <button type="submit" style={{ display: "none" }}>Submit</button>
                </form>
            </div>
        </div>
    )
}

export default Chat;
