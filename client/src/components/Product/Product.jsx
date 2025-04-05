import React, { useState } from "react";
import "./Product.scss";
import productImg from "../../assets/product.svg";
import { Button, styled, Typography } from "@mui/material";
import { Link } from "react-router-dom";
import { convertNumberToPrice } from "../../helpers/string";
import FavoriteBorderRoundedIcon from "@mui/icons-material/FavoriteBorderRounded";
import FavoriteRoundedIcon from "@mui/icons-material/FavoriteRounded";
import { addToWishList, deleteFromWishList } from "../../apis/favor";
import { useAuth } from "../../contexts/AuthContext";
import { useAlert } from "../../contexts/AlertContext";

const ProductName = styled(Typography)`
  font-size: 13px;
`;

const AddFavorButton = styled(FavoriteBorderRoundedIcon)`
  color: green;

  &:hover {
    cursor: pointer;
  }
`;

const DeleteFavorButton = styled(FavoriteRoundedIcon)`
  color: green;

  &:hover {
    cursor: pointer;
  }
`;

const ProductPrice = styled(Typography)`
  font-weight: bold;
`;

const ProductSold = styled(Typography)`
  font-size: 13px;
`;

const BuyButton = styled(Button)`
  width: 100%;
  background: white;
  color: black;
  text-transform: none;
  margin-top: 10px;
  border: 1px solid black;

  &:hover {
    background: green;
    color: white;
  }
`;

export const Product = ({ product }) => {
  const [isWished, setIsWished] = useState(product.isWished);

  const { isAuthenticated } = useAuth();
  const { renderAlert } = useAlert();

  const handleAddFavor = async () => {
    if (!isAuthenticated) {
      renderAlert("info", "Vui lòng đăng nhập để thêm sản phẩm vào yêu thích");
      return;
    }
    const res = await addToWishList(product.id);
    if (res?.status === 200) {
      setIsWished(true);
      renderAlert("success", "Đã thêm sản phẩm vào danh sách yêu thích");
    }
  };

  const handleDeleteFavor = async () => {
    const res = await deleteFromWishList(product.id);
    if (res?.status === 200) {
      setIsWished(false);
      renderAlert("info", "Đã xóa sản phẩm khỏi danh sách yêu thích");
    }
  };

  return (
    <div className="product">
      <img
        src={product?.image?.url || product?.imageUrl || productImg}
        style={{ width: "100%" }}
      />
      <div className="product--name">
        <ProductName>{product.name}</ProductName>
        {isWished ? (
          <DeleteFavorButton onClick={handleDeleteFavor} />
        ) : (
          <AddFavorButton onClick={handleAddFavor} />
        )}
      </div>
      <ProductPrice>
        {convertNumberToPrice(parseInt(product.price))}
      </ProductPrice>
      <ProductSold>{product.sold} sold</ProductSold>
      <Link to={`/products/${product.id}`}>
        <BuyButton variant="contained">Xem chi tiết sản phẩm</BuyButton>
      </Link>
    </div>
  );
};
