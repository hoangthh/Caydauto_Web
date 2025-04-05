import React, { useEffect, useState } from "react";
import "./Profile.scss";
import { List, ListItem, styled, Typography } from "@mui/material";
import { fetchProfile } from "../../apis/profile";
import moment from "moment";

const ProfileHeader = styled(Typography)`
  font-weight: bold;
`;

const ProfileInfoHeader = styled(Typography)`
  margin-left: 5px;
`;

export const Profile = () => {
  console.log("Profile rerender");
  const [profile, setProfile] = useState({});

  useEffect(() => {
    const fetchProfileUser = async () => {
      const profile = await fetchProfile();
      console.log(profile);
      setProfile(profile);
    };

    fetchProfileUser();
  }, []);

  return (
    <div className="profile">
      <img src={profile.imageUrl} className="profile--img" />
      <List>
        <ListItem>
          <div className="profile--info">
            <ProfileHeader>Họ và tên:</ProfileHeader>
            <ProfileInfoHeader>{profile.fullName}</ProfileInfoHeader>
          </div>
        </ListItem>
        <ListItem>
          <div className="profile--info">
            <ProfileHeader>Ngày sinh:</ProfileHeader>
            <ProfileInfoHeader>
              {moment(profile.dateOfBirth).locale("vi").format("DD/MM/YYYY")}
            </ProfileInfoHeader>
          </div>
        </ListItem>
        <ListItem>
          <div className="profile--info">
            <ProfileHeader>Email:</ProfileHeader>
            <ProfileInfoHeader>{profile.email}</ProfileInfoHeader>
          </div>
        </ListItem>
      </List>
    </div>
  );
};
