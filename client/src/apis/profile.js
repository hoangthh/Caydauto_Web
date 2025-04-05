import { axiosInstance } from "./axiosInstance";

export const fetchProfile = async () => {
  try {
    const response = await axiosInstance.get(`/api/Account/state`);
    return response.data;
  } catch (error) {
    console.log(error);
  }
};
