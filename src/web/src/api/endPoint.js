import axios from "axios"

const instance = axios.create({
  baseURL: "https://localhost:5000",
});

instance.defaults.headers.common["Authorization"] = "Auth from an instance"

export default instance;
