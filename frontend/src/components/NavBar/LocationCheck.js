import {useLocation} from "react-router-dom";

export const LocationCheck = ({children}) => {
    const location = useLocation();

    const isRegisterOrLogin = location.pathname === '/register' || location.pathname === '/login';

    if (!isRegisterOrLogin) {
        return (
            <div>
                {children}
            </div>
        )
    }
}