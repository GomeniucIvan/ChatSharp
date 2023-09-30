import Logo from '../assets/images/logo.png';
import { NavLink } from 'react-router-dom';

const LeftNavigation = (props) => {
    return (
        <>
            <div className="left-nav-header">
                <img src={Logo} style={{ width: '36px' }} />

                <NavLink to='/'>New Chat</NavLink>
            </div>

            <div className="left-nav-chats">

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
