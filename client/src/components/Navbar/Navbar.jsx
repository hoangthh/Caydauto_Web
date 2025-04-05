import React from "react";
import "./Navbar.scss";
import logo from "../../assets/logo.svg";
import titleLogo from "../../assets/title-logo.svg";
import favorIcon from "../../assets/favor-icon.svg";
import cartIcon from "../../assets/cart-icon.svg";
import profileIcon from "../../assets/profile-icon.svg";
import {
  Button,
  InputAdornment,
  styled,
  TextField,
  Typography,
} from "@mui/material";
import SearchRoundedIcon from "@mui/icons-material/SearchRounded";
import { Link, useLocation } from "react-router-dom";
import { useAuth } from "../../contexts/AuthContext";
import { logout } from "../../apis/auth";
import { useAlert } from "../../contexts/AlertContext";

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

const LoginButton = styled(Button)`
  text-transform: none;
  color: green;
`;
const RegisterButton = styled(Button)`
  text-transform: none;
  background: green;
  color: white;
`;

const LogoutButton = styled(Button)`
  text-transform: none;
  color: green;
  border: 1px solid green;

  &:hover {
    background: green;
    color: white;
  }
`;

const navbarItemList = [
  { title: "Trang chủ", linkTo: "/" },
  { title: "Sản phẩm", linkTo: "/products" },
  { title: "Tin tức", linkTo: "/news" },
  { title: "Hỗ trợ", linkTo: "/support" },
];

export const Navbar = () => {
  const location = useLocation();
  const { isAuthenticated, setIsAuthenticated } = useAuth();
  const { renderAlert } = useAlert();

  const handleLogout = async () => {
    const response = await logout();
    if (response.status === 200) {
      setIsAuthenticated(false);
      renderAlert("info", "Bạn vừa đăng xuất khỏi tài khoản");
    }
  };

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
          {isAuthenticated ? (
            <>
              <Link to="/cart">
                <img src={cartIcon} className="navbar--activity__actions img" />
              </Link>
              <Link to="/user?tab=0">
                <img
                  src={profileIcon}
                  className="navbar--activity__actions img"
                />
              </Link>
              <Link to="/user?tab=1">
                <img
                  src={favorIcon}
                  className="navbar--activity__actions img"
                />
              </Link>
              <LogoutButton variant="outlined" onClick={handleLogout}>
                Đăng xuất
              </LogoutButton>
            </>
          ) : (
            <>
              <LoginButton
                component="a"
                href="/login"
                variant="outlined"
                color="inherit"
              >
                Đăng nhập
              </LoginButton>
              <RegisterButton
                component="a"
                href="/register"
                variant="contained"
                color="inherit"
              >
                Đăng ký
              </RegisterButton>
            </>
          )}
        </div>
        {/* Actions */}
      </div>
      {/* Navbar Info */}

      <div className="navbar--wrapper">
        {navbarItemList.map((navbarItem) => (
          <Link
            key={navbarItem.linkTo}
            to={navbarItem.linkTo}
            style={{ textDecoration: "none", color: "black" }}
          >
            <NavbarItem
              className={
                location.pathname === navbarItem.linkTo ? "active" : ""
              }
            >
              {navbarItem.title}
            </NavbarItem>
          </Link>
        ))}
      </div>
    </div>
  );
};
