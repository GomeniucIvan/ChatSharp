import LeftNavigation from './components/LeftNavigation';
import AppRoutes from './AppRoutes';
import { Route, Routes, useLocation, useMatch } from 'react-router-dom';
import { useEffect, useState } from 'react';
import './assets/public.scss';
import { get } from './utils/HttpClient';
import { setSessions } from './components/Utils/redux/sessionsSlice';
import { useDispatch } from 'react-redux';
import { isMatchGuid } from './utils/Utils';

const App = () => {
    const location = useLocation();
    const match = useMatch('/:guid');
    const [themeMode, setThemeMode] = useState(localStorage.getItem('themeMode') || 'light-mode');
    const [isConversationPage, setIsConversationPage] = useState(isMatchGuid(match));
    const dispatch = useDispatch();

    useEffect(() => {
        const PopulateComponent = async () => {
            const startupResponse = await get(`StartupChatData`);

            if (startupResponse.IsValid) {
                const startupData = startupResponse.Data;

                setSessions(startupData.Sessions);
                dispatch(setSessions(startupData.Sessions));
            }
        }

        PopulateComponent();
    }, []);

    useEffect(() => {
        setIsConversationPage(isMatchGuid(match));

        if (isMatchGuid(match)) {
            window.scrollTo(0, document.body.scrollHeight);
        }
    }, [location]);

    useEffect(() => {
        document.body.className = themeMode;
        localStorage.setItem('themeMode', themeMode);
    }, [themeMode]);

    const toggleThemeMode = () => {
        if (themeMode === 'light-mode') {
            setThemeMode('dark-mode');
        } else {
            setThemeMode('light-mode');
        }
    };

    return (
        <main className={`main-wrapper`}>
            <div className="left-nav">
                <LeftNavigation toggleThemeMode={toggleThemeMode} />
            </div>

            <div className={`inner-wrapper${isConversationPage ? ' conversation-inner-wrapper' : ''}`}>
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
