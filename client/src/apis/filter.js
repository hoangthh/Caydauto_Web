import { axiosInstance } from "./axiosInstance";

export const fetchFilter = async () => {
  try {
    const response = await axiosInstance.get(`/api/Product/filter`);
    return response.data;
  } catch (error) {
    console.log(error);
  }
};
