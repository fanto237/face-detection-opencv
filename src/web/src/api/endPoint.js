import axios from "axios"

const instance = axios.create({
  baseURL: "https://45.137.68.35:5001",
});

instance.defaults.headers.common["Authorization"] = "Auth from an instance"

export default instance;
