import React from "react";
import "./PaymentSuccessPage.scss";
import paymentSuccessImg from "../../assets/payment-success.svg";
import { Button, styled } from "@mui/material";

const CheckOrderButon = styled(Button)`
  text-transform: none;
  background: green;
  margin-top: 20px;
`;
const ComebackHomeButon = styled(Button)`
  text-transform: none;
  color: green;
  border: 1px solid green;
  margin-top: 10px;
`;

export const PaymentSuccessPage = () => {
  return (
    <div className="payment-success-page">
      <p className="payment-success-page--header">Tạo đơn hàng thành công</p>
      <img src={paymentSuccessImg} className="payment-success-page--img" />
      <p className="payment-success-page--title">
        Vui lòng chờ nhân viên xác nhận đơn hàng và chuyển giao đến bạn
      </p>
      <p className="payment-success-page--title">
        Cảm ơn bạn đã mua sắm với Cây Đầu To
      </p>
      <p className="payment-success-page--title">Chúc bạn 1 ngày vui vẻ</p>
      <CheckOrderButon component="a" href="/user?tab=2" variant="contained">
        Kiểm tra đơn hàng
      </CheckOrderButon>
      <ComebackHomeButon component="a" href="/" variant="outlined">
        Quay về trang chủ
      </ComebackHomeButon>
    </div>
  );
};
