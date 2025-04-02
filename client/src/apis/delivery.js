import { axiosInstance } from "./axiosInstance";

export const fetchProvinces = async () => {
  try {
    const response = await axiosInstance.get(`/api/Delivery/provinces`);
    return response.data;
  } catch (error) {
    console.log(error);
  }
};

export const fetchDistricts = async (provinceId) => {
  try {
    const response = await axiosInstance.get(
      `/api/Delivery/districts/${provinceId}`
    );
    return response.data;
  } catch (error) {
    console.log(error);
  }
};

export const fetchWards = async (districtId) => {
  try {
    const response = await axiosInstance.get(
      `/api/Delivery/wards/${districtId}`
    );
    return response.data;
  } catch (error) {
    console.log(error);
  }
};
