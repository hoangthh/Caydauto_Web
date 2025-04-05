import React, { useEffect, useState } from "react";
import "./DetailPage.scss";
import product from "../../assets/product.svg";
import {
  Accordion,
  AccordionDetails,
  AccordionSummary,
  CircularProgress,
  Button,
  styled,
  Typography,
} from "@mui/material";
import AddCircleRoundedIcon from "@mui/icons-material/AddCircleRounded";
import RemoveCircleRoundedIcon from "@mui/icons-material/RemoveCircleRounded";
import ExpandMoreIcon from "@mui/icons-material/ExpandMore";
import { Product } from "../../components/Product/Product";
import { Link, useParams } from "react-router-dom";
import {
  fetchDetailProductById,
  fetchSimilarProductsById,
} from "../../apis/product";
import { addProductToCart } from "../../apis/cart";
import { convertNumberToPrice } from "../../helpers/string";
import { useAuth } from "../../contexts/AuthContext";
import { useAlert } from "../../contexts/AlertContext";

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
  const [isLoading, setIsLoading] = useState(true);
  const [detailProduct, setDetailProduct] = useState({});
  const [colorList, setColorList] = useState(null);
  const [quantity, setQuantity] = useState(1);
  const [similarProductList, setSimilarProductList] = useState([]);
  const [selectedColor, setSelectedColor] = useState({
    id: null,
    name: null,
    hexCode: null,
  });

  const { productId } = useParams();
  const { isAuthenticated } = useAuth();
  const { renderAlert } = useAlert();

  useEffect(() => {
    const fetchDetailProduct = async () => {
      const detailProduct = await fetchDetailProductById(productId);
      setDetailProduct(detailProduct);
      setColorList(detailProduct.colors);
    };

    const fetchSimilarProductList = async () => {
      const similarProductList = await fetchSimilarProductsById(productId);
      setSimilarProductList(similarProductList);
    };

    setIsLoading(true);
    fetchDetailProduct();
    fetchSimilarProductList();
    setIsLoading(false);
  }, []);

  const handleDecreaseQuantity = () => {
    if (quantity === 1) return;
    setQuantity(quantity - 1);
  };

  const handleIncreaseQuantity = () => {
    setQuantity(quantity + 1);
  };

  const handleAddToCart = async () => {
    if (!isAuthenticated) {
      renderAlert("info", "Vui lòng đăng nhập để thêm vào giỏ hàng");
      return;
    }
    if (!productId || !selectedColor.id || !quantity) {
      renderAlert("info", "Vui lòng chọn màu");
      return;
    }
    const response = await addProductToCart({
      productId,
      colorId: selectedColor.id,
      quantity,
    });
    if (response?.status === 200)
      renderAlert("success", "Thêm vào giỏ hàng thành công");
  };

  const handleBuy = () => {
    if (!isAuthenticated) {
      renderAlert("info", "Vui lòng đăng nhập trước khi mua hàng");
      return;
    }
  };

  const buyState = {
    selectedCartProduct: [
      {
        product: {
          id: detailProduct.id,
          name: detailProduct.name,
          price: detailProduct.price,
          imageUrl:
            detailProduct.images?.length > 0
              ? detailProduct.images[0].url
              : null,
        },
        color: {
          id: selectedColor.id,
          name: selectedColor.name,
          hexCode: selectedColor.hexCode,
        },
        quantity: 1,
      },
    ],
    totalPrice: detailProduct.price,
  };

  if (isLoading) return <CircularProgress />;

  if (!detailProduct) return <Typography>Không có sản phẩm này</Typography>;

  return (
    <div className="detail-page">
      {/* Main Product */}
      <div className="detail-page--product">
        {/* Main Product Image */}
        <div className="detail-page--product__image">
          <img
            src={
              detailProduct.images?.length > 0
                ? detailProduct.images[0].url
                : product
            }
            className=""
          />
          {detailProduct.images?.length > 0 &&
            detailProduct.images
              .slice(0, 5)
              .map((image) => (
                <img
                  src={image.url}
                  key={image.id}
                  style={{ width: "calc(100%/5)" }}
                />
              ))}
        </div>
        {/* Main Product Image */}

        {/* Main Product Detail */}
        <div className="detail-page--product__detail">
          {/*Main Product Name */}
          <ProductName>{detailProduct?.name}</ProductName>
          {/*Main Product Name */}

          {/* Main Product Color */}
          <div className="detail-page--product__detail color">
            {colorList?.map((color) => (
              <input
                key={color.id}
                type="checkbox"
                className={`input detail-color ${color.name}`}
                value={color.name}
                checked={selectedColor.id === color.id}
                onChange={() => setSelectedColor(color)}
              />
            ))}
          </div>
          {/* Main Product Color */}

          {/* Main Product Price */}
          <ProductPrice>
            {convertNumberToPrice(parseInt(detailProduct.price))}
          </ProductPrice>
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
            <AddToCartButton variant="contained" onClick={handleAddToCart}>
              Thêm vào giỏ hàng
            </AddToCartButton>
            <Link to="/payment" state={buyState}>
              <BuyButton variant="contained" onClick={handleBuy}>
                Mua ngay
              </BuyButton>
            </Link>
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

          {/* Main Product Description */}
          <DescriptionAccordion>
            <AccordionSummary expandIcon={<ExpandMoreIcon />}>
              <ProductDescription>Đánh giá sản phẩm</ProductDescription>
            </AccordionSummary>
            <AccordionDetails>
              {detailProduct.avarageRating ||
                "Hiện chưa có đánh giá về sản phẩm này"}
            </AccordionDetails>
          </DescriptionAccordion>
          {/* Main Product Description */}
        </div>
        {/* Main Product Detail */}
      </div>
      {/* Main Product */}

      {/* Similar Product */}
      {similarProductList?.length > 0 && (
        <div className="detail-page--similar-product">
          <SimilarProductHeader>Sản phẩm tương tự</SimilarProductHeader>
          <div className="detail-page--similar-product product-wrapper">
            {similarProductList.map((product) => (
              <div
                className="detail-page--similar-product product-item"
                key={product.id}
              >
                <Product product={product} />
              </div>
            ))}
          </div>
        </div>
      )}
      {/* Similar Product */}
    </div>
  );
};
