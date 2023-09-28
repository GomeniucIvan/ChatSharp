import React from 'react';
import ReactDOM from 'react-dom/client';
import Startup from './Startup';
import reportWebVitals from './reportWebVitals';
import { BrowserRouter } from 'react-router-dom';

const baseUrl = document.getElementsByTagName('base')[0].getAttribute('href');
const root = ReactDOM.createRoot(document.getElementById('root'));

root.render(
    <BrowserRouter basename={baseUrl}>
        <Startup />
    </BrowserRouter>);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
