import { DataGrid, GridToolbar } from "@mui/x-data-grid";
import React, { useState } from "react";
import { Update } from "../Update/Update"; // Đảm bảo rằng component Update được import chính xác
import { Link } from "react-router-dom";
import { updateOrder } from "../../apis/order";
import { useAlert } from "../../contexts/AlertContext";

const paginationModel = { page: 0, pageSize: 5 };

export const DataTable = ({ columns, rows }) => {
  const [openModal, setOpenModal] = useState(false);
  const [selectedId, setSelectedId] = useState(null); // Lưu ID phần tử cần cập nhật

  const { renderAlert } = useAlert();

  const actionColumn = {
    field: "action",
    headerName: "Action",
    width: 100,
    renderCell: (params) => {
      return (
        <div className="action" style={{ display: "flex" }}>
          <Link to={`/admin/`}>
            <i className="fa-solid fa-eye" style={{ color: "#63E6BE" }}></i>
          </Link>
          <div className="update" onClick={() => handleUpdate(params.row.id)}>
            <i
              className="fa-solid fa-wrench"
              style={{ color: "#74C0FC", cursor: "pointer" }}
            ></i>
          </div>

          <div className="delete" onClick={() => handleDelete(params.row.id)}>
            <i className="fa-solid fa-trash" style={{ color: "#ff0000" }}></i>
          </div>
        </div>
      );
    },
  };

  const handleUpdate = (id) => {
    setSelectedId(id); // Lưu ID phần tử cần cập nhật
    setOpenModal(true); // Mở modal
  };

  const handleDelete = (id) => {
    // Thực hiện xóa phần tử ở đây
    console.log("Xóa phần tử có ID: ", id);
  };

  const updateOrderStatus = async (id, newStatus) => {
    const response = await updateOrder(id, newStatus);

    if (response?.status === 200) {
      renderAlert(
        "success",
        "Cập nhật trạng thái đơn hàng thành công! Vui lòng tải lại trang"
      );
    } else {
      renderAlert("error", "Cập nhật trạng thái thất bại!");
    }
  };

  return (
    <>
      <DataGrid
        rows={rows}
        columns={[...columns, actionColumn]}
        initialState={{ pagination: { paginationModel } }}
        pageSizeOptions={[5, 10]}
        slots={{ toolbar: GridToolbar }}
        slotProps={{
          toolbar: {
            showQuickFilter: true,
            quickFilterProps: { debounceMs: 500 },
          },
        }}
        disableRowSelectionOnClick
        checkboxSelection
        sx={{ border: 0 }}
      />
      <Update
        open={openModal}
        handleClose={() => setOpenModal(false)}
        orderId={selectedId} // Truyền ID cần cập nhật vào Update component
        updateOrderStatus={updateOrderStatus} // Truyền hàm cập nhật trạng thái vào Update component
      />
    </>
  );
};
