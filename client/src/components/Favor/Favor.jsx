import React, { useEffect, useState } from "react";
import { fetchWishList } from "../../apis/favor";
import { ProductList } from "../ProductList/ProductList";

export const Favor = () => {
  const [favorList, setFavorList] = useState([]);

  useEffect(() => {
    const fetchFavouList = async () => {
      const favorList = await fetchWishList();
      setFavorList(favorList);
    };
    fetchFavouList();
  }, []);

  return (
    <div>
      <ProductList productList={favorList} itemPerRow={3} />
    </div>
  );
};
