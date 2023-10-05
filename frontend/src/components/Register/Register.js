import {useState} from "react";
import {Link, useNavigate} from 'react-router-dom';
import "./Register.css"

export const Register = (props) => {
    const {register} = props
    const navigate = useNavigate();
    const defaultImageSrc = '/img/avatar.png'
    const [errors, setErrors] = useState({})
    const [formData, setFormData] = useState({
        username: '',
        password: '',
        confirmPassword: '',
        email: '',
        firstName: '',
        lastName: '',
        profilePictureSrc: defaultImageSrc,
        profilePictureFile: null,
        jobTitle: 'Other',
    });


    const showPreview = (e) => {
        if (e.target.files && e.target.files[0]) {
            let imageFile = e.target.files[0];
            const reader = new FileReader();
            reader.onload = (x) => {
                setFormData({
                    ...formData,
                    profilePictureSrc: x.target.result,
                    profilePictureFile: imageFile
                })
            }
            reader.readAsDataURL(imageFile)
        } else {
            setFormData({
                ...formData,
                profilePictureSrc: defaultImageSrc,
                profilePictureFile: null
            })
        }
    };

    const handleChange = (e) => {
        const {name, value} = e.target;
        setFormData({...formData, [name]: value});
    };

    const validate = () => {
        let temp = {}
        temp.firstName = formData.firstName !== "";
        temp.lastName = formData.lastName !== "";
        temp.username = formData.username !== "";
        temp.email = formData.email !== "";
        temp.password = formData.password !== "";
        temp.confirmPassword = formData.confirmPassword !== "";
        setErrors(temp)
        return Object.values(temp).every(x => x === true)
    }

    const resetForm = () => {
        setFormData({
            username: '',
            password: '',
            confirmPassword: '',
            email: '',
            firstName: '',
            lastName: '',
            profilePictureSrc: defaultImageSrc,
            profilePictureFile: null,
            jobTitle: 'Other',
        })
        document.getElementById("img-upload").value = null;
        setErrors({})
        navigate("/home")
    }

    const handleSubmit = (e) => {
        e.preventDefault()
        if (validate()) {
            const data = new FormData()
            data.append("username", formData.username)
            data.append("password", formData.password)
            data.append("confirmPassword", formData.confirmPassword)
            data.append("email", formData.email)
            data.append("firstName", formData.firstName)
            data.append("lastName", formData.lastName)
            data.append("profilePictureSrc", formData.profilePictureSrc)
            data.append("profilePictureFile", formData.profilePictureFile)
            data.append("jobTitle", formData.jobTitle)
            register(data, resetForm);
        }
    };

    const applyErrorClass = (field) => ((field in errors && errors[field] === false) ? " invalid" : "")

    return (
        <div className="container">
            <div className="white-box mx-auto mt-5 pt-4">
                <form onSubmit={handleSubmit}>
                    <div className="row">
                        <div className="col-3"></div>
                        <div className="col-6 d-flex align-items-center justify-content-center">
                            <div className="avatar d-flex align-items-center justify-content-center">
                                <img src={formData.profilePictureSrc} width="100%" height="100%" alt="Avatar"/>
                            </div>
                        </div>
                        <div className="col-3"></div>
                    </div>
                    <div className="row">
                        <div className="col-4"></div>
                        <div className="col-4 text-center">
                            <label className="form-label">Profile Picture</label>
                            <input
                                className="form-control input-border"
                                type="file"
                                accept="image/*"
                                id="img-upload"
                                onChange={showPreview}
                            />
                        </div>
                        <div className="col-4"></div>
                    </div>
                    <div className="row mt-4">
                        <div className="col-1"></div>
                        <div className="col-5">
                            <div className="form-floating">
                                <input
                                    className={"form-control input-border" + applyErrorClass("firstName")}
                                    type="text"
                                    name="firstName"
                                    placeholder="First Name"
                                    value={formData.firstName}
                                    onChange={handleChange}
                                />
                                <label className="form-label">First Name</label>
                            </div>
                        </div>
                        <div className="col-5">
                            <div className="form-floating">
                                <input
                                    className={"form-control input-border" + applyErrorClass("lastName")}
                                    type="text"
                                    name="lastName"
                                    placeholder="Last Name"
                                    value={formData.lastName}
                                    onChange={handleChange}
                                />
                                <label className="form-label">Last Name</label>
                            </div>
                        </div>
                        <div className="col-1"></div>
                    </div>
                    <div className="row mt-4">
                        <div className="col-1"></div>
                        <div className="col-5">
                            <div className="form-floating">
                                <input
                                    className={"form-control input-border" + applyErrorClass("username")}
                                    type="text"
                                    name="username"
                                    placeholder="Username"
                                    value={formData.username}
                                    onChange={handleChange}
                                />
                                <label className="form-label">Username</label>
                            </div>
                        </div>
                        <div className="col-5">
                            <div className="form-floating">
                                <input
                                    className={"form-control input-border" + applyErrorClass("email")}
                                    type="email"
                                    name="email"
                                    placeholder="Email"
                                    value={formData.email}
                                    onChange={handleChange}
                                />
                                <label className="form-label">Email</label>
                            </div>
                        </div>
                        <div className="col-1"></div>
                    </div>
                    <div className="row mt-4">
                        <div className="col-1"></div>
                        <div className="col-5">
                            <div className="form-floating">
                                <input
                                    className={"form-control input-border" + applyErrorClass("password")}
                                    type="password"
                                    name="password"
                                    placeholder="Password"
                                    value={formData.password}
                                    onChange={handleChange}
                                />
                                <label className="form-label">Password</label>
                            </div>
                        </div>
                        <div className="col-5">
                            <div className="form-floating">
                                <input
                                    className={"form-control input-border" + applyErrorClass("confirmPassword")}
                                    type="password"
                                    name="confirmPassword"
                                    placeholder="Confirm Password"
                                    value={formData.confirmPassword}
                                    onChange={handleChange}
                                />
                                <label className="form-label">Confirm Password</label>
                            </div>
                        </div>
                        <div className="col-1"></div>
                    </div>
                    <div className="row mt-4">
                        <div className="col-1"></div>
                        <div className="col-10">
                            <div className="form-floating">
                                <select
                                    className="form-select input-border"
                                    name="jobTitle"
                                    placeholder="Job Title"
                                    value={formData.jobTitle}
                                    onChange={handleChange}
                                >
                                    <option value="Developer">Developer</option>
                                    <option value="Tester">Tester</option>
                                    <option value="Designer">Designer</option>
                                    <option value="BusinessAnalyst">Business Analyst</option>
                                    <option value="ProjectManager">Project Manager</option>
                                    <option value="Other">Other</option>
                                </select>
                                <label className="form-label">Job Title</label>
                            </div>
                        </div>
                        <div className="col-1"></div>
                    </div>
                    <div className="mt-4 d-flex align-items-center justify-content-center">
                        <button className="btn pill-btn" type="submit">Register</button>
                    </div>
                    <div className="mt-3 d-flex align-items-center justify-content-center">
                        <Link to={"/login"} className="light-blue">Already have an account? Click here!</Link>
                    </div>
                </form>
            </div>
        </div>
    )

}