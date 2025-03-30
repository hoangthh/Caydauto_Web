import React, { useEffect, useState } from "react";
import "./Filter.scss";
import { styled, Typography } from "@mui/material";
import { fetchFilter } from "../../apis/filter";

const FilterHeader = styled(Typography)`
  font-size: 25px;
  font-weight: bold;
`;

const FilterSubHeader = styled(Typography)`
  font-size: 20px;
  font-weight: bold;
  margin-top: 20px;
`;

const FilterItem = styled(Typography)``;

export const Filter = () => {
  const [categoryList, setCategoryList] = useState([]);
  const [brandList, setBrandList] = useState([]);
  const [priceList, setPriceList] = useState([]);
  const [colorList, setColorList] = useState([]);

  useEffect(() => {
    const fetchFilterList = async () => {
      const filter = await fetchFilter();
      setCategoryList(filter.categories);
      setBrandList(filter.brands);
      setPriceList(filter.prices);
      setColorList(filter.colors);
    };
    fetchFilterList();
  }, []);

  return (
    <div className="filter">
      <FilterHeader>Lọc sản phẩm</FilterHeader>

      <FilterSubHeader>Phân loại</FilterSubHeader>

      <div className="filter--category">
        {/* Category Item */}
        {categoryList.map((category) => (
          <div className="filter--category__item" key={category.id}>
            <FilterItem>{category.name}</FilterItem>
            <input type="checkbox" className="input" />
          </div>
        ))}
        {/* Category Item */}
      </div>

      <FilterSubHeader>Thương hiệu</FilterSubHeader>

      <div className="filter--brands">
        {brandList.map((brand, index) => (
          <div className="filter--brands__item" key={index}>
            <FilterItem>{brand}</FilterItem>
            <input type="checkbox" className="input" />
          </div>
        ))}
      </div>

      <FilterSubHeader>Giá</FilterSubHeader>

      <div className="filter--price">
        {priceList.map((price, index) => (
          <div className="filter--price__item" key={index}>
            <FilterItem>{price.label}</FilterItem>
            <input type="checkbox" className="input" />
          </div>
        ))}
      </div>

      <FilterSubHeader>Màu</FilterSubHeader>
      <div className="filter--color">
        {colorList.map((color) => (
          <input
            key={color.id}
            type="checkbox"
            className={`input filter-color ${color.name}`}
          />
        ))}
      </div>
    </div>
  );
};
