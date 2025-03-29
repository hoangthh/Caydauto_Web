import React from "react";
import "./ProductPage.scss";
import { Filter } from "../../components/Filter/Filter";
import { Product } from "../../components/Product/Product";
import { Pagination, styled } from "@mui/material";
import { mockProductList } from "../../mockData";
// import { useFetchProducts } from "../../hooks/useFetchProducts";

const ProductPagination = styled(Pagination)`
  margin-top: 20px;
`;

export const ProductPage = () => {
  // const productList = useFetchProducts()
  const productList = mockProductList;

  return (
    <div className="product-page">
      {/* Product Filter */}
      <Filter />
      {/* Product Filter */}

      <div className="product-page--product-wrapper">
        {/* Product List */}
        <div className="product-page--product-list">
          {productList.length > 0 &&
            productList.map((product) => (
              <div
                className="product-page--product-list__product-item"
                key={product.id}
              >
                <Product product={product} />
              </div>
            ))}
        </div>
        {/* Product List */}

        {/* Product Pagination */}
        <ProductPagination count={10} hidePrevButton color="primary" />
        {/* Product Pagination */}
      </div>
    </div>
  );
};
