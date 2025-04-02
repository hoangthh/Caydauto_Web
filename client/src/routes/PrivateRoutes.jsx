import { Navigate, Outlet } from "react-router-dom";
import { useAuth } from "../contexts/AuthContext";

export const PrivateRoutes = () => {
  const { isAuthenticated } = useAuth();

  if (isAuthenticated === undefined) {
    return <p>Loading...</p>; // Chờ xác thực
  }

  return isAuthenticated ? <Outlet /> : <Navigate to="/login" replace />;
};
