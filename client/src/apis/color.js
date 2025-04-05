import { axiosInstance } from "./axiosInstance";

export const fetchColors = async () => {
  try {
    const response = await axiosInstance.get(`/api/Color`);
    return response.data;
  } catch (error) {
    console.log(error);
  }
};
