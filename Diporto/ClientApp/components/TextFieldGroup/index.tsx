import * as React from 'react';

declare interface TextFieldGroupProps {
  field: string,
  value: string,
  label: string,
  type: string,
  required?: boolean,
  onChange: (text) => void
}

const defaultProps: TextFieldGroupProps = {
  field: '',
  value: '',
  label: '',
  type: '',
  required: false,
  onChange: () => {}
}

const TextFieldGroup: React.SFC<TextFieldGroupProps> = ({ label, field, value, type, required, onChange }) => {
  return (
    <div className="mt3">
      <label className="db fw6 lh-copy f6">{label}</label>
      <input
        className="pa2 input-reset ba bg-transparent hover-bg-black hover-white w-100"
        type={type}
        name={field}
        value={value}
        required={required}
        onChange={onChange}
       />
    </div>
  )
};

TextFieldGroup.defaultProps = defaultProps;

export default TextFieldGroup;
