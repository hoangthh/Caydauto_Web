import { BrowserRouter, Routes, Route } from "react-router-dom";
import { HomePage } from "./pages/HomePage/HomePage";
import { ProductPage } from "./pages/ProductPage/ProductPage";
import { MainLayout } from "./layout/MainLayout";
import { DetailPage } from "./pages/DetailPage/DetailPage";
import { CartPage } from "./pages/CartPage/CartPage";
import { PaymentPage } from "./pages/PaymentPage/PaymentPage";
import { NewsPage } from "./pages/NewsPage/NewsPage";
import { SupportPage } from "./pages/SupportPage/SupportPage";

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<MainLayout />}>
          <Route index element={<HomePage />} />
          <Route path="products" element={<ProductPage />} />
          <Route path="products/:productId" element={<DetailPage />} />
          <Route path="cart" element={<CartPage />} />
          <Route path="payment" element={<PaymentPage />} />
          <Route path="news" element={<NewsPage />} />
          <Route path="support" element={<SupportPage />} />
        </Route>
      </Routes>
    </BrowserRouter>
  );
}

export default App;
