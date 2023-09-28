import Logo from '../assets/images/logo.png';
import { NavLink } from 'react-router-dom'

const NavBar = () => {
    return (
        <nav>
            <div className="left-nav-header">
                <img src={Logo} style={{ width: '36px'}} />

                <NavLink to='/'>New Chat</NavLink>
            </div>

            <div className="left-nav-chats">

            </div>
        </nav>
    )
}

export default NavBar;
