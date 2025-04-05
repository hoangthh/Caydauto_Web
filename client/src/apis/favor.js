import { axiosInstance } from "./axiosInstance";

export const addToWishList = async (productId) => {
  try {
    const response = await axiosInstance.post(`/api/WishList/${productId}`);
    return response;
  } catch (error) {
    console.log(error);
  }
};

export const deleteFromWishList = async (productId) => {
  try {
    const response = await axiosInstance.delete(`/api/WishList/${productId}`);
    return response;
  } catch (error) {
    console.log(error);
  }
};

export const fetchWishList = async () => {
  try {
    const response = await axiosInstance.get(`/api/WishList`);
    return response.data;
  } catch (error) {
    console.log(error);
  }
};
