import { BrowserRouter, Routes, Route } from "react-router-dom";
import { HomePage } from "./pages/HomePage/HomePage";
import { ProductPage } from "./pages/ProductPage/ProductPage";
import { MainLayout } from "./layout/MainLayout";
import { EmptyLayout } from "./layout/EmptyLayout";
import { DetailPage } from "./pages/DetailPage/DetailPage";
import { CartPage } from "./pages/CartPage/CartPage";
import { PaymentPage } from "./pages/PaymentPage/PaymentPage";
import { PaymentSuccessPage } from "./pages/PaymentSuccessPage/PaymentSuccessPage";
import { NewsPage } from "./pages/NewsPage/NewsPage";
import { SupportPage } from "./pages/SupportPage/SupportPage";
import { LoginPage } from "./pages/LoginPage/LoginPage";
import { RegisterPage } from "./pages/RegisterPage/RegisterPage";
import { PrivateRoutes } from "./routes/PrivateRoutes";
import { AuthProvider } from "./contexts/AuthContext";
import { UserPage } from "./pages/UserPage/UserPage";
import { AlertProvider } from "./contexts/AlertContext";
import { AdminPage } from "./pages/AdminPage/AdminPage";

function App() {
  return (
    <AuthProvider>
      <AlertProvider>
        <BrowserRouter>
          <Routes>
            <Route element={<PrivateRoutes />}>
              <Route element={<MainLayout />}>
                <Route path="/user" element={<UserPage />} />
                <Route path="/cart" element={<CartPage />} />
                <Route path="/payment" element={<PaymentPage />} />
                <Route
                  path="/payment/success"
                  element={<PaymentSuccessPage />}
                />
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

            <Route path="/admin" element={<AdminPage />} />
          </Routes>
        </BrowserRouter>
      </AlertProvider>
    </AuthProvider>
  );
}

export default App;
