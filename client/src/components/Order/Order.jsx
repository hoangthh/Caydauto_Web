import React, { useEffect, useState } from "react";
import { fetchOrderByUser } from "../../apis/order";
import { OrderList } from "../OrderList/OrderList";
import { List, ListItem, ListItemButton } from "@mui/material";

export const Order = () => {
  const [orderList, setOrderList] = useState([]);

  useEffect(() => {
    const fetchOrderListByUser = async () => {
      const orderList = await fetchOrderByUser(1, 6);

      setOrderList(orderList);
    };
    fetchOrderListByUser();
  }, []);

  return (
    <div>
      {orderList?.length > 0 && (
        <List>
          {orderList.map((order) => (
            <ListItem>
              <OrderList order={order} />
            </ListItem>
          ))}
        </List>
      )}
    </div>
  );
};
