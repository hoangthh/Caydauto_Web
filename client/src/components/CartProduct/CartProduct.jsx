import React from "react";
import "./CartProduct.scss";
import productImg from "../../assets/product.svg";
import { styled, Typography } from "@mui/material";
import AddCircleRoundedIcon from "@mui/icons-material/AddCircleRounded";
import RemoveCircleRoundedIcon from "@mui/icons-material/RemoveCircleRounded";

const ProductName = styled(Typography)`
  font-weight: bold;
`;
const ProductColor = styled(Typography)``;

const IncreaseQuantityButton = styled(AddCircleRoundedIcon)`
  margin-left: 10px;
  color: pink;
  cursor: pointer;
`;

const DecreaseQuantityButton = styled(RemoveCircleRoundedIcon)`
  margin-right: 10px;
  color: pink;
  cursor: pointer;
`;

const ProductQuantity = styled(Typography)``;

const ProductPrice = styled(Typography)`
  font-weight: bold;
  font-size: 18px;
`;

export const CartProduct = ({ showQuantity = true, cartItem }) => {
  return (
    <div className="cart-product">
      <div className="cart-product--left">
        <img
          src={cartItem.product.imageUrl || productImg}
          className="cart-product--left__img"
        />
        <div className="cart-product--left__info">
          <ProductName>{cartItem.product.name}</ProductName>
          <ProductColor>{cartItem.color.name}</ProductColor>
        </div>
      </div>
      <div className="cart-product--right">
        {/* Main Product Quantity */}
        {showQuantity && (
          <div className="cart-product--right__quantity">
            <DecreaseQuantityButton />
            <ProductQuantity>{cartItem.quantity}</ProductQuantity>
            <IncreaseQuantityButton />
          </div>
        )}

        {/* Main Product Price */}
        <div className="cart-product--right__price">
          <ProductPrice>{cartItem.product.price}đ</ProductPrice>
        </div>
      </div>
    </div>
  );
};
