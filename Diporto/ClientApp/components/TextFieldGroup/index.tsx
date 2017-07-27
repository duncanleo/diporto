import * as React from 'react';

declare interface TextFieldGroupProps {
  field: string,
  value: string,
  label: string,
  type: string,
  onChange: (text) => void
}

const TextFieldGroup: React.SFC<TextFieldGroupProps> = ({ label, field, value, type, onChange }) => {
  return (
    <div className="mt3">
      <label className="db fw6 lh-copy f6">{label}</label>
      <input
        className="pa2 input-reset ba bg-transparent hover-bg-black hover-white w-100"
        type={type}
        name={field}
        value={value}
        onChange={onChange}
       />
    </div>
  )
};

export default TextFieldGroup;
