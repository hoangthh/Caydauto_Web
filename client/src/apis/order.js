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
    const orderItems = cartProductList.map((cartProduct) => ({
      productId: cartProduct.product.id,
      colorId: cartProduct.color.id,
      quantity: cartProduct.quantity,
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
    return response;
  } catch (error) {
    console.log(error);
  }
};

export const fetchOrderByUser = async (pageNumber, pageSize) => {
  try {
    const response = await axiosInstance.get(`/api/Order/user`, {
      params: {
        pageNumber,
        pageSize,
      },
    });
    return response.data;
  } catch (error) {
    console.log(error);
  }
};
