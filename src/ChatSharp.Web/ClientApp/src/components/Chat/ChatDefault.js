import { useState } from "react";

const ChatDefault = (props) => {
    const [modelName, setModelName] = useState('llama-2-13b-chat.Q4_K_M.gguf');

    const examples = [
        "What is c#",
        "Assist in a task",
        "Code a snake game",
    ]

    return (
        <div className='chat-default-wrapper'>
            <div className='chat-default-header'>
                <div className='chat-default-models-wrapper-title'>
                    <h1>
                        ChatSharp
                    </h1>

                    <span>Where conversations get sharper</span>
                </div>

                <div className='chat-default-models-wrapper'>
                    <label className='chat-default-models-label'>
                        Current Model
                    </label>

                    <label className='chat-default-models-name'>
                        {modelName}
                    </label>
                </div>
            </div>
            <div className='chat-default-examples'>
                {examples.map((example, index) => (
                    <button key={index} className='chat-default-examples-item' onClick={() => props.selectExample(example)}>
                        {example}
                    </button>
                ))}
            </div>
        </div>
    )
}

export default ChatDefault;
