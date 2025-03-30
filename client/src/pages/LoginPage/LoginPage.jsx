import React, { useState } from "react";
import "./LoginPage.scss";
import loginDecor from "../../assets/login-decor.svg";
import logo from "../../assets/logo.svg";
import titleLogo from "../../assets/title-logo.svg";
import { Button, styled, TextField, Typography } from "@mui/material";
import { Link, useNavigate } from "react-router-dom";
import { login } from "../../apis/auth";

const LoginHeader = styled(Typography)`
  font-weight: bold;
  font-size: 25px;
  margin-top: 20px;
`;

const EmailInput = styled(TextField)`
  margin-top: 20px;
  width: 60%;
`;

const PasswordInput = styled(TextField)`
  margin-top: 20px;
  width: 60%;
`;

const LoginButton = styled(Button)`
  margin-top: 20px;
  padding: 15px 50px;
  background: green;
  border-radius: 30px;
`;

export const LoginPage = () => {
  const [loginForm, setLoginForm] = useState({
    email: "",
    password: "",
  });

  const navigate = useNavigate();

  const handleChange = (e) => {
    setLoginForm({
      ...loginForm,
      [e.target.name]: e.target.value,
    });
  };

  const handleLogin = async () => {
    const response = await login(loginForm);
    if (response?.isSuccess) navigate("/");
    console.log(response);
  };

  return (
    <div className="login-page">
      <div className="login-page--left">
        <img src={loginDecor} className="login-page--left__img" />
      </div>

      <div className="login-page--right">
        <div>
          <img src={logo} />
          <img src={titleLogo} />
        </div>

        <LoginHeader>ĐĂNG NHẬP</LoginHeader>

        <div className="login-page--right__suggest-register">
          <Typography>Chưa có sẵn tài khoản?</Typography>
          <Link to="/register" style={{ marginLeft: "5px", fontSize: "17px" }}>
            Đăng ký
          </Link>
        </div>

        <EmailInput
          label="Email"
          placeholder="Email của bạn"
          name="email"
          onChange={handleChange}
        />

        <PasswordInput
          label="Mật khẩu"
          placeholder="Mật khẩu"
          name="password"
          onChange={handleChange}
        />

        <LoginButton variant="contained" onClick={handleLogin}>
          Đăng nhập
        </LoginButton>

        <Link to="/forgot-password" style={{ marginTop: "20px" }}>
          Quên mật khẩu
        </Link>
      </div>
    </div>
  );
};
