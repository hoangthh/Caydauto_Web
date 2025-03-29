import React from "react";
import "./CartProduct.scss";
import product from "../../assets/product.svg";
import { styled, Typography } from "@mui/material";
import AddCircleRoundedIcon from "@mui/icons-material/AddCircleRounded";
import RemoveCircleRoundedIcon from "@mui/icons-material/RemoveCircleRounded";

const ProductName = styled(Typography)`
  font-weight: bold;
`;
const ProductColor = styled(Typography)``;
const ProductPrice = styled(Typography)`
  font-weight: bold;
`;

const IncreaseQuantityButton = styled(AddCircleRoundedIcon)`
  margin-right: 10px;
  color: pink;
  cursor: pointer;
`;

const DecreaseQuantityButton = styled(RemoveCircleRoundedIcon)`
  margin-left: 10px;
  color: pink;
  cursor: pointer;
`;

const ProductQuantity = styled(Typography)``;

export const CartProduct = ({ showQuantity = true }) => {
  return (
    <div className="cart-product">
      <div className="cart-product--left">
        <img src={product} className="cart-product--left__img" />
        <div className="cart-product--left__info">
          <ProductName>Tên sản phẩm</ProductName>
          <ProductColor>Hồng</ProductColor>
        </div>
      </div>
      <div className="cart-product--right">
        {/* Main Product Quantity */}
        {showQuantity && (
          <div className="cart-product--right__quantity">
            <IncreaseQuantityButton />
            <ProductQuantity>5</ProductQuantity>
            <DecreaseQuantityButton />
          </div>
        )}

        {/* Main Product Price */}
        <div className="cart-product--right__price">
          <ProductPrice>250.000đ</ProductPrice>
        </div>
      </div>
    </div>
  );
};
