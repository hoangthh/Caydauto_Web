import { axiosInstance } from "./axiosInstance";

export const fetchCartProducts = async () => {
  try {
    const response = await axiosInstance.get(`/api/Cart`);
    return response.data;
  } catch (error) {
    console.log(error);
  }
};

export const addProductToCart = async ({ productId, colorId, quantity }) => {
  try {
    const response = await axiosInstance.post(`/api/Cart/add`, {
      productId,
      colorId,
      quantity,
    });
    return response;
  } catch (error) {
    console.log(error);
  }
};

export const removeProductFromCart = async (cartItemId) => {
  try {
    const response = await axiosInstance.delete(
      `/api/Cart/remove/${cartItemId}`
    );
    return response;
  } catch (error) {
    console.log(error);
  }
};
