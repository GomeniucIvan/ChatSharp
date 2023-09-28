import Navbar from './components/NavBar';
import AppRoutes from './AppRoutes';
import { Route, Routes } from 'react-router-dom';

import './assets/public.scss';

const App = () => {
    const styleMode = '';

    const handleNewConversation = () => {

    };

    const selectedConversation = () => {

    };

    return (
        <main
            className={`main-wrapper ${styleMode}`}
        >
            <div className="left-nav">
                <Navbar
                    selectedConversation={selectedConversation}
                    onNewConversation={handleNewConversation}
                />
            </div>

            <div className="inner-wrapper">
                <Routes>
                    {AppRoutes.map((route, index) => {
                        const { element, path, ...rest } = route;

                        return <Route
                            key={index}
                            path={path}
                            {...rest}
                            element={element} />;
                    })}
                </Routes>
            </div>
        </main>
    );
}

export default App;
