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

  const [filters, setFilters] = useState({
    categories: [],
    brands: [],
    colors: [],
    prices: [],
    minPrice: 0,
    maxPrice: 0,
  });

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

  console.log(filters);
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
        {priceList.map((price, index) => (
          <div className="filter--price__item" key={index}>
            <FilterItem>{price.label}</FilterItem>
            <input
              type="checkbox"
              className="input"
              name="prices"
              onChange={(event) => handleChange(event, price, "price")}
            />
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
            name="colors"
            checked={filters.colors.includes(color.id)}
            onChange={(event) => handleChange(event, color.id)}
          />
        ))}
      </div>
    </div>
  );
};
