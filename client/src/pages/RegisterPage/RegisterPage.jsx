import React, { useState } from "react";
import "./RegisterPage.scss";
import loginDecor from "../../assets/login-decor.svg";
import logo from "../../assets/logo.svg";
import titleLogo from "../../assets/title-logo.svg";
import { Button, styled, TextField, Typography } from "@mui/material";
import { Link } from "react-router-dom";
import { register } from "../../apis/auth";

const LoginHeader = styled(Typography)`
  font-weight: bold;
  font-size: 25px;
  margin-top: 20px;
`;

const FullNameInput = styled(TextField)`
  margin-top: 20px;
  width: 60%;
`;

const EmailInput = styled(TextField)`
  margin-top: 20px;
  width: 60%;
`;

const NoticeText = styled(Typography)`
  margin-top: 5px;
  max-width: 60%;
`;

const PasswordInput = styled(TextField)`
  margin-top: 20px;
  width: 60%;
`;

const RegisterButton = styled(Button)`
  margin-top: 20px;
  padding: 15px 50px;
  background: green;
  border-radius: 30px;
`;

const WarningText = styled(Typography)`
  margin-top: 15px;
  color: red;
`;

export const RegisterPage = () => {
  const [registerForm, setRegisterForm] = useState({
    fullName: "",
    email: "",
    password: "",
  });
  const [isNullEmail, setIsNullEmail] = useState(true);
  const [isInvalidForm, setIsInvalidForm] = useState(false);

  const handleChange = (e) => {
    setRegisterForm({
      ...registerForm,
      [e.target.name]: e.target.value,
    });
    if (registerForm.email) setIsNullEmail(false);
    if (registerForm.fullName && registerForm.email && registerForm.password)
      setIsInvalidForm(false);
  };

  const handleRegister = async () => {
    console.log(registerForm);
    if (
      !registerForm.fullName ||
      !registerForm.email ||
      !registerForm.password
    ) {
      setIsInvalidForm(true);
      return;
    }

    const response = await register(registerForm);
    // if (response.isSuccess) navigate("/")
    console.log(response);
  };

  return (
    <div className="register-page">
      <div className="login-page--left">
        <img src={loginDecor} className="login-page--left__img" />
      </div>

      <div className="login-page--right">
        <div>
          <img src={logo} />
          <img src={titleLogo} />
        </div>

        <LoginHeader>TẠO TÀI KHOẢN MỚI</LoginHeader>

        <div className="login-page--right__suggest-register">
          <Typography>Đã có tài khoản?</Typography>
          <Link to="/login" style={{ marginLeft: "5px", fontSize: "17px" }}>
            Đăng nhập
          </Link>
        </div>

        <FullNameInput
          label="Họ và tên"
          placeholder="Họ và tên của bạn"
          name="fullName"
          onChange={handleChange}
        />

        <EmailInput
          label="Email"
          placeholder="Email của bạn"
          name="email"
          onChange={handleChange}
        />

        {!isNullEmail && (
          <NoticeText>
            *Lưu ý: Bạn sẽ cần dùng Email trên để kích hoạt tài khoản
          </NoticeText>
        )}

        <PasswordInput
          label="Mật khẩu"
          placeholder="Mật khẩu của bạn"
          name="password"
          onChange={handleChange}
        />

        {isInvalidForm && (
          <WarningText>*Vui lòng điền đẩy đủ thông tin</WarningText>
        )}

        <RegisterButton variant="contained" onClick={handleRegister}>
          Tạo tài khoản mới
        </RegisterButton>
      </div>
    </div>
  );
};
