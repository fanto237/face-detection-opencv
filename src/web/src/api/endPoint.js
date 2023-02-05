import axios from "axios"

const instance = axios.create({
  baseURL: "https://localhost:7259/api/orders",
});

instance.defaults.headers.common["Authorization"] = "Auth from an instance"

export default instance;
