import React from "react";
import "./Product.scss";
import productImg from "../../assets/product.svg";
import { Button, styled, Typography } from "@mui/material";
import { Link } from "react-router-dom";

const ProductName = styled(Typography)`
  margin-top: 5px;
  font-size: 13px;
`;

const ProductPrice = styled(Typography)`
  font-weight: bold;
`;

const ProductSold = styled(Typography)`
  font-size: 13px;
`;

const BuyButton = styled(Button)`
  width: 100%;
  background: white;
  color: black;
  text-transform: none;
  margin-top: 10px;
  border: 1px solid black;

  &:hover {
    background: green;
    color: white;
  }
`;

export const Product = ({ product }) => {
  return (
    <div className="product">
      <img src={productImg} style={{ width: "100%" }} />
      <ProductName>{product.name}</ProductName>
      <ProductPrice>{product.price}đ</ProductPrice>
      <ProductSold>{product.sold} sold</ProductSold>
      <Link to={`/products/${product.id}`}>
        <BuyButton variant="contained">Mua hàng</BuyButton>
      </Link>
    </div>
  );
};
