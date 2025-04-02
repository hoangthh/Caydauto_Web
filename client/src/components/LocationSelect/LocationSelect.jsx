import { InputLabel, MenuItem, Select, styled } from "@mui/material";
import React from "react";
import "./LocationSelect.scss";

const StyledSelect = styled(Select)`
  width: 100%;
`;

export const LocationSelect = ({
  label,
  value,
  onChange,
  options,
  valueKey,
  labelKey,
}) => {
  return (
    <div className="location-select">
      <InputLabel>{label}</InputLabel>
      <StyledSelect
        value={value?.toString() || ""}
        onChange={(e) => onChange(e.target.value)}
        autoWidth
      >
        {options.map((option) => (
          <MenuItem key={option[valueKey]} value={option[valueKey]}>
            {option[labelKey]}
          </MenuItem>
        ))}
      </StyledSelect>
    </div>
  );
};
