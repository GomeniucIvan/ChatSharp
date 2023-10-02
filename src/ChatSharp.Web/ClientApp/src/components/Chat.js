import { useEffect, useRef, useState } from "react";
import ChatDefault from "./Chat/ChatDefault";
import ChatMessaging from "./Chat/ChatMessaging";
import { get, post } from "../utils/HttpClient";
import { generateUUID, isNullOrEmpty } from "../utils/Utils";
import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import { useLocation, useParams, useNavigate, useMatch } from "react-router-dom";
import { useDispatch } from "react-redux";
import { addSession } from "./Utils/redux/sessionsSlice";

const Chat = () => {
    const { guid } = useParams();
    const [footerHeight, setFooterHeight] = useState(0);
    const [isDefaultPage, setIsDefaultPage] = useState(isNullOrEmpty(guid));
    const [conversations, setConversations] = useState([]);
    const [enteredMessage, setEnteredMessage] = useState('');
    const [incommingMessage, setIncommingMessage] = useState('');
    const [isGeneratingResponse, setIsGeneratingResponse] = useState(false);
    const [signalRConnection, setSignalRConnection] = useState(null);
    const [waitingResponse, setWaitingResponse] = useState(false);
    const [workingGuid, setWorkingGuid] = useState(guid);
    const footerRef = useRef(null);
    const location = useLocation();
    const dispatch = useDispatch();
    const navigate = useNavigate();
    const match = useMatch('/:guid');

    useEffect(() => {
        setIsDefaultPage(isNullOrEmpty(guid));
    }, [location]);

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
            const incMsg = message;
            setWaitingResponse(false);
            setIncommingMessage(prevMessage => prevMessage + incMsg);
        });

        connection.on(`OnNewSessionCreate`, (data) => {
            let newSession = {
                Name: data[0],
                Guid: data[1],
            };
            navigate(`/${data[1]}`);
            dispatch(addSession(newSession));
        });

        connection.start().then(function () {
            //handleSocketOnOpen();
        }).catch(function (err) {
            //handleDisconnect();
            return console.error(err.toString());
        });

        if (!isNullOrEmpty(guid)) {
            const PopulateComponent = async () => {
                await loadSessionHistory(guid); 
            }

            PopulateComponent();
        }
    }, []);

    useEffect(() => {
        loadSessionHistory();
    }, [location]);

    const loadSessionHistory = async (guidRouteParam) => {
        if (isNullOrEmpty(guidRouteParam)) {

            if (match) {
                guidRouteParam = match.params.guid;
            }
        }

        if (!isNullOrEmpty(guidRouteParam)) {
            const sessionResponse = await get(`ConversationLoadSessionHistory?guid=${guidRouteParam}`);

            if (sessionResponse.IsValid) {
                console.log(sessionResponse)
                setConversations(sessionResponse.Data);
            }
        }       
    }

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
                IsMine: true,
                Message: enteredMessageToSubmit
            };

            setConversations(prevConversations => [...prevConversations, conversationMessage]);
            await sendMessage(enteredMessageToSubmit);
        }
    };

    const selectExample = async (example) => {
        let conversationMessage = {
            IsMine: true,
            Message: example
        };

        conversations.push(conversationMessage)
        setConversations(conversations);

        setIsDefaultPage(false);
        await sendMessage(example);
    }

    const sendMessage = async (enteredMessage) => {
        setIsGeneratingResponse(true);
        setWaitingResponse(true);
        let workingGuidToPass = workingGuid;

        if (isNullOrEmpty(workingGuidToPass)) {
            workingGuidToPass = generateUUID();
            setWorkingGuid(workingGuidToPass);      
        }

        const postModel = {
            Message: enteredMessage,
            ModelGuid: workingGuidToPass
        };

        console.log(postModel)

        var result = await post('ConversationSendMessage', postModel);
        if (result.IsValid) {
            let conversationMessage = {
                IsMine: false,
                Message: result.Data
            };

            setConversations(prevConversations => [...prevConversations, conversationMessage]);
            setIsGeneratingResponse(false);
            setWaitingResponse(false);
            setIncommingMessage('');
        } else {
            setIsGeneratingResponse(false);
            setWaitingResponse(false);
            setIncommingMessage('');
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
