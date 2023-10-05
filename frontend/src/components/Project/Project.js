import {Link, useParams} from "react-router-dom";
import React, {useEffect, useState} from "react";
import {createAPIEndpoint, ENDPOINTS} from "../../api/axios";
import "./Project.css"
import Select from "react-select";

export const Project = (props) => {
    const {createTask} = props;
    const {id} = useParams();
    const [errors, setErrors] = useState({})
    const [project, setProject] = useState({})
    const [taskData, setTaskData] = useState({
        description: "",
        creatorId: sessionStorage.getItem("userId"),
        projectId: id,
        taskMembers: [],
    });

    useEffect(() => {
        const headers = {"Authorization": "Bearer " + sessionStorage.getItem("token")}
        createAPIEndpoint(ENDPOINTS.projects,headers).fetchById(id)
            .then(res => {
                setProject(res.data)
                console.log(res.data)
            })
            .catch(err => {
                console.log(err);
            });
    }, [id])

    const userOptions = project.projectMembers ? project.projectMembers.map((user) => ({
            value: user.id,
            label: user.userName,
        })) : [];

    const handleChange = (e) => {
        const {name, value} = e.target;
        setTaskData({...taskData, [name]: value});
    };

    const handleUserSelectChange = (selectedOptions) => {
        setTaskData({...taskData, taskMembers: selectedOptions})
    };

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

    const validate = () => {
        let temp = {}
        temp.description = taskData.description !== "";
        temp.taskMembers = taskData.taskMembers !== [];
        setErrors(temp)
        return Object.values(temp).every(x => x === true)
    }

    const resetForm = () => {
        setTaskData({
            description: "",
            creatorId: sessionStorage.getItem("userId"),
            projectId: id,
            taskMembers: [],
        })
        setErrors({})
        window.location.reload();
    }

    const handleSubmit = (e) => {
        e.preventDefault()
        if (validate()) {
            const data = new FormData()
            data.append("description", taskData.description)
            data.append("creatorId", taskData.creatorId)
            data.append("projectId", taskData.projectId)
            taskData.taskMembers.forEach((member) => {
                data.append("taskMembers", member.value);
            });
            createTask(data, resetForm)
        }
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
        <div className="container d-flex flex-column flex-grow-1 white-box-project pb-4 px-5 top-margin">
            <div className="my-4">
                <h2 className="dark-blue"><b>{project.title}</b></h2>
                <p className="light-blue">Started on {project.dateStarted}</p>
            </div>

            <h3 className="dark-blue">Project Members</h3>
            <div className="d-flex flex-wrap flex-row">
                {project.projectMembers && project.projectMembers.map((member) => (
                    <Link className="project-link" to={"/users/" + member.id} key={member.id}>
                        <div className="user-card" key={member.id}>
                            <img src={member.profilePictureSrc} className="user-avatar mx-auto" alt="Profile Avatar"/>
                            <p className="user-name">{member.userName}</p>
                        </div>
                    </Link>
                ))}
            </div>
            <div className="d-flex flex-row gap-4 mt-4">
                <div>
                    <h3 className="dark-blue">Tasks</h3>
                </div>
                <div>
                    <button className="btn pill-btn mx-auto" data-bs-toggle="modal"
                            data-bs-target="#staticBackdrop">
                        +Add Task
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
                                            <textarea
                                                className={"form-control input-border task-description" + applyErrorClass("description")}
                                                id="taskDescription"
                                                name="description"
                                                placeholder="Task Description"
                                                value={taskData.description}
                                                onChange={handleChange}
                                                rows="4"
                                                required
                                            />
                                            <label htmlFor="taskDescription" className="form-label">Task Description</label>
                                        </div>
                                        <div className="text-start my-3">
                                            <Select
                                                className={applyErrorClass("taskMembers")}
                                                options={userOptions}
                                                isMulti
                                                value={taskData.taskMembers}
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
            </div>
            <div className="d-flex flex-wrap flex-row py-1">
                {project.tasks && project.tasks.map((task) => (
                    <Link className="project-link" to={"/tasks/"+task.id} key={task.id}>
                        <div className="task-card" key={task.id} style={{
                            backgroundColor: getTaskStatusColor(task.status),
                        }}>
                            <p className="cream user-name">{task.description}</p>
                        </div>
                    </Link>
                ))}
            </div>
        </div>
    );
};