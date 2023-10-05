import axios from "axios";

export const BASE_URL = 'http://localhost:5230/'

export const ENDPOINTS = {
    register: "Auth/Register",
    login: "Auth/Login",
    projects: "Projects",
    userProjects: "Projects/UserProjects",
    users: "Users",
    userProfile: "Users/UserProfile",
    userTasks: "Users/UserTasks",
    tasks: "Tasks",
    comments: "Comments"
}

export const createAPIEndpoint = (endpoint, headers={}) => {
    let url = BASE_URL + 'api/' + endpoint + '/'
    return {
        fetch: () => axios.get(url,{ headers: headers }),
        fetchById: (id) => axios.get(url+id,{ headers: headers }),
        post: (newRecord) => axios.post(url,newRecord,{ headers: headers }),
        put: (id,updateRecord) => axios.put(url+id,updateRecord,{ headers: headers }),
        delete: id => axios.delete(url+id,{ headers: headers })
    }
}

const instance = axios.create({
    baseURL: 'http://localhost:5230/api',
    headers: {
        'Access-Control-Allow-Origin' : '*',
    }
})

export default instance;