import { axiosInstance } from "./axiosInstance";

export const fetchShippingFee = async ({
  toDistrictId,
  toWardCode,
  insuranceValue,
}) => {
  try {
    const response = await axiosInstance.get(`/api/Delivery/shipping-fee`, {
      params: {
        toDistrictId,
        toWardCode,
        insuranceValue,
      },
    });
    return response.data;
  } catch (error) {
    console.log(error);
  }
};
