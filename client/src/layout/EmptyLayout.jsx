import React from "react";
import { Container } from "@mui/material";
import { Outlet } from "react-router-dom";

export const EmptyLayout = () => {
  return (
    <Container>
      <Outlet />
    </Container>
  );
};
