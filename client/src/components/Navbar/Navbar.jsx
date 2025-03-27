import React from "react";
import "./Navbar.scss";
import logo from "../../assets/logo.svg";
import titleLogo from "../../assets/title-logo.svg";
import favorIcon from "../../assets/favor-icon.svg";
import cartIcon from "../../assets/cart-icon.svg";
import profileIcon from "../../assets/profile-icon.svg";
import { InputAdornment, styled, TextField, Typography } from "@mui/material";
import SearchRoundedIcon from "@mui/icons-material/SearchRounded";
import { Link } from "react-router-dom";

const SearchInput = styled(TextField)`
  width: 100%;
`;

const NavbarItem = styled(Typography)`
  font-weight: bold;
  text-transform: uppercase;

  &:hover {
    color: green;
    text-decoration: underline;
    cursor: pointer;
  }
`;

export const Navbar = () => {
  return (
    <div className="navbar">
      {/* Navbar Info */}
      <div className="navbar--activity">
        {/* Logo */}
        <Link to="/">
          <div className="navbar--activity__logo">
            <img src={logo} />
            <img src={titleLogo} />
          </div>
        </Link>
        {/* Logo */}

        {/* Search Input */}
        <div className="navbar--activity__search">
          <SearchInput
            // label="Tìm kiếm"
            placeholder="Tìm kiếm"
            variant="outlined"
            slotProps={{
              input: {
                startAdornment: (
                  <InputAdornment position="start">
                    <SearchRoundedIcon />
                  </InputAdornment>
                ),
              },
            }}
          />
        </div>
        {/* Search Input */}

        {/* Actions */}
        <div className="navbar--activity__actions">
          {/* Before Login */}
          {/* <div>Đăng nhập</div>
        <div>Đăng ký</div> */}
          {/* After Login */}
          <>
            <img src={favorIcon} className="navbar--activity__actions img" />
            <img src={cartIcon} className="navbar--activity__actions img" />
            <img src={profileIcon} className="navbar--activity__actions img" />
          </>
        </div>
        {/* Actions */}
      </div>
      {/* Navbar Info */}

      <div className="navbar--wrapper">
        <Link to="/" style={{ textDecoration: "none", color: "black" }}>
          <NavbarItem>Trang chủ</NavbarItem>
        </Link>
        <Link to="/products" style={{ textDecoration: "none", color: "black" }}>
          <NavbarItem>Sản phẩm</NavbarItem>
        </Link>
        <Link to="/news" style={{ textDecoration: "none", color: "black" }}>
          <NavbarItem>Tin tức</NavbarItem>
        </Link>
        <Link to="/support" style={{ textDecoration: "none", color: "black" }}>
          <NavbarItem>Hỗ trợ</NavbarItem>
        </Link>
      </div>
    </div>
  );
};
