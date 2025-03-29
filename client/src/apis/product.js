import axios from "axios";

const axiosInstance = axios.create({
  baseURL: import.meta.env.VITE_BACKEND_URL,
  // withCredentials: true
});

export const fetchProductsWithFilterByPagination = async () => {
  try {
    const response = await axiosInstance.get(`/api/Product/`);

    return response.data.item;
  } catch (error) {
    console.log(error);
  }
};

export const fetchDetailProductById = async (productId) => {
  try {
    const response = await axiosInstance.get(`/api/Product/${productId}`);
    return response.data;
  } catch (error) {
    console.log(error);
  }
};
