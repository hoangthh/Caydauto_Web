import React, { useEffect, useState } from "react";
import "./ProductPage.scss";
import { Filter } from "../../components/Filter/Filter";
import { Product } from "../../components/Product/Product";
import { Pagination, styled } from "@mui/material";
import {
  fetchProductsWithFilterByPagination,
  fetchFilterProducts,
} from "../../apis/product";
import { debounce } from "lodash";

const ProductPagination = styled(Pagination)`
  margin-top: 20px;
`;

const PAGE_SIZE = 6;

export const ProductPage = () => {
  const [productList, setProductList] = useState([]);
  const [page, setPage] = useState(1);
  const [filters, setFilters] = useState({});

  useEffect(() => {
    const fetchProductList = async () => {
      const productList = await fetchProductsWithFilterByPagination(
        page,
        PAGE_SIZE
      );
      console.log("productList: ", productList);
      setProductList(productList);
    };

    fetchProductList();
  }, [page]);

  useEffect(() => {
    const fetchFilteredProducts = debounce(async () => {
      const response = await fetchFilterProducts(filters); // Hàm gọi API với filters
      console.log("filter: ", { filters, response });
      setProductList(response); // Cập nhật danh sách sản phẩm
    }, 300); // Chờ 300ms trước khi gọi API

    fetchFilteredProducts();

    return () => fetchFilteredProducts.cancel(); // Hủy debounce nếu filters thay đổi liên tục
  }, [filters]);

  return (
    <div className="product-page">
      {/* Product Filter */}
      <Filter onFilterChange={(newFilters) => setFilters(newFilters)} />
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
          onChange={(event, value) => setPage(value)}
        />
        {/* Product Pagination */}
      </div>
    </div>
  );
};
