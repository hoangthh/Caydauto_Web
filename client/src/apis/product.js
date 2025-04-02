import { axiosInstance } from "./axiosInstance";

export const fetchProductsWithFilterByPagination = async (page, pageSize) => {
  try {
    const response = await axiosInstance.get(
      `/api/Product/all?page=${page}&pageSize=${pageSize}`
    );
    return response.data;
  } catch (error) {
    console.log(error);
  }
};

export const fetchDetailProductById = async (productId) => {
  try {
    const response = await axiosInstance.get(`/api/Product/${productId}`);
    return response.data;
  } catch (error) {
    console.log(error);
  }
};

export const fetchSimilarProductsById = async (productId) => {
  try {
    const response = await axiosInstance.get(
      `/api/Product/similar/${productId}`
    );
    return response.data;
  } catch (error) {
    console.log(error);
  }
};

export const fetchFilterProducts = async ({
  page,
  pageSize,
  categories,
  brands,
  minPrice,
  maxPrice,
  colors,
  orderBy,
}) => {
  try {
    const response = await axiosInstance.post(`/api/Product`, {
      page,
      pageSize,
      categories,
      brands,
      minPrice,
      maxPrice,
      colors,
      orderBy,
    });
    return response.data;
  } catch (error) {
    console.log(error);
  }
};
