import React, {useEffect, useState} from "react";
import {createAPIEndpoint, ENDPOINTS} from "../../api/axios";
import "./Home.css"
import Select from 'react-select';
import {Link} from "react-router-dom";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faX} from "@fortawesome/free-solid-svg-icons";

export const Home = (props) => {
    const {createProject, deleteProject} = props
    const [errors, setErrors] = useState({})
    const [userProjects, setUserProjects] = useState([]);
    const [allUsers, setAllUsers] = useState([]);
    const [userProfile, setUserProfile] = useState({});
    const [isProjectManager, setIsProjectManager] = useState(false);
    const users = allUsers.filter(user => user.id !== sessionStorage.getItem("userId"));
    const [projectData, setProjectData] = useState({
        title: "",
        projectMembers: [],
    });

    const jobTitles = {
        0: "Developer",
        1: "Tester",
        2: "Designer",
        3: "Business Analyst",
        4: "Project Manager",
        5: "Other"
    }

    const userOptions = users.map(user => ({
        value: user.id,
        label: user.userName,
    }));

    useEffect(() => {
        const headers = {"Authorization": "Bearer " + sessionStorage.getItem("token")}
        createAPIEndpoint(ENDPOINTS.userProjects, headers).fetchById(sessionStorage.getItem("userId"))
            .then(res => {
                setUserProjects(res.data)
            })
            .catch(err => {
                console.log(err);
            });

        createAPIEndpoint(ENDPOINTS.users, headers).fetch()
            .then(res => {
                setAllUsers(res.data)
            })
            .catch(err => {
                console.log(err);
            });

        createAPIEndpoint(ENDPOINTS.users, headers).fetchById(sessionStorage.getItem("userId"))
            .then(res => {
                setUserProfile(res.data)
                setIsProjectManager(res.data.jobTitle === 4)
            })
            .catch(err => {
                console.log(err);
            })
    }, [])

    const handleChange = (e) => {
        const {name, value} = e.target;
        setProjectData({...projectData, [name]: value});
    };

    const handleUserSelectChange = (selectedOptions) => {
        setProjectData({...projectData, projectMembers: selectedOptions})
    };

    const validate = () => {
        let temp = {}
        temp.title = projectData.title !== "";
        setErrors(temp)
        return Object.values(temp).every(x => x === true)
    }

    const resetForm = () => {
        setProjectData({
            title: "",
            projectMembers: [],
        })
        setErrors({})
        window.location.reload()
    }

    const handleSubmit = (e) => {
        e.preventDefault()
        if (validate()) {
            const data = new FormData()
            data.append("title", projectData.title)
            projectData.projectMembers.forEach((member) => {
                data.append("projectMembers", member.value);
            });
            data.append("projectMembers", sessionStorage.getItem("userId"));
            createProject(data, resetForm)
        }
    }

    const handleDeleteClick = (e, projectId) => {
        e.preventDefault();
        delProject(projectId);
    };
    const delProject = (id) => {
        deleteProject(id)
    }

    const customStyles = {
        control: (provided, state) => ({
            ...provided,
            border: '2px solid #5F9DF7',
            borderRadius: '0.375rem',
            ':focus, :active': {
                boxShadow: '0 0 0 0.25rem rgba(23, 70, 162,0.25) !important',
                border: '2px solid #1746A2'
            },
        }),
    };

    const applyErrorClass = (field) => ((field in errors && errors[field] === false) ? " invalid" : "")

    return (
        <div>
            <div className="d-flex justify-content-between align-items-top gap-4 top-margin">
                <div className="ms-2" style={{width: 25 + "%"}}>
                    <div className="card text-center">
                        <div className="card-body">
                            <img src={userProfile.profilePictureSrc} alt={userProfile.userName}
                                 className="avatar"/>
                            <h4 className="card-title dark-blue">{userProfile.userName}</h4>
                            <h6 className="card-text dark-blue">{jobTitles[userProfile.jobTitle]}</h6>
                            <div className="my-5">
                                {isProjectManager &&
                                    <div>
                                        <button className="btn pill-btn mx-auto" data-bs-toggle="modal"
                                                data-bs-target="#staticBackdrop">
                                            + Add Project
                                        </button>
                                        <div className="modal fade" id="staticBackdrop" data-bs-backdrop="static"
                                             data-bs-keyboard="false" tabIndex="-1"
                                             aria-labelledby="staticBackdropLabel" aria-hidden="true">
                                            <div className="modal-dialog" role="document">
                                                <div className="modal-content">
                                                    <div className="modal-header">
                                                        <h3 className="modal-title fs-5 dark-blue"
                                                            id="staticBackdropLabel">Create a new
                                                            Project</h3>
                                                        <button type="button" className="btn-close"
                                                                data-bs-dismiss="modal" aria-label="Close"></button>
                                                    </div>
                                                    <form onSubmit={handleSubmit}>
                                                        <div className="modal-body">
                                                            <div className="form-floating my-2">
                                                                <input
                                                                    type="text"
                                                                    className={"form-control input-border" + applyErrorClass("title")}
                                                                    id="projectTitle"
                                                                    name="title"
                                                                    placeholder="Project Title"
                                                                    value={projectData.title}
                                                                    onChange={handleChange}
                                                                    required
                                                                />
                                                                <label htmlFor="projectTitle" className="form-label">Project
                                                                    Title</label>
                                                            </div>
                                                            <div className="text-start my-3">
                                                                <Select
                                                                    options={userOptions}
                                                                    isMulti
                                                                    value={projectData.projectMembers}
                                                                    styles={customStyles}
                                                                    placeholder="Select Members"
                                                                    onChange={handleUserSelectChange}
                                                                />
                                                            </div>
                                                        </div>
                                                        <div className="modal-footer">
                                                            <button type="submit" className="btn btn-primary">
                                                                Save
                                                            </button>
                                                            <button type="button" className="btn btn-danger"
                                                                    data-bs-dismiss="modal">Close
                                                            </button>
                                                        </div>
                                                    </form>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                }
                                {!isProjectManager &&
                                    <p>&nbsp;</p>
                                }
                            </div>
                        </div>
                    </div>
                    <div className="half-circle">
                        <div className="band"></div>
                        <div className="band"></div>
                        <div className="band"></div>
                    </div>
                </div>
                <div className="" style={{width: 45 + "%"}}>
                    <h3 className="dark-blue home-title">Projects:</h3>
                    {userProjects &&
                        <div>
                            {userProjects.map((project) => (
                                <Link className="project-link" to={"/projects/" + project.id} key={project.id}>
                                    <div className="project-card p-3 mb-3">
                                        <div className="d-flex justify-content-between align-items-center">
                                            <div>
                                                <h5 className="dark-blue">{project.title}</h5>
                                            </div>
                                            <div>
                                                <button className="btn"
                                                        onClick={(e) => handleDeleteClick(e, project.id)}>
                                                    <FontAwesomeIcon icon={faX} size="1x" className="text-danger"/>
                                                </button>
                                            </div>
                                        </div>
                                        <p className="light-blue">{project.dateStarted}</p>
                                        <h6 className="dark-blue">Project Members:</h6>
                                        <div className="user-list">
                                            {project.projectMembers.slice(0, 6).map((projectMember) => (
                                                <div key={projectMember.userName}>
                                                    <img
                                                        src={projectMember.profilePictureSrc}
                                                        alt={projectMember.userName}
                                                        className="user-avatar"
                                                    />
                                                </div>
                                            ))}
                                            {project.projectMembers.length > 6 && (
                                                <div key="others">
                                                    <img src="/img/more.jpg" alt="And Others" className="user-avatar"/>
                                                </div>
                                            )}
                                        </div>
                                    </div>
                                </Link>
                            ))}
                        </div>
                    }
                </div>
                <div className="me-2" style={{width: 18 + "%"}}>
                    <div className="list">
                        <div className="px-3 py-3">
                            <h4 className="dark-blue home-title"><b>Users:</b></h4>
                        </div>
                        <div className="d-flex justify-content-center flex-column flex-wrap px-3 pb-5">
                            {users.map(user => (
                                <Link className="user-link" to={"/users/" + user.id} key={user.id}>
                                    <div
                                        className="user-row d-flex justify-content-start align-items-center dark-blue mx-auto gap-2">
                                        <img src={user.profilePictureSrc} alt={user.userName}
                                             className="user-avatar"/>
                                        <p className="user-name">{user.userName}</p>
                                    </div>
                                </Link>
                            ))}
                        </div>
                    </div>
                </div>
            </div>
        </div>
    )
}