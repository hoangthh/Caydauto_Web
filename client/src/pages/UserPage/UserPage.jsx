import { Box, styled, Tab, Tabs } from "@mui/material";
import React, { useEffect } from "react";
import { Profile } from "../../components/Profile/Profile";
import { useSearchParams } from "react-router-dom";
import { Favor } from "../../components/Favor/Favor";
import { Order } from "../../components/Order/Order";

function CustomTabPanel(props) {
  const { children, value, index, ...other } = props;

  return (
    <div
      role="tabpanel"
      hidden={value !== index}
      id={`simple-tabpanel-${index}`}
      aria-labelledby={`simple-tab-${index}`}
      {...other}
    >
      {value === index && <Box sx={{ p: 3 }}>{children}</Box>}
    </div>
  );
}

function a11yProps(index) {
  return {
    id: `simple-tab-${index}`,
    "aria-controls": `simple-tabpanel-${index}`,
  };
}

const StyledTab = styled(Tab)``;

export const UserPage = () => {
  const [value, setValue] = React.useState(0);

  const [searchParams] = useSearchParams();

  // Lấy giá trị của một query param cụ thể
  const tab = searchParams.get("tab") || 0;

  useEffect(() => {
    setValue(parseInt(tab));
  }, [tab]);

  const handleChange = (event, newValue) => {
    setValue(newValue);
  };

  return (
    <div>
      <Tabs
        value={value}
        onChange={handleChange}
        aria-label="basic tabs example"
      >
        <Tab label="Hồ sơ của bạn" {...a11yProps(0)} />
        <Tab label="Sản phẩm đã thích" {...a11yProps(1)} />
        <Tab label="Đơn hàng của bạn" {...a11yProps(2)} />
      </Tabs>
      <CustomTabPanel value={value} index={0}>
        <Profile />
      </CustomTabPanel>
      <CustomTabPanel value={value} index={1}>
        <Favor />
      </CustomTabPanel>
      <CustomTabPanel value={value} index={2}>
        <Order />
      </CustomTabPanel>
    </div>
  );
};
