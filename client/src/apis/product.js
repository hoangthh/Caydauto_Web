import axios from "axios";

const axiosInstance = axios.create({
  baseURL: import.meta.env.VITE_BACKEND_URL,
  // withCredentials: true
});

export const fetchProductsByPagination = async () => {
  const response = axiosInstance.get(`/api/Product/`);

  return response.data.item;
};
