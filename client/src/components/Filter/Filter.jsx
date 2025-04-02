import React, { useEffect, useState } from "react";
import "./Filter.scss";
import { Slider, styled, Typography } from "@mui/material";
import { fetchFilter } from "../../apis/filter";
import { convertNumberToPrice } from "../../helpers/string";

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

const MIN_PRICE_DEFAULT = 0;
const MAX_PRICE_DEFAULT = 500000;
const STEP_PRICE = 100000;
const MIN_PRICE_DISTANCE = 50000;

export const Filter = ({ onFilterChange }) => {
  const [categoryList, setCategoryList] = useState([]);
  const [brandList, setBrandList] = useState([]);
  const [colorList, setColorList] = useState([]);

  const [valuePrice, setValuePrice] = React.useState([
    MIN_PRICE_DEFAULT,
    MAX_PRICE_DEFAULT,
  ]);

  const [filters, setFilters] = useState({
    categories: [],
    brands: [],
    colors: [],
    minPrice: MIN_PRICE_DEFAULT,
    maxPrice: MAX_PRICE_DEFAULT,
  });

  const minPriceDistance = MIN_PRICE_DISTANCE;

  useEffect(() => {
    const fetchFilterList = async () => {
      const filter = await fetchFilter();
      setCategoryList(filter.categories);
      setBrandList(filter.brands);
      setColorList(filter.colors);
    };
    fetchFilterList();
  }, []);

  // Gửi filters ra ngoài khi nó thay đổi
  useEffect(() => {
    if (onFilterChange) {
      onFilterChange(filters);
    }
  }, [filters, onFilterChange]);

  const handleChange = (event, filterValue, flag) => {
    setFilters((prevFilters) => {
      if (flag === "price") {
        let newPrices = [...prevFilters.prices];
        let newMinPrices = prevFilters.minPrice ? [prevFilters.minPrice] : [];
        let newMaxPrices = prevFilters.maxPrice ? [prevFilters.maxPrice] : [];

        if (newPrices.includes(filterValue.label)) {
          newPrices = newPrices.filter((label) => label !== filterValue.label);
          newMinPrices = newMinPrices.filter((min) => min !== filterValue.min);
          newMaxPrices = newMaxPrices.filter((max) => max !== filterValue.max);
        } else {
          newPrices.push(filterValue.label);
          newMinPrices.push(filterValue.min);
          newMaxPrices.push(filterValue.max);
        }

        return {
          ...prevFilters,
          prices: newPrices,
          minPrice: newMinPrices.length ? Math.min(...newMinPrices) : null,
          maxPrice: newMaxPrices.length ? Math.max(...newMaxPrices) : null,
        };
      }

      const filterItems = prevFilters[event.target.name];
      const isSelected = filterItems.includes(filterValue);

      return {
        ...prevFilters,
        [event.target.name]: isSelected
          ? filterItems.filter((filter) => filter !== filterValue)
          : [...filterItems, filterValue],
      };
    });
  };

  const handlePriceChange = (event, newValue, activeThumb) => {
    if (activeThumb === 0) {
      setValuePrice([
        Math.min(newValue[0], valuePrice[1] - minPriceDistance),
        valuePrice[1],
      ]);
      setFilters({
        ...filters,
        minPrice: newValue[0],
        maxPrice: newValue[1],
      });
    } else {
      setValuePrice([
        valuePrice[0],
        Math.max(newValue[1], valuePrice[0] + minPriceDistance),
      ]);
    }
  };

  const valueLabelFormat = (value) => {
    return convertNumberToPrice(value);
  };

  return (
    <div className="filter">
      <FilterHeader>Lọc sản phẩm</FilterHeader>

      <FilterSubHeader>Phân loại</FilterSubHeader>

      <div className="filter--category">
        {/* Category Item */}
        {categoryList.map((category) => (
          <div className="filter--category__item" key={category.id}>
            <FilterItem>{category.name}</FilterItem>
            <input
              type="checkbox"
              className="input"
              name="categories"
              checked={filters.categories.includes(category.id)}
              // onChange={() => handleCategoryChange(category.id)}
              onChange={(event) => handleChange(event, category.id)}
            />
          </div>
        ))}
        {/* Category Item */}
      </div>

      <FilterSubHeader>Thương hiệu</FilterSubHeader>

      <div className="filter--brands">
        {brandList.map((brand, index) => (
          <div className="filter--brands__item" key={index}>
            <FilterItem>{brand}</FilterItem>
            <input
              type="checkbox"
              className="input"
              name="brands"
              checked={filters.brands.includes(brand)}
              onChange={(event) => handleChange(event, brand)}
            />
          </div>
        ))}
      </div>

      <FilterSubHeader>Giá</FilterSubHeader>

      <div className="filter--price">
        <Slider
          value={valuePrice}
          onChange={handlePriceChange}
          valueLabelFormat={valueLabelFormat}
          valueLabelDisplay="auto"
          disableSwap
          min={MIN_PRICE_DEFAULT}
          step={STEP_PRICE}
          max={MAX_PRICE_DEFAULT}
        />
      </div>

      <FilterSubHeader>Màu</FilterSubHeader>
      <div className="filter--color">
        {colorList.map((color) => (
          <input
            key={color.id}
            type="checkbox"
            className={`input filter-color ${color.name}`}
            name="colors"
            checked={filters.colors.includes(color.id)}
            onChange={(event) => handleChange(event, color.id)}
          />
        ))}
      </div>
    </div>
  );
};
