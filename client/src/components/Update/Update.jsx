import React, { useState, useEffect } from "react";
import {
  Modal,
  Button,
  Select,
  MenuItem,
  FormControl,
  InputLabel,
} from "@mui/material";

export const Update = ({ open, handleClose, orderId, updateOrderStatus }) => {
  const [status, setStatus] = useState("");

  const handleChangeStatus = (event) => {
    setStatus(event.target.value);
  };

  const handleUpdate = () => {
    if (status) {
      updateOrderStatus(orderId, status); // Gọi API cập nhật trạng thái
      handleClose(); // Đóng modal
    } else {
      alert("Vui lòng chọn trạng thái");
    }
  };

  useEffect(() => {
    setStatus(""); // Reset trạng thái khi mở modal
  }, [open]);

  return (
    <Modal open={open} onClose={handleClose}>
      <div
        style={{
          padding: "20px",
          backgroundColor: "white",
          margin: "50px auto",
          maxWidth: "400px",
        }}
      >
        <h2>Cập nhật trạng thái đơn hàng</h2>
        <FormControl fullWidth>
          <InputLabel>Trạng thái</InputLabel>
          <Select
            value={status}
            onChange={handleChangeStatus}
            label="Trạng thái"
          >
            <MenuItem value="pending">Pending</MenuItem>
            <MenuItem value="processing">Processing</MenuItem>
            <MenuItem value="delivering">Delivering</MenuItem>
            <MenuItem value="delivered">Delivered</MenuItem>
            <MenuItem value="canceled">Canceled</MenuItem>
          </Select>
        </FormControl>

        <Button
          onClick={handleUpdate}
          variant="contained"
          color="primary"
          style={{ marginTop: "20px" }}
        >
          Cập nhật
        </Button>
        <Button
          onClick={handleClose}
          variant="outlined"
          style={{ marginTop: "10px" }}
        >
          Hủy
        </Button>
      </div>
    </Modal>
  );
};
