import Chat from "./components/Chat";
import Settings from "./components/Settings/Settings";

const AppRoutes = [
    {
        index: true,
        path: '/',
        element: <Chat />
    },
    {
        path: '/:guid',
        element: <Chat />
    },
    {
        path: '/settings',
        element: <Settings />
    },
]

export default AppRoutes;