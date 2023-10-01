import Logo from '../assets/images/logo.png';
import { NavLink, useLocation, useMatch } from 'react-router-dom';
import { useEffect } from 'react';
import { useSelector } from 'react-redux';

const LeftNavigation = (props) => {
    const match = useMatch('/:guid');
    const location = useLocation();
    const sessions = useSelector(state => state.sessions);

    const setPageRoute = () => {
        let pageTitle = 'ChatSharp';
        if (match) {
            const guid = match.params.guid;

            const session = sessions.find(i => i.Guid === guid);
            if (session) {
                pageTitle = session.Name;
            }
        }
        document.title = pageTitle;
    }

    useEffect(() => {
        setPageRoute();
    }, [sessions]);

    useEffect(() => {
        setPageRoute();
    }, [location]);

    return (
        <>
            <div className="left-nav-header">
                <img src={Logo} style={{ width: '36px' }} />

                <NavLink to='/'>New Chat</NavLink>
            </div>

            <div className="left-nav-sessions">
                <ul>
                    {sessions.map((session, index) => (
                        <li key={index} className='left-nav-session'>
                            <NavLink className={``} to={`/${session.Guid}`}>
                                {session.Name}
                            </NavLink>
                        </li>
                    ))}
                </ul>
            </div>

            <div className='left-nav-footer'>
                <ul>
                    <li><a onClick={() => props.toggleThemeMode()}>Theme</a></li>
                    <li><NavLink to='/settings'>Settings</NavLink></li>
                </ul>
            </div>
        </>
    )
}

export default LeftNavigation;
