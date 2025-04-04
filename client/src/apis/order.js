import { axiosInstance } from "./axiosInstance";

export const createOrder = async ({
  paymentMethod,
  shippingAddress,
  toProvinceId,
  toDistrictId,
  toWardId,
  discountCode,
  cartProductList,
}) => {
  try {
    const orderItems = cartProductList.map((order) => ({
      productId: order.id,
      colorId: order.color.id,
      quantity: order.quantity,
    }));
    const order = {
      paymentMethod,
      shippingAddress,
      toProvinceId,
      toDistrictId,
      toWardId,
      discountCode,
      orderItems,
    };
    const response = await axiosInstance.post(`/api/Order`, order);
    return response.data;
  } catch (error) {
    console.log(error);
  }
};
