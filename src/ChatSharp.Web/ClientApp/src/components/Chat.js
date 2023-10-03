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
    const [waitingResponse, setWaitingResponse] = useState(false);
    const [workingGuid, setWorkingGuid] = useState(guid);
    const [abortController, setAbortController] = useState(null);
    const footerRef = useRef(null);
    const location = useLocation();
    const dispatch = useDispatch();
    const navigate = useNavigate();
    const match = useMatch('/:guid');
    const chatWrapperRef = useRef(null);

    useEffect(() => {
        setIsDefaultPage(isNullOrEmpty(guid));
        loadSessionHistory();
        setIncommingMessage('');      
    }, [location]);

    useEffect(() => {
        if (footerRef.current) {
            setFooterHeight(footerRef.current.offsetHeight);
        }     

        const connection = new HubConnectionBuilder()
            .withUrl("/chatSharpHub")
            .withAutomaticReconnect()
            .configureLogging(LogLevel.None)
            .build();

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

        return () => {
            connection.stop();
        };
    }, []);

    const loadSessionHistory = async (guidRouteParam) => {
        setConversations([]);

        if (isNullOrEmpty(guidRouteParam)) {
            if (match) {
                guidRouteParam = match.params.guid;
            }
        }

        if (!isNullOrEmpty(guidRouteParam)) {
            const sessionResponse = await get(`ConversationLoadSessionHistory?guid=${guidRouteParam}`);
            if (sessionResponse.IsValid) {
                setConversations(sessionResponse.Data);
                setWorkingGuid(guidRouteParam);

                setTimeout(function () {
                    if (chatWrapperRef.current) {
                        chatWrapperRef.current.scrollTop = chatWrapperRef.current.scrollHeight;
                    }
                }, 100)
            }
        } else {
            setWorkingGuid(null);
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

        setConversations(prevConversations => [...prevConversations, conversationMessage]);
        setWorkingGuid(null);
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

        const cancellationController = new AbortController();
        setAbortController(cancellationController);

        try {
            var result = await post('ConversationSendMessage', postModel, cancellationController.signal);

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
        } catch (error) {
            if (error.name === 'AbortError') {
                console.log("Request was cancelled");
            }
            // handle other potential errors here
        }
    }

    const cancelRequestGeneration = () => {
        if (abortController) {
            abortController.abort();

            setWaitingResponse(false);
            let conversationMessage = {
                IsMine: false,
                Message: incommingMessage
            };
            setConversations(prevConversations => [...prevConversations, conversationMessage]);

            setIncommingMessage('');
        }
    };

    return (
        <div className={`chat-wrapper default-wrapper-${(isDefaultPage ? 'true' : 'false')}`}>
            <div className={`chat-wrapper-body default-${(isDefaultPage ? 'true' : 'false')}`} ref={chatWrapperRef}>
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
