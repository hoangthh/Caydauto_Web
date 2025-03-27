import React from "react";
import "./ProductPage.scss";
import { Filter } from "../../components/Filter/Filter";
import { Product } from "../../components/Product/Product";
import { Pagination, styled } from "@mui/material";

const ProductPagination = styled(Pagination)`
  margin-top: 20px;
`;

export const ProductPage = () => {
  return (
    <div className="product-page">
      <Filter />
      <div className="product-page--product-wrapper">
        <div className="product-page--product-list">
          <div className="product-page--product-list__product-item">
            <Product />
          </div>
          <div className="product-page--product-list__product-item">
            <Product />
          </div>
          <div className="product-page--product-list__product-item">
            <Product />
          </div>
          <div className="product-page--product-list__product-item">
            <Product />
          </div>
          <div className="product-page--product-list__product-item">
            <Product />
          </div>
        </div>

        <ProductPagination count={10} hidePrevButton color="primary" />
      </div>
    </div>
  );
};
