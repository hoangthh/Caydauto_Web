import {
  Accordion,
  AccordionDetails,
  AccordionSummary,
  Divider,
  FormControlLabel,
  Radio,
  RadioGroup,
  styled,
  TextField,
  Typography,
} from "@mui/material";
import React from "react";
import "./PaymentPage.scss";
import ExpandMoreIcon from "@mui/icons-material/ExpandMore";
import { CartProduct } from "../../components/CartProduct/CartProduct";

const PaymentPageHeader = styled(Typography)`
  font-weight: bold;
  font-size: 20px;
  text-transform: uppercase;
`;

const AccordionHeader = styled(Typography)`
  font-weight: bold;
  font-size: 18px;
`;

const CustomerInfoHeader = styled(Typography)`
  font-weight: bold;
`;

const CustomerAddressInput = styled(TextField)`
  margin-top: 10px;
  width: 100%;
`;

const user = {
  name: "Huy Hoàng",
  email: "email@gmail.com",
  phone: "033*****",
};

export const PaymentPage = () => {
  return (
    <div className="payment-page">
      <PaymentPageHeader>Thông tin giao hàng</PaymentPageHeader>

      <div className="payment-page--wrapper">
        <Accordion>
          <AccordionSummary expandIcon={<ExpandMoreIcon />}>
            <AccordionHeader>Thông tin đơn hàng</AccordionHeader>
          </AccordionSummary>
          <AccordionDetails>
            <div className="payment-page--wrapper__order-list">
              <CartProduct showQuantity={false} />
              <Divider />
              <CartProduct showQuantity={false} />
              <Divider />
              <CartProduct showQuantity={false} />
            </div>
          </AccordionDetails>
        </Accordion>

        <Accordion>
          <AccordionSummary expandIcon={<ExpandMoreIcon />}>
            <AccordionHeader>Thông tin khách hàng</AccordionHeader>
          </AccordionSummary>
          <AccordionDetails>
            <div className="payment-page--wrapper__customer-info">
              <div className="payment-page--wrapper__customer-info item">
                <CustomerInfoHeader>Họ và tên: </CustomerInfoHeader>
                <Typography>{user.name}</Typography>
              </div>
              <div className="payment-page--wrapper__customer-info item">
                <CustomerInfoHeader>Email: </CustomerInfoHeader>
                <Typography>{user.email}</Typography>
              </div>
              <div className="payment-page--wrapper__customer-info item">
                <CustomerInfoHeader>Số điện thoại: </CustomerInfoHeader>
                <Typography>{user.phone}</Typography>
              </div>

              <CustomerAddressInput
                placeholder="Nhập địa chỉ giao hàng"
                label="Địa chỉ giao hàng"
              />
            </div>
          </AccordionDetails>
        </Accordion>

        <Accordion>
          <AccordionSummary expandIcon={<ExpandMoreIcon />}>
            <AccordionHeader>Phương thức thanh toán</AccordionHeader>
          </AccordionSummary>
          <AccordionDetails>
            <RadioGroup
              aria-labelledby="demo-radio-buttons-group-label"
              defaultValue="cod"
              name="radio-buttons-group"
            >
              <FormControlLabel
                value="cod"
                control={<Radio />}
                label="Thanh toán khi nhận hàng"
              />
              <Divider />
              <FormControlLabel
                value="vnpay"
                control={<Radio />}
                label="Chuyển khoản qua VNPAY"
              />
            </RadioGroup>
          </AccordionDetails>
        </Accordion>

        <Accordion>
          <AccordionSummary expandIcon={<ExpandMoreIcon />}></AccordionSummary>
          <AccordionDetails></AccordionDetails>
        </Accordion>
      </div>
    </div>
  );
};
