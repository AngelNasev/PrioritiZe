import './App.css';
import '@fortawesome/fontawesome-svg-core/styles.css';
import {Route, BrowserRouter as Router, Routes} from "react-router-dom";
import {Register} from "./components/Register/Register";
import {Login} from "./components/Login/Login";
import {createAPIEndpoint, ENDPOINTS} from "./api/axios";
import {Home} from "./components/Home/Home";
import {Auth} from "./components/Auth/Auth";
import {NavBar} from "./components/NavBar/NavBar";
import {LocationCheck} from "./components/NavBar/LocationCheck";
import {Project} from "./components/Project/Project";
import {Task} from "./components/Task/Task";
import {User} from "./components/User/User";
import {About} from "./components/About/About";
import {UserTasks} from "./components/UserTasks/UserTasks";

function App() {

    const register = (formData, onSuccess) => {
        createAPIEndpoint(ENDPOINTS.register).post(formData)
            .then(res => {
                const token = res.data.token;
                const id = res.data.id;
                sessionStorage.setItem("token", token);
                sessionStorage.setItem("userId", id);
                onSuccess(res);
            })
            .catch(err => {
                console.log(err)
            })
    }

    const logIn = (formData, onSuccess) => {
        createAPIEndpoint(ENDPOINTS.login).post(formData)
            .then(res => {
                const token = res.data.token;
                const id = res.data.id;
                sessionStorage.setItem("token", token);
                sessionStorage.setItem("userId", id);
                onSuccess(res);
            })
            .catch(err => {
                console.log(err);
            });
    }

    const handleLogout = () => {
        sessionStorage.removeItem("token");
        sessionStorage.removeItem("userId");
        window.location.reload()
    }

    const createProject = (formData, onSuccess) => {
        const headers = {"Authorization": "Bearer " + sessionStorage.getItem("token")}
        createAPIEndpoint(ENDPOINTS.projects, headers).post(formData)
            .then(res => {
                onSuccess(res)
            })
            .catch(err => {
                console.log(err);
            });
    }

    const createTask = (formData, onSuccess) => {
        const headers = {"Authorization": "Bearer " + sessionStorage.getItem("token")}
        createAPIEndpoint(ENDPOINTS.tasks, headers).post(formData)
            .then(res => {
                onSuccess(res)
            })
            .catch(err => {
                console.log(err);
            });
    }

    const deleteProject = (id) => {
        const headers = {"Authorization": "Bearer " + sessionStorage.getItem("token")}
        createAPIEndpoint(ENDPOINTS.projects, headers).delete(id)
            .then(res => {
                window.location.reload();
            })
            .catch(err => {
                console.log(err);
            });
    }

    const updateStatus = (id, formData, onSuccess) => {
        const headers = {"Authorization": "Bearer " + sessionStorage.getItem("token")}
        createAPIEndpoint(ENDPOINTS.tasks, headers).put(id,formData)
            .then(res => {
                onSuccess(res)
            })
            .catch(err => {
                console.log(err);
            });

    }

    const createComment = (formData, onSuccess) => {
        const headers = {"Authorization": "Bearer " + sessionStorage.getItem("token")}
        createAPIEndpoint(ENDPOINTS.comments, headers).post(formData)
            .then(res => {
                onSuccess(res)
            })
            .catch(err => {
                console.log(err);
            });
    }

    return (
        <Router>
            <LocationCheck>
                <NavBar onLogout={handleLogout}/>
            </LocationCheck>
            <div>
                <Routes>
                    <Route path={"/register"} element={<Register register={register}/>}/>
                    <Route path={"/login"} element={<Login logIn={logIn}/>}/>
                    <Route path={"/users/:id"} element={<Auth><User/></Auth>}/>
                    <Route path={"/user-tasks/:id"} element={<Auth><UserTasks/></Auth>}/>
                    <Route path={"/tasks/:id"} element={<Auth><Task createComment={createComment} updateStatus={updateStatus}/></Auth>}/>
                    <Route path={"/projects/:id"} element={<Auth><Project createTask={createTask}/></Auth>}/>
                    <Route path={"/about"} element={<Auth><About/></Auth>}/>
                    <Route path={"/home"}
                           element={<Auth><Home createProject={createProject} deleteProject={deleteProject}/></Auth>}/>
                    <Route path={"/"} element={<Auth><Home/></Auth>}/>
                </Routes>
            </div>
        </Router>
    )
}

export default App;
