import { BrowserRouter, Routes, Route } from "react-router-dom";
import { HomePage } from "./pages/HomePage/HomePage";
import { ProductPage } from "./pages/ProductPage/ProductPage";
import { MainLayout } from "./layout/MainLayout";
import { EmptyLayout } from "./layout/EmptyLayout";
import { DetailPage } from "./pages/DetailPage/DetailPage";
import { CartPage } from "./pages/CartPage/CartPage";
import { PaymentPage } from "./pages/PaymentPage/PaymentPage";
import { NewsPage } from "./pages/NewsPage/NewsPage";
import { SupportPage } from "./pages/SupportPage/SupportPage";
import { LoginPage } from "./pages/LoginPage/LoginPage";
import { RegisterPage } from "./pages/RegisterPage/RegisterPage";
import { PrivateRoutes } from "./routes/PrivateRoutes";
import { AuthProvider } from "./contexts/AuthContext"; // Import AuthProvider

function App() {
  return (
    <AuthProvider>
      <BrowserRouter>
        <Routes>
          <Route element={<PrivateRoutes />}>
            <Route element={<MainLayout />}>
              <Route path="/cart" element={<CartPage />} />
              <Route path="/payment" element={<PaymentPage />} />
            </Route>
          </Route>

          <Route element={<MainLayout />}>
            <Route path="/" element={<HomePage />} />
            <Route path="/products" element={<ProductPage />} />
            <Route path="/products/:productId" element={<DetailPage />} />
            <Route path="/news" element={<NewsPage />} />
            <Route path="/support" element={<SupportPage />} />
          </Route>

          <Route element={<EmptyLayout />}>
            <Route path="/login" element={<LoginPage />} />
            <Route path="/register" element={<RegisterPage />} />
          </Route>
        </Routes>
      </BrowserRouter>
    </AuthProvider>
  );
}

export default App;
