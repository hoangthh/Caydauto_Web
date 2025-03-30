import React, { useEffect, useState } from "react";
import { Typography } from "@mui/material";
import { Link, Outlet } from "react-router-dom";

export const PrivateRoutes = () => {
  const [isLoading, setIsLoading] = useState(false);
  const [user, setUser] = useState(null);

  useEffect(() => {
    const checkUser = async () => {
      setIsLoading(true);
      setIsLoading(false);
    };
    checkUser();
  });

  if (isLoading)
    return (
      <Typography>
        Loading to <Link to="/login">login</Link> ...
      </Typography>
    );

  return <Outlet />;
};
