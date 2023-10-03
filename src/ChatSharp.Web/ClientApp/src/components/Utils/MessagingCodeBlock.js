import React from 'react';
import { Prism as SyntaxHighlighter } from 'react-syntax-highlighter';
import { vscDarkPlus } from 'react-syntax-highlighter/dist/esm/styles/prism'; 
import { formatMessageForHTML } from '../../utils/Utils';


const MessagingCodeBlock = (props) => {
    const lines = props.message.split(/(?:\r\n|\r|\n|<br\s*\/?>)/g);

    let inCodeBlock = false;
    let codeLines = [];
    const output = [];

    lines.forEach((line, index) => {
        if (line.trim() === '```') {
            if (inCodeBlock && codeLines.length) {
                
                output.push(
                    <SyntaxHighlighter key={index} language="csharp" style={vscDarkPlus}>
                        {codeLines.join('\n')}
                    </SyntaxHighlighter>
                );
                codeLines = []; 
            }
            inCodeBlock = !inCodeBlock;
            return;
        }

        if (inCodeBlock) {
            codeLines.push(line);
        } else {
            output.push(
                <div key={index} dangerouslySetInnerHTML={{ __html: formatMessageForHTML(line) }} />
            );
        }
    });

   
    if (inCodeBlock && codeLines.length) {
        output.push(
            <SyntaxHighlighter key={lines.length} language="csharp" style={vscDarkPlus}>
                {codeLines.join('\n')}
            </SyntaxHighlighter>
        );
    }

    return <div>{output}</div>;
}

export default MessagingCodeBlock;
