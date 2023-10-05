import {Link, useNavigate} from "react-router-dom";
import {useState} from "react";
import "./Login.css";

export const Login = (props) => {
    const {logIn} = props
    const navigate = useNavigate();
    const [errors, setErrors] = useState({})
    const [formData, setFormData] = useState({
        username: '',
        password: '',
    });

    const handleChange = (e) => {
        const {name, value} = e.target;
        setFormData({...formData, [name]: value});
    };

    const validate = () => {
        let temp = {}
        temp.username = formData.username !== "";
        temp.password = formData.password !== "";
        setErrors(temp)
        return Object.values(temp).every(x => x === true)
    }

    const resetForm = () => {
        setFormData({
            username: '',
            password: '',
        })
        setErrors({})
        navigate("/home");
    }

    const handleSubmit = (e) => {
        e.preventDefault()
        if (validate()) {
            const data = new FormData()
            data.append("username", formData.username)
            data.append("password", formData.password)
            logIn(data, resetForm);
        }
    };

    const applyErrorClass = (field) => ((field in errors && errors[field] === false) ? " invalid" : "")

    return (
        <div className="container">
            <div className="white-box-sm mx-auto mt-5 pt-5">
                <div className="text-center mt-1 mb-5">
                    <h1 className="dark-blue pb-3">Log in</h1>
                </div>
                <form onSubmit={handleSubmit}>
                    <div className="row mt-4">
                        <div className="col-2"></div>
                        <div className="col-8">
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
                        <div className="col-2"></div>
                    </div>
                    <div className="row mt-4">
                        <div className="col-2"></div>
                        <div className="col-8">
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
                        <div className="col-2"></div>
                    </div>
                    <div className="mt-4 d-flex align-items-center justify-content-center">
                        <button className="btn pill-btn" type="submit">Login</button>
                    </div>
                    <div className="mt-3 d-flex align-items-center justify-content-center">
                        <Link to={"/register"} className="light-blue">Not signed up? Click here!</Link>
                    </div>
                </form>
            </div>
        </div>
    )
}