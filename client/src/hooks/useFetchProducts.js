import React, { useEffect, useState } from "react";
import { fetchProductsByPagination } from "../apis/product";

export const useFetchProducts = () => {
  const [productList, setProductList] = useState([]);

  useEffect(() => {
    const fetchProducts = async () => {
      const productList = await fetchProductsByPagination();

      setProductList(productList);
    };

    fetchProducts();
  }, []);

  return productList;
};
