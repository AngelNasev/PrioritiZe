import {Link, useParams} from "react-router-dom";
import React, {useEffect, useState} from "react";
import {createAPIEndpoint, ENDPOINTS} from "../../api/axios";
import "./UserTasks.css"

export const UserTasks = (props) => {
    const {id} = useParams();
    const [userTasks, setUserTasks] = useState({})

    useEffect(() => {
        const headers = {"Authorization": "Bearer " + sessionStorage.getItem("token")}
        createAPIEndpoint(ENDPOINTS.userTasks,headers).fetchById(id)
            .then(res => {
                setUserTasks(res.data)
                console.log(res.data)
            })
            .catch(err => {
                console.log(err);
            });
    }, [id])

    const getTaskStatusColor = (status) => {
        switch (status) {
            case 0:
                return "#ED2939";
            case 1:
                return "#F5DE10";
            case 2:
                return "#29AB87";
            default:
                return "#FFFFFF";
        }
    };

    return (
        <div className="container d-flex justify-content-between flex-column flex-grow-1 white-box-task pb-4 px-5 top-margin">
            <div className="text-center pt-3 pb-5 dark-blue">
                <h2 className="home-title"><b>{userTasks.userName}'s Tasks</b></h2>
            </div>
            <div className="d-flex justify-content-between flex-row flex-grow-1">
                <div className="w-33 text-center">
                    <h4 className="dark-blue">Pending Tasks</h4>
                    <div className="d-flex flex-wrap flex-column py-1">
                        {userTasks.pendingTasks && userTasks.pendingTasks.map((task) => (
                            <Link className="project-link" to={"/tasks/"+task.id} key={task.id}>
                                <div className="user-task-card" key={task.id} style={{
                                    backgroundColor: getTaskStatusColor(task.status),
                                }}>
                                    <p className="cream user-name">{task.description}</p>
                                </div>
                            </Link>
                        ))}
                    </div>
                </div>
                <div className="w-33 text-center">
                    <h4 className="dark-blue">In Progress Tasks</h4>
                    <div className="d-flex flex-wrap flex-column py-1">
                        {userTasks.inProgressTasks && userTasks.inProgressTasks.map((task) => (
                            <Link className="project-link" to={"/tasks/"+task.id} key={task.id}>
                                <div className="user-task-card" key={task.id} style={{
                                    backgroundColor: getTaskStatusColor(task.status),
                                }}>
                                    <p className="cream user-name">{task.description}</p>
                                </div>
                            </Link>
                        ))}
                    </div>
                </div>
                <div className="w-33 text-center">
                    <div className="text-center">
                        <h4 className="dark-blue">Completed Tasks</h4>
                    </div>
                    <div className="d-flex flex-wrap flex-column py-1">
                        {userTasks.completedTasks && userTasks.completedTasks.map((task) => (
                            <Link className="project-link" to={"/tasks/"+task.id} key={task.id}>
                                <div className="user-task-card" key={task.id} style={{
                                    backgroundColor: getTaskStatusColor(task.status),
                                }}>
                                    <p className="cream user-name">{task.description}</p>
                                </div>
                            </Link>
                        ))}
                    </div>
                </div>
            </div>
        </div>
    )
}