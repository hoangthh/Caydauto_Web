import React, { useEffect, useState } from "react";
import "./ProductPage.scss";
import { Filter } from "../../components/Filter/Filter";
import { Product } from "../../components/Product/Product";
import { Pagination, styled } from "@mui/material";
import { fetchProductsWithFilterByPagination } from "../../apis/product";

const ProductPagination = styled(Pagination)`
  margin-top: 20px;
`;

export const ProductPage = () => {
  const [productList, setProductList] = useState([]);
  const [page, setPage] = useState(1);

  useEffect(() => {
    const fetchProductList = async () => {
      const productList = await fetchProductsWithFilterByPagination(page, 6);
      setProductList(productList);
    };

    fetchProductList();
  }, [page]);

  const handleChange = (event, value) => {
    console.log(value);
    setPage(value);
  };

  return (
    <div className="product-page">
      {/* Product Filter */}
      <Filter />
      {/* Product Filter */}

      <div className="product-page--product-wrapper">
        {/* Product List */}
        <div className="product-page--product-list">
          {productList?.items?.length > 0 &&
            productList.items.map((product) => (
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
        <ProductPagination
          count={productList.totalPages}
          hidePrevButton
          color="primary"
          onChange={handleChange}
        />
        {/* Product Pagination */}
      </div>
    </div>
  );
};
