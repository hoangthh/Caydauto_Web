import React, { useState } from "react";
import "./Product.scss";
import productImg from "../../assets/product.svg";
import { Button, styled, Typography } from "@mui/material";
import { Link } from "react-router-dom";
import { convertNumberToPrice } from "../../helpers/string";
import FavoriteBorderRoundedIcon from "@mui/icons-material/FavoriteBorderRounded";
import FavoriteRoundedIcon from "@mui/icons-material/FavoriteRounded";
import { addToWishList, deleteFromWishList } from "../../apis/favor";

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
  const [isWished, setIsWished] = useState(product.isWished || true);

  const handleAddFavor = async () => {
    const res = await addToWishList(product.id);
    if (res?.status === 200) setIsWished(true);
  };

  const handleDeleteFavor = async () => {
    const res = await deleteFromWishList(product.id);
    if (res?.status === 200) setIsWished(false);
  };

  return (
    <div className="product">
      <img src={productImg} style={{ width: "100%" }} />
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
        <BuyButton variant="contained">Mua hàng</BuyButton>
      </Link>
    </div>
  );
};
