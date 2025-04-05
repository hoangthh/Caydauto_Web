import { axiosInstance } from "./axiosInstance";

export const login = async ({ email, password }) => {
  try {
    const response = await axiosInstance.post(`/api/Account/login`, {
      email,
      password,
    });

    return response.data;
  } catch (error) {
    console.log(error);
  }
};

export const register = async ({ fullName, email, password }) => {
  try {
    const response = await axiosInstance.post(`/api/Account/register`, {
      fullName,
      email,
      password,
    });

    return response.data;
  } catch (error) {
    console.log(error);
  }
};

export const logout = async () => {
  try {
    const response = await axiosInstance.post(`/api/Account/logout`);

    return response;
  } catch (error) {
    console.log(error);
    return error;
  }
};

export const checkAuthenticated = async () => {
  try {
    const response = await axiosInstance.get(`/api/Account/state`);

    return response;
  } catch (error) {
    console.log(error);
    return error;
  }
};
