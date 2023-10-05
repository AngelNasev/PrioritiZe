import React, {useEffect, useState} from "react";
import {useParams} from "react-router-dom";
import {createAPIEndpoint, ENDPOINTS} from "../../api/axios";
import "./User.css"
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faChartSimple} from "@fortawesome/free-solid-svg-icons";

export const User = (props) => {
    const {id} = useParams();
    const [user, setUser] = useState(null);

    useEffect(() => {
        const headers = {"Authorization": "Bearer " + sessionStorage.getItem("token")}
        createAPIEndpoint(ENDPOINTS.userProfile, headers).fetchById(id)
            .then(res => {
                setUser(res.data)
                console.log(res.data)
            })
            .catch(err => {
                console.log(err);
            });
    }, [id])

    const jobTitles = {
        0: "Developer",
        1: "Tester",
        2: "Designer",
        3: "Business Analyst",
        4: "Project Manager",
        5: "Other"
    }

    return (
        <div className="container top-margin">
            {user &&
                <div className="d-flex flex-row justify-content-center align-items-top gap-5 pt-5">
                    <div style={{width: 30 + "%"}}>
                        <div className="card text-center p-4">
                            <div className="card-body">
                                <img src={user.profilePictureSrc} alt={user.userName}
                                     className="avatar"/>
                                <h4 className="card-title dark-blue">{user.userName}</h4>
                                <h6 className="card-text dark-blue">{jobTitles[user.jobTitle]}</h6>
                            </div>
                        </div>
                        <div className="half-circle">
                            <div className="band"></div>
                            <div className="band"></div>
                            <div className="band"></div>
                        </div>
                    </div>
                    <div style={{width: 40 + "%"}}>
                        <div className="card mb-4 p-2">
                            <div className="card-body">
                                <h4 className="card-title dark-blue">First Name: <span className="orange">{user.firstName}</span></h4>
                                <h4 className="card-title dark-blue">Last Name: <span className="orange">{user.lastName}</span></h4>
                            </div>
                        </div>
                        <div className="card p-2">
                            <div className="card-body">
                                <h4 className="card-title dark-blue"><FontAwesomeIcon icon={faChartSimple}/> <b>Statistics</b></h4>
                                <h5 className="card-text dark-blue">Member of {user.numProjects} projects</h5>
                                <h5 className="card-text dark-blue">Assigned to {user.numTasks} tasks</h5>
                                <h5 className="card-text dark-blue">Written {user.numComments} comments</h5>
                            </div>
                        </div>
                        <div className="half-circle">
                            <div className="band"></div>
                            <div className="band"></div>
                            <div className="band"></div>
                        </div>
                    </div>
                </div>
            }
        </div>
    )
}