import React, { useEffect, useState } from "react";
import "./AdminPage.scss";
import {
  AppBar,
  Box,
  Button,
  Drawer,
  IconButton,
  List,
  ListItem,
  ListItemButton,
  ListItemIcon,
  ListItemText,
  Toolbar,
  Typography,
} from "@mui/material";
import MenuIcon from "@mui/icons-material/Menu";
import CategoryIcon from "@mui/icons-material/Category";
import PaletteIcon from "@mui/icons-material/Palette";
import InboxIcon from "@mui/icons-material/MoveToInbox";
import MailIcon from "@mui/icons-material/Mail";

import { fetchOrderAll } from "../../apis/order";
import { fetchProductsWithFilterByPagination } from "../../apis/product";
import { fetchCategories } from "../../apis/category";
import { fetchColors } from "../../apis/color";

import { DataTable } from "../../components/DataTable/DataTable";
import { Add } from "../../components/Add/Add";

import { addCategory, addColor, addProduct, addOrder } from "../../apis/admin";

const drawerItems = [
  { label: "Orders", key: "orders", icon: <InboxIcon /> },
  { label: "Products", key: "products", icon: <MailIcon /> },
  { label: "Categories", key: "categories", icon: <CategoryIcon /> },
  { label: "Colors", key: "colors", icon: <PaletteIcon /> },
];

const columnsMap = {
  orders: [
    { field: "id", headerName: "ID", width: 40 },
    { field: "userId", headerName: "User ID", width: 60 },
    { field: "orderStatus", headerName: "Status", width: 100 },
    { field: "totalPrice", headerName: "Total", width: 80 },
    { field: "deliveryFee", headerName: "Delivery", width: 100 },
    { field: "paymentMethod", headerName: "Payment", width: 160 },
    { field: "orderDate", headerName: "Date", width: 130 },
    { field: "shippingAddress", headerName: "Address", width: 550 },
  ],
  products: [
    { field: "id", headerName: "ID", width: 50 },
    { field: "name", headerName: "Name", width: 200 },
    { field: "brand", headerName: "Brand", width: 100 },
    { field: "price", headerName: "Price", width: 100 },
    { field: "stockQuantity", headerName: "Stock", width: 100 },
    { field: "sold", headerName: "Sold", width: 100 },
  ],
  categories: [
    { field: "id", headerName: "ID", width: 50 },
    { field: "name", headerName: "Category Name", width: 200 },
    { field: "description", headerName: "Description", width: 500 },
  ],
  colors: [
    { field: "id", headerName: "ID", width: 50 },
    { field: "name", headerName: "Color", width: 150 },
    {
      field: "hexCode",
      headerName: "Hex Code",
      width: 150,
      renderCell: (params) => (
        <div style={{ display: "flex", alignItems: "center", gap: "8px" }}>
          <div
            style={{
              width: 20,
              height: 20,
              backgroundColor: params.value,
              border: "1px solid #ccc",
              borderRadius: "50%",
            }}
          />
          {params.value}
        </div>
      ),
    },
  ],
};

const fieldConfig = {
  categories: [
    { name: "name", label: "Category Name" },
    { name: "description", label: "Description" },
  ],
  colors: [
    { name: "name", label: "Color Name" },
    { name: "hexCode", label: "Hex Code", type: "color" },
  ],
  products: [
    { name: "name", label: "Product Name" },
    { name: "brand", label: "Brand" },
    { name: "price", label: "Price", type: "number" },
    { name: "stock", label: "Stock", type: "number" },
  ],
  orders: [
    { name: "userId", label: "User ID" },
    { name: "orderStatus", label: "Status" },
    { name: "totalPrice", label: "Total Price", type: "number" },
    { name: "deliveryFee", label: "Delivery Fee", type: "number" },
    { name: "paymentMethod", label: "Payment Method" },
    { name: "orderDate", label: "Order Date", type: "date" },
    { name: "shippingAddress", label: "Shipping Address" },
  ],
};

// mapping từ tab sang API tương ứng
const apiMap = {
  categories: addCategory,
  colors: addColor,
  products: addProduct,
  orders: addOrder,
};

export const AdminPage = () => {
  const [rows, setRows] = useState([]);
  const [openDrawer, setOpenDrawer] = useState(false);
  const [activeTab, setActiveTab] = useState("orders");
  const [openModal, setOpenModal] = useState(false);

  const handleOpenModal = () => {
    setOpenModal(true);
  };

  const handleCloseModal = () => {
    setOpenModal(false);
  };

  const toggleDrawer = (newOpen) => () => {
    setOpenDrawer(newOpen);
  };

  const fetchDataByTab = async (tabKey) => {
    switch (tabKey) {
      case "orders": {
        const data = await fetchOrderAll(1, 6);
        setRows(data);
        break;
      }
      case "products": {
        const data = await fetchProductsWithFilterByPagination(1, 6);
        setRows(data.items);
        break;
      }
      case "categories": {
        const data = await fetchCategories();
        setRows(data);
        break;
      }
      case "colors": {
        const data = await fetchColors();
        setRows(data);
        break;
      }
      default:
        setRows([]);
    }
  };

  useEffect(() => {
    fetchDataByTab(activeTab);
  }, [activeTab]);

  const DrawerList = (
    <Box sx={{ width: 250 }} role="presentation" onClick={toggleDrawer(false)}>
      <List>
        {drawerItems.map((item) => (
          <ListItem key={item.key} disablePadding>
            <ListItemButton onClick={() => setActiveTab(item.key)}>
              <ListItemIcon>{item.icon}</ListItemIcon>
              <ListItemText primary={item.label} />
            </ListItemButton>
          </ListItem>
        ))}
      </List>
    </Box>
  );

  return (
    <div>
      <AppBar position="static">
        <Toolbar>
          <IconButton
            size="large"
            edge="start"
            color="inherit"
            aria-label="menu"
            sx={{ mr: 2 }}
            onClick={toggleDrawer(true)}
          >
            <MenuIcon />
          </IconButton>
          <Typography variant="h6" sx={{ flexGrow: 1 }}>
            Admin Dashboard
          </Typography>

          <Button color="inherit">Login</Button>
        </Toolbar>
      </AppBar>

      <Drawer open={openDrawer} onClose={toggleDrawer(false)}>
        {DrawerList}
      </Drawer>

      <Box sx={{ padding: "16px" }}>
        <Typography variant="h5" sx={{ mb: 2 }}>
          {activeTab.charAt(0).toUpperCase() + activeTab.slice(1)}
        </Typography>
        <Button color="inherit" onClick={handleOpenModal}>
          Add {drawerItems.find((item) => item.key === activeTab)?.label}
        </Button>

        <DataTable rows={rows} columns={columnsMap[activeTab]} />
      </Box>

      <Add
        fields={fieldConfig[activeTab]}
        onSubmit={apiMap[activeTab]}
        open={openModal}
        handleClose={handleCloseModal}
      />
    </div>
  );
};
