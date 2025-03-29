import React, { useEffect, useState } from "react";
import "./DetailPage.scss";
import product from "../../assets/product.svg";
import {
  Accordion,
  AccordionDetails,
  AccordionSummary,
  Button,
  styled,
  Typography,
} from "@mui/material";
import AddCircleRoundedIcon from "@mui/icons-material/AddCircleRounded";
import RemoveCircleRoundedIcon from "@mui/icons-material/RemoveCircleRounded";
import ExpandMoreIcon from "@mui/icons-material/ExpandMore";
import { Product } from "../../components/Product/Product";
import { useParams } from "react-router-dom";
import { fetchDetailProductById } from "../../apis/product";
import { fetchDetailProduct } from "../../mockData";

const ProductName = styled(Typography)`
  font-size: 20px;
  font-weight: bold;
`;

const ProductPrice = styled(Typography)`
  margin-top: 20px;
  font-size: 17px;
  font-weight: bold;
`;

const IncreaseQuantityButton = styled(AddCircleRoundedIcon)`
  margin-left: 10px;
  color: pink;
  cursor: pointer;
`;

const DecreaseQuantityButton = styled(RemoveCircleRoundedIcon)`
  margin-right: 10px;
  color: pink;
  cursor: pointer;
`;

const ProductQuantity = styled(Typography)``;

const AddToCartButton = styled(Button)`
  background: #fff;
  color: black;
  border: 1px solid black;
  text-transform: none;
`;

const BuyButton = styled(Button)`
  margin-left: 20px;
  text-transform: none;
  background: green;
`;

const DescriptionAccordion = styled(Accordion)`
  margin-top: 20px;
  background: transparent;
`;

const ProductDescription = styled(Typography)`
  font-weight: bold;
`;

const SimilarProductHeader = styled(Typography)`
  font-weight: bold;
  font-size: 20px;
  margin-top: 20px;
`;

export const DetailPage = () => {
  const [detailProduct, setDetailProduct] = useState({});
  const [color, setColor] = useState(null);
  const [quantity, setQuantity] = useState(1);

  const { productId } = useParams();

  useEffect(() => {
    const fetchDetailProduct = async () => {
      // const detailProduct = await fetchDetailProductById(productId);
      const detailProduct = await fetchDetailProduct(productId);
      setDetailProduct(detailProduct);
    };

    detailProduct && fetchDetailProduct();
  }, [detailProduct, productId]);

  const handleDecreaseQuantity = () => {
    if (quantity === 1) return;

    setQuantity(quantity - 1);
  };

  const handleIncreaseQuantity = () => {
    // if (quantity === 0) return;

    setQuantity(quantity + 1);
  };

  return (
    <div className="detail-page">
      {/* Main Product */}
      <div className="detail-page--product">
        {/* Main Product Image */}
        <div className="detail-page--product__image">
          <img src={product} className="" />
        </div>
        {/* Main Product Image */}

        {/* Main Product Detail */}
        <div className="detail-page--product__detail">
          {/*Main Product Name */}
          <ProductName>{detailProduct?.name}</ProductName>
          {/*Main Product Name */}

          {/* Main Product Color */}
          <div className="detail-page--product__detail color">
            <input
              type="checkbox"
              className="input orange"
              onChange={(e) => {
                console.log(e);
              }}
            />
            <input type="checkbox" className="input pink" />
            <input type="checkbox" className="input red" />
            <input type="checkbox" className="input black" />
          </div>
          {/* Main Product Color */}

          {/* Main Product Price */}
          <ProductPrice>{detailProduct.price}đ</ProductPrice>
          {/* Main Product Price */}

          {/* Main Product Quantity */}
          <div className="detail-page--product__detail quantity">
            <DecreaseQuantityButton onClick={handleDecreaseQuantity} />
            <ProductQuantity>{quantity}</ProductQuantity>
            <IncreaseQuantityButton onClick={handleIncreaseQuantity} />
          </div>
          {/* Main Product Quantity */}

          {/* Main Product Action */}
          <div className="detail-page--product__detail action">
            <AddToCartButton variant="contained">
              Thêm vào giỏ hàng
            </AddToCartButton>
            <BuyButton variant="contained">Mua ngay</BuyButton>
          </div>
          {/* Main Product Action */}

          {/* Main Product Description */}
          <DescriptionAccordion>
            <AccordionSummary expandIcon={<ExpandMoreIcon />}>
              <ProductDescription>Chi tiết sản phẩm</ProductDescription>
            </AccordionSummary>
            <AccordionDetails>{detailProduct.description}</AccordionDetails>
          </DescriptionAccordion>
          {/* Main Product Description */}
        </div>
        {/* Main Product Detail */}
      </div>
      {/* Main Product */}

      {/* Similar Product */}
      {/* <div className="detail-page--similar-product">
        <SimilarProductHeader>Sản phẩm tương tự</SimilarProductHeader>
        <div className="detail-page--similar-product product-wrapper">
          <div className="detail-page--similar-product product-item">
            <Product />
          </div>
          <div className="detail-page--similar-product product-item">
            <Product />
          </div>
          <div className="detail-page--similar-product product-item">
            <Product />
          </div>
          <div className="detail-page--similar-product product-item">
            <Product />
          </div>
          <div className="detail-page--similar-product product-item">
            <Product />
          </div>
          <div className="detail-page--similar-product product-item">
            <Product />
          </div>
        </div>
      </div> */}
      {/* Similar Product */}
    </div>
  );
};
