import React, { useEffect, useState } from "react";
import "./CartPage.scss";
import { Link } from "react-router-dom";
import {
  Button,
  Checkbox,
  List,
  ListItem,
  ListItemButton,
  ListItemText,
  styled,
  Typography,
} from "@mui/material";
import { CartProduct } from "../../components/CartProduct/CartProduct";
import { fetchCartProducts, removeProductFromCart } from "../../apis/cart";
import { convertNumberToPrice } from "../../helpers/string";
import { useAlert } from "../../contexts/AlertContext";

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
  padding-right: 50px;
`;

const SelectAllButtonHeader = styled(Typography)`
  font-weight: bold;
`;

const SelectAllButton = styled(Checkbox)`
  margin-right: 15px;
`;

const DeleteCartProductButton = styled(Button)`
  margin-left: 15px;
  text-transform: none;
  color: red;
  border: 1px solid red;

  &:hover {
    background: red;
    color: white;
  }
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

  const { renderAlert } = useAlert();
  console.log(selectedCartProduct);
  const totalPrice =
    selectedCartProduct?.length > 0
      ? selectedCartProduct.reduce((total, item) => {
          return total + item.product.price * item.quantity;
        }, 0)
      : 0;

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

  const handleRemoveCartProduct = (cartItemId) => async () => {
    const response = await removeProductFromCart(cartItemId);
    if (response?.status === 200)
      renderAlert("success", "Xóa sản phẩm khỏi giỏ hàng thành công");
    else renderAlert("warning", "Xóa sản phẩm khỏi giỏ hàng thất bại");
    setCartProductList({
      ...cartProductList,
      cartItems: cartProductList.cartItems.filter(
        (cartItem) => cartItem.id !== cartItemId
      ),
    });
    setSelectedCartProduct(
      cartProductList.cartItems.filter((cartItem) => cartItem.id !== cartItemId)
    );
  };

  const handleQuantityChange = (cartItemId, newQuantity) => {
    // Cập nhật trong cartProductList nếu cần
    const updatedCartItems = cartProductList.cartItems.map((item) =>
      item.id === cartItemId ? { ...item, quantity: newQuantity } : item
    );
    setCartProductList({ ...cartProductList, cartItems: updatedCartItems });

    // Cập nhật trong selectedCartProduct
    const updatedSelected = selectedCartProduct.map((item) =>
      item.id === cartItemId ? { ...item, quantity: newQuantity } : item
    );
    setSelectedCartProduct(updatedSelected);
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
                  <SelectAllButton
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
                    <div
                      style={{
                        display: "flex",
                        flexDirection: "column",
                        justifyContent: "center",
                        alignItems: "center",
                      }}
                    >
                      <Checkbox
                        edge="end"
                        onChange={handleSelectedCartProduct(cartItem)}
                        checked={selectedCartProduct.includes(cartItem)}
                      />
                      <DeleteCartProductButton
                        variant="outlined"
                        onClick={handleRemoveCartProduct(cartItem.id)}
                      >
                        Xóa
                      </DeleteCartProductButton>
                    </div>
                  }
                >
                  <ListItemButton>
                    <CartProduct
                      cartItem={cartItem}
                      key={cartItem.id}
                      flexBasisRightInfo={"35%"}
                      onQuantityChange={(newQuantity) =>
                        handleQuantityChange(cartItem.id, newQuantity)
                      }
                    />
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
            <TotalPrice>
              {convertNumberToPrice(parseInt(totalPrice)) || 0}
            </TotalPrice>
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
