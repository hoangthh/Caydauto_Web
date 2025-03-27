import React from "react";
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
  margin-right: 10px;
  color: pink;
  cursor: pointer;
`;

const DecreaseQuantityButton = styled(RemoveCircleRoundedIcon)`
  margin-left: 10px;
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
          <ProductName>
            Hộp đựng bút học sinh tiện lợi Line Field Kokuyo Cây Đầu To
          </ProductName>
          {/*Main Product Name */}

          {/* Main Product Color */}
          <div className="detail-page--product__detail color">
            <input type="checkbox" className="input orange" />
            <input type="checkbox" className="input pink" />
            <input type="checkbox" className="input red" />
            <input type="checkbox" className="input black" />
          </div>
          {/* Main Product Color */}

          {/* Main Product Price */}
          <ProductPrice>250.000đ</ProductPrice>
          {/* Main Product Price */}

          {/* Main Product Quantity */}
          <div className="detail-page--product__detail quantity">
            <IncreaseQuantityButton />
            <ProductQuantity>5</ProductQuantity>
            <DecreaseQuantityButton />
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
            <AccordionDetails>
              Lorem ipsum dolor sit amet, consectetur adipiscing elit.
              Suspendisse malesuada lacus ex, sit amet blandit leo lobortis
              eget.
            </AccordionDetails>
          </DescriptionAccordion>
          {/* Main Product Description */}
        </div>
        {/* Main Product Detail */}
      </div>
      {/* Main Product */}

      {/* Similar Product */}
      <div className="detail-page--similar-product">
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
      </div>
      {/* Similar Product */}
    </div>
  );
};
