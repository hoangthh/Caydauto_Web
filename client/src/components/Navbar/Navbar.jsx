import React from "react";
import "./Navbar.scss";
import logo from "../../assets/logo.svg";
import titleLogo from "../../assets/title-logo.svg";
import favorIcon from "../../assets/favor-icon.svg";
import cartIcon from "../../assets/cart-icon.svg";
import profileIcon from "../../assets/profile-icon.svg";
import { InputAdornment, styled, TextField, Typography } from "@mui/material";
import SearchRoundedIcon from "@mui/icons-material/SearchRounded";
import { Link, useLocation } from "react-router-dom";

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

const navbarItemList = [
  { title: "Trang chủ", linkTo: "/" },
  { title: "Sản phẩm", linkTo: "/products" },
  { title: "Tin tức", linkTo: "/news" },
  { title: "Hỗ trợ", linkTo: "/support" },
];

export const Navbar = () => {
  // const [activeNavbarItem, setActiveNavbarItem] = useState("home");
  const location = useLocation();

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
            <Link to="/favor">
              <img src={favorIcon} className="navbar--activity__actions img" />
            </Link>
            <Link to="/cart">
              <img src={cartIcon} className="navbar--activity__actions img" />
            </Link>
            <Link to="/profile">
              <img
                src={profileIcon}
                className="navbar--activity__actions img"
              />
            </Link>
          </>
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
