import React, { useEffect, useState } from "react";
import "./CartPage.scss";
import { Link } from "react-router-dom";
import {
  Button,
  Checkbox,
  Divider,
  List,
  ListItem,
  ListItemButton,
  ListItemText,
  styled,
  Typography,
} from "@mui/material";
import { CartProduct } from "../../components/CartProduct/CartProduct";
import { fetchCartProducts } from "../../apis/cart";

const CartPageHeader = styled(Typography)`
  font-weight: bold;
  font-size: 20px;
`;

const CartProductList = styled(List)`
  max-height: 600px;
  overflow: auto;
`;

const SelectAllListItemText = styled(ListItemText)`
  display: flex;
  justify-content: flex-end;
  padding-right: 30px;
`;
const SelectAllButtonHeader = styled(Typography)`
  font-weight: bold;
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
  const [cartProductList, setCartProductList] = useState([]);
  const [selectedCartProduct, setSelectedCartProduct] = React.useState([]);

  const totalPrice =
    selectedCartProduct?.length > 0
      ? selectedCartProduct.reduce((total, item) => {
          return total + item.product.price * item.quantity;
        }, 0)
      : cartProductList.totalPrice;

  useEffect(() => {
    const fetchCartProductList = async () => {
      const cartProductList = await fetchCartProducts();
      setCartProductList(cartProductList);
      setSelectedCartProduct(cartProductList.cartItems);
    };

    fetchCartProductList();
  }, []);

  const handleSelectedCartProduct = (value) => () => {
    const currentIndex = selectedCartProduct.indexOf(value);
    const newChecked = [...selectedCartProduct];

    if (currentIndex === -1) {
      newChecked.push(value);
    } else {
      newChecked.splice(currentIndex, 1);
    }

    setSelectedCartProduct(newChecked);
  };

  const handleSelectedAllCartProduct = () => {
    setSelectedCartProduct(
      JSON.stringify(selectedCartProduct) !==
        JSON.stringify(cartProductList.cartItems)
        ? cartProductList.cartItems
        : []
    );
  };

  return (
    <div className="cart-page">
      <CartPageHeader>GIỎ HÀNG</CartPageHeader>

      <div className="cart-page--main">
        <div className="cart-page--main__left">
          {cartProductList?.cartItems?.length > 0 ? (
            <CartProductList>
              <ListItem
                secondaryAction={
                  <Checkbox
                    edge="end"
                    onChange={handleSelectedAllCartProduct}
                    checked={
                      JSON.stringify(selectedCartProduct) ===
                      JSON.stringify(cartProductList.cartItems)
                    }
                  />
                }
              >
                <SelectAllListItemText>
                  <SelectAllButtonHeader>
                    {JSON.stringify(selectedCartProduct) !==
                    JSON.stringify(cartProductList.cartItems)
                      ? "Chọn tất cả"
                      : "Bỏ chọn tất cả"}
                  </SelectAllButtonHeader>
                </SelectAllListItemText>
              </ListItem>

              {cartProductList.cartItems.map((cartItem) => (
                <ListItem
                  disablePadding
                  secondaryAction={
                    <Checkbox
                      edge="end"
                      onChange={handleSelectedCartProduct(cartItem)}
                      checked={selectedCartProduct.includes(cartItem)}
                    />
                  }
                >
                  <ListItemButton>
                    <CartProduct cartItem={cartItem} key={cartItem.id} />
                  </ListItemButton>
                </ListItem>
              ))}
            </CartProductList>
          ) : (
            "Chưa có sản phẩm trong giỏ hàng"
          )}
        </div>

        <div className="cart-page--main__right">
          <div className="cart-page--main__right price">
            <TotalPriceHeader>Thành tiền</TotalPriceHeader>
            <TotalPrice>{parseInt(totalPrice) || 0}đ</TotalPrice>
          </div>

          <Link
            to="/payment"
            state={{ selectedCartProduct, totalPrice }}
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
