import { List, ListItemButton, Typography } from "@mui/material";
import React from "react";
import "./OrderList.scss";
import { CartProduct } from "../CartProduct/CartProduct";

export const OrderList = ({ order }) => {
  return (
    <div className="order-list">
      <div className="order-list--header">
        <Typography>Mã đơn hàng: {order.id}</Typography>
        <Typography>Trạng thái đơn hàng: {order.orderStatus}</Typography>
      </div>
      {order.orderItems?.length && (
        <List>
          {order.orderItems?.map((cartItem) => (
            <ListItemButton disablePadding>
              <CartProduct cartItem={cartItem} changeQuantity={false} />
            </ListItemButton>
          ))}
        </List>
      )}
    </div>
  );
};
