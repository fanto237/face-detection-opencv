import axios from "axios"

const instance = axios.create({
  baseURL: "https://api.fantodev.com/",
});

instance.defaults.headers.common["Authorization"] = "Auth from an instance"

export default instance;
