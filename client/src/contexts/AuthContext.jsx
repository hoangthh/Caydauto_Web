import React, { createContext, useContext, useEffect, useState } from "react";
import { checkAuthenticated } from "../apis/auth";

// Tạo Context
const AuthContext = createContext(null);

// Hook để sử dụng AuthContext
export const useAuth = () => {
  return useContext(AuthContext);
};

export const AuthProvider = ({ children }) => {
  const [isAuthenticated, setIsAuthenticated] = useState(undefined); // undefined để biết khi nào chưa load xong

  useEffect(() => {
    const checkUser = async () => {
      try {
        const response = await checkAuthenticated();

        if (response.status === 200) {
          setIsAuthenticated(true);
        } else {
          setIsAuthenticated(false);
        }
      } catch (error) {
        console.log(error);
        setIsAuthenticated(false);
      }
    };

    checkUser();
  }, []);

  return (
    <AuthContext.Provider value={{ isAuthenticated, setIsAuthenticated }}>
      {children}
    </AuthContext.Provider>
  );
};
