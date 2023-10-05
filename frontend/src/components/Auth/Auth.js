import {isAuthenticated} from "../../auth/isAuthenticated";
import {Navigate} from "react-router-dom";

export const Auth = ({children}) => {

    if (isAuthenticated()){
        return children;
    }
    else {
        return <Navigate to={"/login"}/>
    }
}