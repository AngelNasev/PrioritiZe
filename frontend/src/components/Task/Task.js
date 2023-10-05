import {Link, useParams} from "react-router-dom";
import React, {useEffect, useState} from "react";
import {createAPIEndpoint, ENDPOINTS} from "../../api/axios";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faGear, faSquare} from "@fortawesome/free-solid-svg-icons";
import "./Task.css"

export const Task = (props) => {
    const {createComment, updateStatus} = props
    const {id} = useParams();
    const [task, setTask] = useState({})
    const [errors, setErrors] = useState({})
    const [commentData, setCommentData] = useState({
        content: "",
        authorId: sessionStorage.getItem("userId"),
        taskId: id
    })
    const [selectedStatus, setSelectedStatus] = useState(task.status);


    useEffect(() => {
        const headers = {"Authorization": "Bearer " + sessionStorage.getItem("token")}
        createAPIEndpoint(ENDPOINTS.tasks,headers).fetchById(id)
            .then(res => {
                setTask(res.data)
                setSelectedStatus(res.data.status)
            })
            .catch(err => {
                console.log(err);
            });
    }, [id])

    const taskStatus = {
        0: "Pending",
        1: "In Progress",
        2: "Completed",
    }
    const statusOptions = [
        { key: 0, value: "Pending" },
        { key: 1, value: "In Progress" },
        { key: 2, value: "Completed" }
    ];

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

    const handleChange = (e) => {
        const {name, value} = e.target;
        setCommentData({...commentData, [name]: value});
    };

    const handleStatusChange = (e) => {
        const { value } = e.target;
        setSelectedStatus(value);
    };


    const validate = () => {
        let temp = {}
        temp.content = commentData.content !== "";
        setErrors(temp)
        return Object.values(temp).every(x => x === true)
    }

    const resetForm = () => {
        setCommentData({
            content: "",
            authorId: sessionStorage.getItem("userId"),
            taskId: id,
        })
        setErrors({})
        window.location.reload();
    }

    const resetStatusForm = () => {
        setSelectedStatus(task.status)
        window.location.reload();
    }

    const handleSubmit = (e) => {
        e.preventDefault()
        if (validate()) {
            const data = new FormData()
            data.append("content", commentData.content)
            data.append("authorId", commentData.authorId)
            data.append("taskId", commentData.taskId)
            createComment(data, resetForm)
        }
    }

    const changeStatus = (e) => {
        e.preventDefault()
        if (task.status !== undefined && selectedStatus !== task.status.toString()) {
            const data = new FormData()
            data.append("taskId", id)
            data.append("taskStatus", selectedStatus)
            updateStatus(id,data,resetStatusForm)
        }
        else {
            window.location.reload()
        }
    }

    const applyErrorClass = (field) => ((field in errors && errors[field] === false) ? " invalid" : "")

    return (
        <div className="container d-flex flex-column flex-grow-1 gap-3 white-box-task pb-4 px-5 top-margin">
            <div className="my-4">
                <h3 className="dark-blue">
                    <FontAwesomeIcon icon={faSquare} style={{color: getTaskStatusColor(task.status)}}/>
                    &nbsp;{task.description}</h3>
                <p className="light-blue">Created on <b>{task.createdAt}</b> by <b>{task.creator?.userName}</b></p>
            </div>
            <div className="d-flex flex-row">
                <div className="w-75">
                    <div className="ms-2 me-5">
                        <h6 className="dark-blue">Add a new comment:</h6>
                        <form onSubmit={handleSubmit}>
                            <div className="input-group mb-3">
                            <textarea
                                className={"form-control input-border " + applyErrorClass("content")}
                                id="content"
                                name="content"
                                placeholder="Comment..."
                                value={commentData.content}
                                onChange={handleChange}
                                rows="3"
                                required
                            />
                                <button className="btn send-button" type="submit" id="button-addon2">Send
                                </button>
                            </div>
                        </form>
                    </div>
                    {task.comments && task.comments.map(comment => (
                        <div key={comment.id} className="card comment-card ms-2 me-5 my-2">
                            <div className="card-header comment-header">
                                <div className="d-flex justify-content-between align-items-center">
                                    <div className="ps-2 cream">
                                        <b>
                                            {comment.author.userName}
                                        </b>
                                    </div>
                                    <div className="pe-2 cream">
                                        {comment.timestamp}
                                    </div>
                                </div>
                            </div>
                            <div className="card-body comment-body">
                                <p className="card-text dark-blue">{comment.content}</p>
                            </div>
                        </div>
                    ))}
                </div>
                <div className="w-25">
                    <div>
                        <div className="px-3 pt-3">
                            <h5 className="dark-blue"><b>Task status:</b></h5>
                        </div>
                        <div className="px-3">
                            <div className="d-flex justify-content-between align-items-center">
                                <div>
                                    <h5 style={{color: getTaskStatusColor(task.status)}}>{taskStatus[task.status]}</h5>
                                </div>
                                <div className="me-2">
                                    <button className="btn" data-bs-toggle="modal"
                                            data-bs-target="#staticBackdrop">
                                        <h5 className="dark-blue">
                                            <FontAwesomeIcon icon={faGear}/>
                                        </h5>
                                    </button>
                                    <div className="modal fade" id="staticBackdrop" data-bs-backdrop="static"
                                         data-bs-keyboard="false" tabIndex="-1"
                                         aria-labelledby="staticBackdropLabel" aria-hidden="true">
                                        <div className="modal-dialog" role="document">
                                            <div className="modal-content">
                                                <div className="modal-header">
                                                    <h3 className="modal-title fs-5 dark-blue"
                                                        id="staticBackdropLabel">Change the task status</h3>
                                                    <button type="button" className="btn-close"
                                                            data-bs-dismiss="modal" aria-label="Close"></button>
                                                </div>
                                                <form onSubmit={changeStatus}>
                                                    <div className="modal-body">
                                                        <div className="text-start my-3">
                                                            <label htmlFor="statusSelect" className="form-label">Select Status:</label>
                                                            <select
                                                                className="form-select"
                                                                id="statusSelect"
                                                                name="status"
                                                                value={selectedStatus}
                                                                onChange={handleStatusChange}
                                                            >
                                                                {statusOptions.map((option) => (
                                                                    <option key={option.key} value={option.key}>
                                                                        {option.value}
                                                                    </option>
                                                                ))}
                                                            </select>

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
                        </div>
                    </div>
                    <div>
                        <div className="px-3 pt-3">
                            <h5 className="dark-blue"><b>Last Updated:</b></h5>
                        </div>
                        <div className="px-3">
                            <h6 className="light-blue">{task.updatedAt}</h6>
                        </div>
                    </div>
                    <div>
                        <div className="px-3 py-3">
                            <h5 className="dark-blue"><b>Members assigned to this task:</b></h5>
                        </div>
                        <div className="d-flex justify-content-center flex-column flex-wrap px-3 pb-5">
                            {task.taskMembers && task.taskMembers.map(user => (
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