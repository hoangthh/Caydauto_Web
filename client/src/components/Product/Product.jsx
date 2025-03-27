import React from "react";
import "./Product.scss";
import product from "../../assets/product.svg";
import { Button, styled, Typography } from "@mui/material";

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

export const Product = () => {
  return (
    <div className="product">
      <img src={product} style={{ width: "100%" }} />
      <ProductName>Đèn học Bakia</ProductName>
      <ProductPrice>250.000đ</ProductPrice>
      <ProductSold>280 sold</ProductSold>
      <BuyButton variant="contained">Mua hàng</BuyButton>
    </div>
  );
};
