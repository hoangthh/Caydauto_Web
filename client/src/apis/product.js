import { axiosInstance } from "./axiosInstance";

export const fetchProductsWithFilterByPagination = async () => {
  try {
    const response = await axiosInstance.get(`/api/Product/`);
    return response.data.items;
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

export const fetchSimilarProductsById = async (productId) => {
  try {
    const response = await axiosInstance.get(
      `/api/Product/similar/${productId}`
    );
    return response.data;
  } catch (error) {
    console.log(error);
  }
};
