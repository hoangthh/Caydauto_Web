import {
  Accordion,
  AccordionDetails,
  AccordionSummary,
  Button,
  Divider,
  FormControlLabel,
  InputLabel,
  Radio,
  RadioGroup,
  styled,
  TextField,
  Typography,
} from "@mui/material";
import React, { useEffect, useState, useCallback } from "react";
import "./PaymentPage.scss";
import ExpandMoreIcon from "@mui/icons-material/ExpandMore";
import { CartProduct } from "../../components/CartProduct/CartProduct";
import {
  fetchDistricts,
  fetchProvinces,
  fetchWards,
} from "../../apis/location";
import { fetchShippingFee } from "../../apis/delivery";
import { LocationSelect } from "../../components/LocationSelect/LocationSelect";
import { useLocation } from "react-router-dom";
import { createOrder } from "../../apis/order";

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

const OrderValueHeader = styled(Typography)``;

const ShippingValueHeader = styled(Typography)``;

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

const user = {
  name: "Huy Hoàng",
  email: "email@gmail.com",
  phone: "033*****",
};

export const PaymentPage = () => {
  const [provinceList, setProvinceList] = useState([]);
  const [districtList, setDistrictList] = useState([]);
  const [wardList, setWardList] = useState([]);
  const [location, setLocation] = useState({
    provinceId: null,
    districtId: null,
    wardCode: null,
    provinceName: "",
    districtName: "",
    wardName: "",
  });
  const [shippingFee, setShippingFee] = useState(0);
  const [paymentMethod, setPaymentMethod] = useState("COD");

  const { state } = useLocation();

  const shippingAddress =
    location.provinceName +
    (location.provinceName && ", ") +
    location.districtName +
    (location.districtName && ", ") +
    location.wardName;

  const fetchLocationData = useCallback(
    async (fetchFunction, setState, param = null) => {
      const data = await fetchFunction(param);
      setState(data);
    },
    []
  );

  useEffect(() => {
    fetchLocationData(fetchProvinces, setProvinceList);
  }, [fetchLocationData]);

  useEffect(() => {
    if (location.provinceId) {
      fetchLocationData(fetchDistricts, setDistrictList, location.provinceId);
      const selectedProvince = provinceList.find(
        (p) => p.provinceId === location.provinceId
      );
      setLocation((prev) => ({
        ...prev,
        provinceName: selectedProvince ? selectedProvince.provinceName : "",
      }));
    }
  }, [location.provinceId, fetchLocationData, provinceList]);

  useEffect(() => {
    if (location.districtId) {
      fetchLocationData(fetchWards, setWardList, location.districtId);
      const selectedDistrict = districtList.find(
        (d) => d.districtID === location.districtId
      );
      setLocation((prev) => ({
        ...prev,
        districtName: selectedDistrict ? selectedDistrict.districtName : "",
      }));
    }
  }, [location.districtId, fetchLocationData, districtList]);

  useEffect(() => {
    if (location.wardCode) {
      const selectedWard = wardList.find(
        (w) => w.wardCode === location.wardCode
      );
      setLocation((prev) => ({
        ...prev,
        wardName: selectedWard ? selectedWard.wardName : "",
      }));
    }
  }, [location.wardCode, wardList]);

  useEffect(() => {
    const fetchFee = async () => {
      const shippingFeeParams = {
        toDistrictId: location.districtId,
        toWardCode: location.wardCode,
        insuranceValue: parseInt(state.totalPrice),
      };
      console.log(shippingFeeParams);
      const fee = await fetchShippingFee(shippingFeeParams);
      console.log("fee", fee);
      setShippingFee(fee);
    };

    location.wardCode && fetchFee();
  }, [location.wardCode]);

  const handleLocationChange = (key, value) => {
    setLocation((prev) => ({ ...prev, [key]: value }));
  };

  const handleCreateOrder = async () => {
    if (!location.wardCode) return;

    const order = {
      paymentMethod,
      shippingAddress,
      toProvinceId: location.provinceId,
      toDistrictId: location.districtId,
      toWardId: location.wardCode,
      discountCode: null,
      cartProductList: state.selectedCartProduct,
    };

    const res = await createOrder(order);
    console.log(res);
  };

  return (
    <div className="payment-page">
      <PaymentPageHeader>Thông tin giao hàng</PaymentPageHeader>

      <div className="payment-page--main">
        <div className="payment-page--main__left">
          <Accordion>
            <AccordionSummary expandIcon={<ExpandMoreIcon />}>
              <AccordionHeader>Thông tin đơn hàng</AccordionHeader>
            </AccordionSummary>
            <AccordionDetails>
              <div className="payment-page--main__left order-list">
                {state.selectedCartProduct.map((cartItem) => (
                  <CartProduct cartItem={cartItem} key={cartItem.id} />
                ))}
              </div>
            </AccordionDetails>
          </Accordion>

          <Accordion>
            <AccordionSummary expandIcon={<ExpandMoreIcon />}>
              <AccordionHeader>Thông tin khách hàng</AccordionHeader>
            </AccordionSummary>
            <AccordionDetails>
              <div className="payment-page--main__left customer-info">
                <div className="payment-page--main__left customer-info item">
                  <CustomerInfoHeader>Họ và tên: </CustomerInfoHeader>
                  <Typography>{user.name}</Typography>
                </div>
                <div className="payment-page--main__left customer-info item">
                  <CustomerInfoHeader>Email: </CustomerInfoHeader>
                  <Typography>{user.email}</Typography>
                </div>
                <div className="payment-page--main__left customer-info item">
                  <CustomerInfoHeader>Số điện thoại: </CustomerInfoHeader>
                  <Typography>{user.phone}</Typography>
                </div>

                <InputLabel id="address">Địa chỉ giao hàng</InputLabel>
                <CustomerAddressInput
                  id="address"
                  value={shippingAddress}
                  aria-readonly
                />

                <div className="payment-page--main__left customer-info item location">
                  <LocationSelect
                    label="Tỉnh/ Thành phố"
                    value={location.provinceId}
                    onChange={(value) =>
                      handleLocationChange("provinceId", value)
                    }
                    options={provinceList}
                    valueKey="provinceId"
                    labelKey="provinceName"
                  />

                  <LocationSelect
                    label="Huyện/ Thị xã"
                    value={location.districtId}
                    onChange={(value) =>
                      handleLocationChange("districtId", value)
                    }
                    options={districtList}
                    valueKey="districtID"
                    labelKey="districtName"
                  />
                  <LocationSelect
                    label="Quận/ Phường"
                    value={location.wardCode}
                    onChange={(value) =>
                      handleLocationChange("wardCode", value)
                    }
                    options={wardList}
                    valueKey="wardCode"
                    labelKey="wardName"
                  />
                </div>
              </div>
            </AccordionDetails>
          </Accordion>

          <Accordion>
            <AccordionSummary expandIcon={<ExpandMoreIcon />}>
              <AccordionHeader>Phương thức thanh toán</AccordionHeader>
            </AccordionSummary>
            <AccordionDetails>
              <RadioGroup
                aria-labelledby="payment-method"
                defaultValue="COD"
                name="radio-buttons-group"
                onChange={(e) => setPaymentMethod(e.target.value)}
              >
                <FormControlLabel
                  value="COD"
                  control={<Radio />}
                  label="Thanh toán khi nhận hàng"
                />
                <Divider />
                <FormControlLabel
                  value="VnPay"
                  control={<Radio />}
                  label="Chuyển khoản qua VNPAY"
                />
              </RadioGroup>
            </AccordionDetails>
          </Accordion>
        </div>
        <div className="payment-page--main__right">
          <div className="payment-page--main__right order">
            <OrderValueHeader>Giá trị đơn hàng</OrderValueHeader>
            <OrderValueHeader>{state?.totalPrice}đ</OrderValueHeader>
          </div>

          <div className="payment-page--main__right shipping">
            <ShippingValueHeader>Phí vận chuyển</ShippingValueHeader>
            <ShippingValueHeader>{shippingFee}đ</ShippingValueHeader>
          </div>

          <div className="payment-page--main__right price">
            <TotalPriceHeader>Thành tiền</TotalPriceHeader>
            <TotalPrice>{state?.totalPrice + shippingFee}đ</TotalPrice>
          </div>

          <PaymentButton variant="contained" onClick={handleCreateOrder}>
            Hoàn tất đơn hàng
          </PaymentButton>
        </div>
      </div>
    </div>
  );
};
