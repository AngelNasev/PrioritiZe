import "./NavBar.css"
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faRightFromBracket, faUser} from "@fortawesome/free-solid-svg-icons";
export const NavBar = (props) => {
    const {onLogout} = props

    return (
        <nav className="navbar fixed-top navbar-expand-lg nav-background">
            <div className="container-fluid">
                <a className="navbar-brand nav-link dark-blue prioritize" href="/home">Prioriti<span
                    className="orange prioritize">Z</span>e</a>
                <div className="collapse navbar-collapse justify-content-center" id="navbarNav-1">
                    <ul className="navbar-nav">
                        <li className="nav-item">
                            <a className="nav-link dark-blue" href="/home"><b>Home</b></a>
                        </li>
                        <li className="nav-item">
                            <a className="nav-link dark-blue" href={"/user-tasks/"+sessionStorage.getItem("userId")}><b>Tasks</b></a>
                        </li>
                        <li className="nav-item">
                            <a className="nav-link dark-blue" href="/about"><b>About Us</b></a>
                        </li>
                    </ul>
                </div>
                <div className="pe-5 d-flex justify-content-center align-items-center">
                    <div className="me-3">
                        <a href={"/users/"+sessionStorage.getItem("userId")}>
                            <FontAwesomeIcon icon={faUser} size="2x" style={{color: "#1746A2",}} className="nav-link"/>
                        </a>
                    </div>
                    {(sessionStorage.getItem("userId") != null) &&
                        <div>
                            <a href="#" onClick={onLogout}>
                                <FontAwesomeIcon icon={faRightFromBracket} size="2x" style={{color: "#1746A2",}} className="nav-link"/>
                            </a>
                        </div>
                    }
                </div>
            </div>
        </nav>
    )
}