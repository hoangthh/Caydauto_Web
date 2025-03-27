import React from "react";
import "./HomePage.scss";
import { Button, styled, Typography } from "@mui/material";
import banner from "../../assets/banner.svg";
import titleIntro from "../../assets/title-intro.svg";
import introIcon1 from "../../assets/intro-icon-1.svg";
import introIcon2 from "../../assets/intro-icon-2.svg";
import introIcon3 from "../../assets/intro-icon-3.svg";
import introIcon4 from "../../assets/intro-icon-4.svg";
import introIcon5 from "../../assets/intro-icon-5.svg";
import intro from "../../assets/intro.svg";
import distributor from "../../assets/distributor.svg";

const BuyButton = styled(Button)`
  background: green;
  margin-top: 30px;
`;

export const HomePage = () => {
  return (
    <div className="homepage">
      {/* Banner */}
      <img src={banner} className="homepage--banner" />
      {/* Banner */}

      {/* Intro */}
      <div className="homepage--intro">
        {/* Intro left */}
        <div className="homepage--intro__left">
          {/* Intro Title */}
          <img src={titleIntro} className="homepage--intro__left--title" />
          {/* Intro Title */}

          {/* Intro Info */}
          <div className="homepage--intro__left--info">
            <div className="homepage--intro__left--info item">
              <img src={introIcon1} />
              <Typography>Hotline: 033*******</Typography>
            </div>
            <div className="homepage--intro__left--info item">
              <img src={introIcon2} />
              <Typography>
                Địa chỉ: Lầu 1, số 35 nguyễn Văn Tráng, quận 1, Tp Hồ Chí minh
              </Typography>
            </div>
            <div className="homepage--intro__left--info item">
              <img src={introIcon3} />
              <Typography>Facebook: Cây Đầu To</Typography>
            </div>
            <div className="homepage--intro__left--info item">
              <img src={introIcon4} />
              <Typography>Instagram: @caydauto</Typography>
            </div>
            <div className="homepage--intro__left--info item">
              <img src={introIcon5} />
              <Typography>Tiktok: @caydauto.stationery</Typography>
            </div>

            <BuyButton variant="contained">
              Mua sắm ngay với Cây Đầu To
            </BuyButton>
          </div>
          {/* Intro Info */}
        </div>
        {/* Intro Left */}

        {/* Intro Right */}
        <img src={intro} className="intro--img" />
        {/* Intro Right */}
      </div>
      {/* Intro */}

      <img src={distributor} className="distributor" />
    </div>
  );
};
