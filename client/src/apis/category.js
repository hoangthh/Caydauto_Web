import { axiosInstance } from "./axiosInstance";

export const fetchCategories = async () => {
  try {
    const response = await axiosInstance.get(`/api/Category/all`);
    return response.data;
  } catch (error) {
    console.log(error);
  }
};
