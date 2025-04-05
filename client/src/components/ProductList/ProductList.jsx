import React from "react";
import "./ProductList.scss";
import { Product } from "../Product/Product";

export const ProductList = ({ productList, itemPerRow }) => {
  return (
    <div className="product-list">
      {productList?.items?.length > 0
        ? productList.items.map((product) => (
            <div
              className="product-list__product-item"
              key={product.id}
              style={{
                flexBasis: itemPerRow ? `calc(100%/${itemPerRow} - 20px)` : "",
              }}
            >
              <Product product={product} />
            </div>
          ))
        : "Hiện chưa có sản phẩm nào"}
    </div>
  );
};
