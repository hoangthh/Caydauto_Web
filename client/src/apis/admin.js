import { axiosInstance } from "./axiosInstance";

// Add Category
export const addCategory = async ({ name, description }) => {
  try {
    const res = await axiosInstance.post(`api/Category/create`, {
      name,
      description,
    });
    return res.data;
  } catch (error) {
    console.error("Error adding category:", error);
    throw error;
  }
};

// Add Color
export const addColor = async ({ name }) => {
  try {
    const res = await axiosInstance.post(`api/Color`, { name });
    return res.data;
  } catch (error) {
    console.error("Error adding color:", error);
    throw error;
  }
};

// Add Product
export const addProduct = async ({
  name,
  price,
  description,
  brand,
  stockQuantity,
  images,
  categoryIds,
  colorIds,
}) => {
  try {
    const res = await axiosInstance.post(`api/Product/create`, {
      name,
      price,
      description,
      brand,
      stockQuantity,
      images, // format: [{ file: "image1.jpg" }]
      categoryIds,
      colorIds,
    });
    return res.data;
  } catch (error) {
    console.error("Error adding product:", error);
    throw error;
  }
};

// Add Order
export const addOrder = async ({
  paymentMethod,
  shippingAddress,
  toProvinceId,
  toDistrictId,
  toWardId,
  discountCode,
  orderItems,
}) => {
  try {
    const res = await axiosInstance.post(`api/Order`, {
      paymentMethod,
      shippingAddress,
      toProvinceId,
      toDistrictId,
      toWardId,
      discountCode,
      orderItems, // format: [{ productId, colorId, quantity }]
    });
    return res.data;
  } catch (error) {
    console.error("Error adding order:", error);
    throw error;
  }
};
