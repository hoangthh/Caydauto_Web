import React from "react";
import "./CartPage.scss";
import { Link } from "react-router-dom";
import { Button, styled, Typography } from "@mui/material";
import { CartProduct } from "../../components/CartProduct/CartProduct";

const CartPageHeader = styled(Typography)`
  font-weight: bold;
  font-size: 20px;
`;

const TotalPriceHeader = styled(Typography)`
  font-weight: bold;
  font-size: 20px;
`;

const TotalPrice = styled(Typography)`
  font-weight: bold;
  font-size: 20px;
  color: green;
`;

const PaymentButton = styled(Button)`
  width: 100%;
  margin-top: 20px;
  text-transform: none;
  background: green;
  color: white;
`;

const ContinueBuyButton = styled(Button)`
  width: 100%;
  margin-top: 20px;
  text-transform: none;
  color: black;
  border: 1px solid black;
`;

export const CartPage = () => {
  return (
    <div className="cart-page">
      <CartPageHeader>GIỎ HÀNG</CartPageHeader>

      <div className="cart-page--main">
        <div className="cart-page--main__left">
          <CartProduct />
          <CartProduct />
          <CartProduct />
          <CartProduct />
          <CartProduct />
          <CartProduct />
        </div>

        <div className="cart-page--main__right">
          <div className="cart-page--main__right price">
            <TotalPriceHeader>Thành tiền</TotalPriceHeader>
            <TotalPrice>200.000đ</TotalPrice>
          </div>

          <Link
            to="/payment"
            style={{ color: "white", textDecoration: "none" }}
          >
            <PaymentButton variant="contained">Thanh toán</PaymentButton>
          </Link>

          <Link
            to="/products"
            style={{ color: "black", textDecoration: "none" }}
          >
            <ContinueBuyButton variant="outlined">
              Tiếp tục mua hàng
            </ContinueBuyButton>
          </Link>
        </div>
      </div>
    </div>
  );
};
