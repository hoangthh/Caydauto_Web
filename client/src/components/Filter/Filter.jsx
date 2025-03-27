import React from "react";
import "./Filter.scss";
import { styled, Typography } from "@mui/material";

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
  return (
    <div className="filter">
      <FilterHeader>Lọc sản phẩm</FilterHeader>

      <FilterSubHeader>Phân loại</FilterSubHeader>

      <div className="filter--category">
        {/* Category Item */}
        <div className="filter--category__item">
          <FilterItem>Tất cả sản phẩm</FilterItem>
          <input type="checkbox" className="input" />
        </div>
        {/* Category Item */}
        <FilterItem>Hộp bút - Túi bút</FilterItem>
        <FilterItem>Vở học sinh</FilterItem>
        <FilterItem>Bút chì</FilterItem>
        <FilterItem>Bút gel</FilterItem>
        <FilterItem>Bút ghi nhớ</FilterItem>
        <FilterItem>Băng keo</FilterItem>
        <FilterItem>Dụng cụ hỗ trợ</FilterItem>
      </div>

      <FilterSubHeader>Thương hiệu</FilterSubHeader>

      <div className="filter--brands">
        <div className="filter--brands__item">
          <FilterItem>Crayonia</FilterItem>
          <input type="checkbox" className="input" />
        </div>
        <div className="filter--brands__item">
          <FilterItem>Pilot</FilterItem>
          <input type="checkbox" className="input" />
        </div>
        <div className="filter--brands__item">
          <FilterItem>Marvy</FilterItem>
          <input type="checkbox" className="input" />
        </div>
        <div className="filter--brands__item">
          <FilterItem>Stabilo</FilterItem>
          <input type="checkbox" className="input" />
        </div>
      </div>

      <FilterSubHeader>Giá</FilterSubHeader>

      <div className="filter--price">
        <div className="filter--price__item">
          <FilterItem>Dưới 100.000</FilterItem>
          <input type="checkbox" className="input" />
        </div>
        <div className="filter--price__item">
          <FilterItem>100.000 - 500.000</FilterItem>
          <input type="checkbox" className="input" />
        </div>
        <div className="filter--price__item">
          <FilterItem>Trên 500.000</FilterItem>
          <input type="checkbox" className="input" />
        </div>
      </div>

      <FilterSubHeader>Màu</FilterSubHeader>
      <div className="filter--color">
        <input type="checkbox" className="input color white" />
        <input type="checkbox" className="input color gray" />
        <input type="checkbox" className="input color yellow" />
        <input type="checkbox" className="input color orange" />
        <input type="checkbox" className="input color black" />
        <input type="checkbox" className="input color red" />
        <input type="checkbox" className="input color blue" />
        <input type="checkbox" className="input color pink" />
        <input type="checkbox" className="input color green" />
        <input type="checkbox" className="input color brown" />
      </div>
    </div>
  );
};
