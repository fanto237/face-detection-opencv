import axios from "axios"

const instance = axios.create({
  baseURL: "http://45.137.68.35:5000",
});

instance.defaults.headers.common["Authorization"] = "Auth from an instance"

export default instance;
